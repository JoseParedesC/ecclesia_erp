

public record AccountingPeriodDetailDto(
    int Year,
    int Month,
    StatusDocument Status,
    DateTime? ClosedAt,
    Guid CommunityId
);