using Ecclesia.Domain.Entities.AccountingPeriod;

public static class AccountingPeriodMapper
{
    public static AccountingPeriodDetailDto ToDetailDto(this AccountingPeriodEntity entity)  => new (
        entity.Year,
        entity.Month,
        entity.Status,
        entity.ClosedAt,
        entity.CommunityId
    );
    
}