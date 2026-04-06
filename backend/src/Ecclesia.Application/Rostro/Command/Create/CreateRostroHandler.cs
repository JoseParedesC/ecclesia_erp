using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public sealed class CreateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        CreateRostroCommand command,
        CancellationToken ct = default)
    {
        var codeNormalized = command.Code.Trim().ToUpperInvariant();

        if (await repository.ExistsByCodeAsync(codeNormalized, ct: ct))
            return Result<Guid>.Failure("El código ya está en uso.");

        if (await repository.ExistsByNameAsync(command.Name.Trim(), ct: ct))
            return Result<Guid>.Failure("Ya existe un Rostro con ese nombre.");

        var rostro = RostroEntity.Create(command.Code, command.Name, command.Description);

        await repository.AddAsync(rostro, ct);
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
