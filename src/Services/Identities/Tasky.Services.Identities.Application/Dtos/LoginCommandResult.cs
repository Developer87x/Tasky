namespace Tasky.Services.Identities.Application.Dtos;

public class LoginCommandResult
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
