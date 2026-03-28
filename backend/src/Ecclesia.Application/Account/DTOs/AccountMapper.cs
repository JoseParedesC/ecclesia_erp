using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.DTOs;

public static class AccountMapper
{
    public static AccountListDto ToListDto(this AccountEntity entity) => new(
        entity.Id,
        entity.Code,
        entity.Name,
        entity.Type,
        entity.ParentAccountId,
        entity.ParentAccount?.Name,
        entity.CreatedAt
    );

    public static AccountDetailDto ToDto(this AccountEntity entity) => new(
        entity.Id,
        entity.Code,
        entity.Name,
        entity.Type,
        entity.ParentAccountId,
        entity.ParentAccount?.Name,
        entity.ChildAccounts.Select(x => x.ToChildDto()),
        entity.CreatedAt,
        entity.UpdatedAt
    );

    public static AccountChildDto ToChildDto(this AccountEntity entity) => new(
        entity.Id,
        entity.Code,
        entity.Name,
        entity.Type
    );

    public static AccountSearchDto ToSearchDto(this AccountEntity entity) => new(
        entity.Id,
        entity.Code,
        entity.Name,
        entity.Type
    );
}