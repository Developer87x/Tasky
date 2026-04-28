namespace Tasky.Services.Identities.Domain.Exceptions;

public class UnauthorizedException(string message) : DomainException(message)
{
}