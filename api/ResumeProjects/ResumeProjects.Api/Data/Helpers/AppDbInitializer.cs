using Microsoft.AspNetCore.Identity;

namespace ResumeProjects.Api.Data.Helpers
{
    public class AppDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                }

                if (!await roleManager.RoleExistsAsync(UserRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.User));
                }


            }


        }
    }
}
