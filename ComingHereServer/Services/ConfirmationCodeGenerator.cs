namespace ComingHereServer.Services
{
    public class ConfirmationCodeGenerator
    {
        public string GenerateCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
