namespace ComingHereShared.DTO
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = "";
    }
}
