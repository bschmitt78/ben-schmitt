using System.ComponentModel.DataAnnotations;

namespace ResumeProjects.Api.Data.ViewModels
{
    public class TokenRequestViewModel
    {
        [Required]
        public string? Token { get; set; }

        [Required]
        public string? RefreshToken { get; set; }
    }
}
