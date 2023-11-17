namespace MauiWithAPI.API.Services.Authentication.Dto
{
    public class AuthResponse
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
