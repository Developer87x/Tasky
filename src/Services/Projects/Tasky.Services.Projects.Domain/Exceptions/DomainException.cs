using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasky.Services.Projects.Domain.Exceptions;

public class DomainException(string message) : Exception(message)
{
    
}

public class NotFoundException(string message) : DomainException(message)
{
}
public class UnauthorizedException(string message) : DomainException(message)
{
}
public class BadRequestException(string message) : DomainException(message)
{
}