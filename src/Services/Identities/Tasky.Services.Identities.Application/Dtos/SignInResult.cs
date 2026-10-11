namespace Tasky.Services.Identities.Application.Dtos;

public class SignInResult
{
    public SignInResult()
    {
    }

    public SignInResult(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }

    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } =string.Empty;
}