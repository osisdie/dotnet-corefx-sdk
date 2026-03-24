namespace CoreFX.Abstractions.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public int AccessTokenExpireMins { get; set; }
        public int RefreshTokenExpireMins { get; set; }
    }
}
