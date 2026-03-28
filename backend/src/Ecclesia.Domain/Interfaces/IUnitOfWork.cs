
using Ecclesia.Domain.Entities.Income;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}