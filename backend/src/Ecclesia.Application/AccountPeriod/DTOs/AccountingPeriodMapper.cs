using Ecclesia.Domain.Entities.AccountingPeriod;

namespace Ecclesia.Application.AccountingPeriods.DTOs;

public static class AccountingPeriodMapper
{
    public static AccountingPeriodListDto ToListDto(this AccountingPeriodEntity entity) => new(
        entity.Id,
        entity.Year,
        entity.Month,
        entity.Status,
        entity.ClosedAt,
        entity.CreatedAt
    );

    public static AccountingPeriodDetailDto ToDto(this AccountingPeriodEntity entity) => new(
        entity.Id,
        entity.Year,
        entity.Month,
        entity.Status,
        entity.ClosedAt,
        entity.ClosedBy,
        entity.ReopenedAt,
        entity.ReopenedBy,
        entity.CreatedAt,
        entity.UpdatedAt
    );
}