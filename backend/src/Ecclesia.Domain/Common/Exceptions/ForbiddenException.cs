namespace Ecclesia.Domain.Common.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Acceso prohibido.") : base(message) { }
}