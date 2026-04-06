using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public sealed class UpdateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        UpdateRostroCommand command,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(command.Id, ct);
        if (rostro is null)
            return Result<Guid>.Failure("Rostro no encontrado.");

        if (!rostro.IsActive)
            return Result<Guid>.Failure("No se puede modificar un Rostro inactivo.");

        if (await repository.ExistsByNameAsync(command.Name.Trim(), excludeId: command.Id, ct: ct))
            return Result<Guid>.Failure("Ya existe otro Rostro con ese nombre.");

        rostro.Update(command.Name, command.Description);
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
