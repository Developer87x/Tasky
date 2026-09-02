namespace Tasky.BuildingBlocks.Core.Exceptions;

public class DomainException(string message) : Exception(message)
{
    
}

public class NotFoundException(string message) : Exception(message)
{
    
}
public class UnexpectedException(string message) : Exception(message)
{

}

public class BadRequestException(string message) : Exception(message)
{

}

public class ConflictException(string message) : Exception(message)
{

}

public class ValidationException(string message) : Exception(message)
{

}

public class UnauthorizedException(string message) : Exception(message)
{

}

public class ForbiddenException(string message) : Exception(message)
{

}

public class UnprocessableEntityException(string message) : Exception(message)
{

}

public class InternalServerErrorException(string message) : Exception(message)
{

}

