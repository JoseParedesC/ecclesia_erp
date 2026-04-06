using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Queries.GetRostroById;

public sealed class GetRostroByIdHandler(IRostroRepository repository)
{
    public async Task<Result<RostroDetailDto>> HandleAsync(
        GetRostroByIdQuery query,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(query.Id, ct);
        if (rostro is null)
            return Result<RostroDetailDto>.Failure("Rostro no encontrado.");

        var dto = new RostroDetailDto(
            rostro.Id,
            rostro.Code,
            rostro.Name,
            rostro.Description,
            rostro.IsActive,
            rostro.CreatedAt,
            rostro.UpdatedAt);

        return Result<RostroDetailDto>.Success(dto);
    }
}
