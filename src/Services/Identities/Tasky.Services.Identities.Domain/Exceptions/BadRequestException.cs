namespace Tasky.Services.Identities.Domain.Exceptions;

public class BadRequestException(string message) : DomainException(message)
{
}