using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.DeactivateRostro;

public sealed class DeactivateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        DeactivateRostroCommand command,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(command.Id, ct);
        if (rostro is null)
            return Result<Guid>.Failure("Rostro no encontrado.");

        if (!rostro.IsActive)
            return Result<Guid>.Failure("El Rostro ya está inactivo.");

        rostro.Deactivate();
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
