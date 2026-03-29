using Ecclesia.Domain.Common.Constants;

namespace Ecclesia.Application.AccountingPeriods.DTOs;

public record AccountingPeriodDetailDto(
    Guid           Id,
    int            Year,
    int            Month,
    StatusDocument Status,
    DateTime?      ClosedAt,
    Guid?          ClosedBy,
    DateTime?      ReopenedAt,
    Guid?          ReopenedBy,
    DateTime       CreatedAt,
    DateTime       UpdatedAt
);