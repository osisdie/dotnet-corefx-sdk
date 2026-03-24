namespace CoreFX.Auth.Consts
{
    public sealed class JwtConst
    {
        public const string JwtHeaderPrefix = "Bearer ";
        public const string JwtTokenItemName = "JwtToken";

        public const string JwtExpiredMsg = @"JWT expired";
        public const string JwtInvalidMsg = @"JWT invalid";
    }
}
