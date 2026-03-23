namespace Ecclesia.Domain.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, Guid id) : base($"{entity} con Id '{id}' no encontrado.") { }
}