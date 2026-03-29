using Ecclesia.Domain.Common.Constants;

namespace Ecclesia.Application.AccountingPeriods.DTOs;

public record AccountingPeriodListDto(
    Guid           Id,
    int            Year,
    int            Month,
    StatusDocument Status,
    DateTime?      ClosedAt,
    DateTime       CreatedAt
);