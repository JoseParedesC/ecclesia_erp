using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Users.Queries;

public class GetAllUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<UserEntity>>> HandleAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        // Consulta
        var users = await _userRepository.ListAllAsync(cancellationToken);
        return Result<IEnumerable<UserEntity>>.Success(users);
    }
}
