namespace Ecclesia.Application.Incomes.DTOs;

public record IncomeDto(
    Guid Id,
    DateTime Date,
    decimal Amount,
    Guid? ThirdPartyId,
    string? ThirdPartyName,
    Guid? CashAccountId,
    string? CashAccountName,
    Guid? CommunityId,
    Guid? JournalVoucherId,
    string? VoucherNumber,
    DateTime CreatedAt
);