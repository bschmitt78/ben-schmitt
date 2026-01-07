using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResumeProjects.Api.Data;
using ResumeProjects.Api.Data.Helpers;
using ResumeProjects.Api.Data.Models;
using ResumeProjects.Api.Data.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ResumeProjects.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration
,
            TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out the required fields");

            }

            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);

            if (user != null)
            {
                return BadRequest($"The user with email address {registerVm.EmailAddress} alrewady exists");
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = registerVm.Username,
                Email = registerVm.EmailAddress,
                FirstName = registerVm.FirstName,
                LastName = registerVm.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, registerVm.Password);

            if (result.Succeeded)
            {
                switch (registerVm.Role)
                {
                    case UserRole.Admin:
                        await _userManager.AddToRoleAsync(newUser, UserRole.Admin);
                        break;
                    default:
                        await _userManager.AddToRoleAsync(newUser, UserRole.User);
                        break;
                }

                return Ok($"User with email address {registerVm.EmailAddress} has been registered successfully");
            }

            return BadRequest("An error occurred while registering the user");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out the required fields");
            }

            var user = await _userManager.FindByNameAsync(loginVm.EmailAddress);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginVm.Password))
            {
                var token = await GenerateJwtTokenAsync(user, null);

                return Ok(token);
            }

            return Unauthorized();

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestViewModel tokenRequestVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out the required fields");
            }
            var result = await VerifyAndGenerateTokenAsync(tokenRequestVM);

            return Ok(result);
        }

        private async Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequestViewModel tokenRequestVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token ==
            tokenRequestVM.RefreshToken);

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestVM.Token,
                    _tokenValidationParameters,
                    out var validatedToken);

                return await GenerateJwtTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                if (storedToken.DateExpires >= DateTime.UtcNow)
                {
                    return await GenerateJwtTokenAsync(dbUser, storedToken);
                }
                else
                {
                    return await GenerateJwtTokenAsync(dbUser, null);
                }
            }
        }

        private async Task<AuthResultViewModel> GenerateJwtTokenAsync(ApplicationUser user, RefreshToken refreshToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //add roles to claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(refreshToken is not null)
            {
               var refreshTokenResponse = new AuthResultViewModel()
               {
                   Token = jwtToken,
                   RefreshToken = refreshToken.Token,
                   ExpiresAt = token.ValidTo
               };

               return refreshTokenResponse;
            }

            var newRefreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpires = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultViewModel()
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return response;
        }
    }
}
