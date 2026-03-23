using Ecclesia.Domain.Entities.Users;

namespace Ecclesia.Domain.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByIdNoTrackAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserEntity user, CancellationToken cancellationToken = default);
}