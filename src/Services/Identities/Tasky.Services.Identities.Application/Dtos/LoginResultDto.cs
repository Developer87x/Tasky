namespace Tasky.Services.Identities.Application.Dtos
{
    public class LoginResultDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }


}
