using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;
using Ecclesia.Domain.Common.PagedQuery;

namespace Ecclesia.Application.Users.Queries.GetAllUsers;

public class GetAllUsersHandler
{
    private readonly IUserRepository _userRepository;
    private readonly GetAllUsersValidator _validator;

    public GetAllUsersHandler(IUserRepository userRepository, GetAllUsersValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<PagedResult<UserEntity>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<UserEntity>>.Failure(errors);
        }

        var result = await _userRepository.ListAllAsync(query, cancellationToken);

        return Result<PagedResult<UserEntity>>.Success(result);
    }
}