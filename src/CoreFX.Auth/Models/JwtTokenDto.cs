using System;

namespace CoreFX.Auth.Models
{
    public class JwtTokenDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        //public string FullName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        //public string Email { get; set; }
        public DateTime? Exp { get; set; }
        public string _id { get; set; } = Guid.NewGuid().ToString();
    }
}
