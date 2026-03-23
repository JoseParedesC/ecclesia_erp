
using Ecclesia.Domain.Entities.Income;

namespace Ecclesia.Domain.Interfaces;

public interface IIncomeRepository
{
    Task AddAsync(IncomeEntity income, CancellationToken ct);
}