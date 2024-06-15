namespace Entities.Auth
{
    public class AuthContext
    {
        public const string Issure = "https://localhost:7150";
        public const string Audience = "http://localhost:5243";
        public const string Secret = "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr";
        public int RefreshTokenInMinute = 15;
        public int RefreshTokenInHour = 1;
        public int RefreshTokenInDay = 1;
    }
}
