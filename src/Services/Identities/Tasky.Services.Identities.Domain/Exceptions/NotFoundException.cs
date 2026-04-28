namespace Tasky.Services.Identities.Domain.Exceptions;

public class NotFoundException(string message) : DomainException(message)
{
}