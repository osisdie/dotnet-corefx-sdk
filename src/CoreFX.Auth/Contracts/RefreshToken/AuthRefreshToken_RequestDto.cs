using System.ComponentModel.DataAnnotations;

namespace CoreFX.Auth.Contracts.RefreshToken
{
    public class AuthRefreshToken_RequestDto
    {
        [Required]
        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
