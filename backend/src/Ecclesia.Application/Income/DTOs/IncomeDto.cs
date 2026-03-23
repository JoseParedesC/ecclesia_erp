namespace Ecclesia.Application.Incomes.DTOs;

public record IncomeDto(
    Guid Id,
    DateTime Date,
    decimal Amount,
    Guid? DonorId,
    string DonorName,
    Guid? CashAccountId,
    string CashAccountName,
    Guid? CommunityId,
    Guid? JournalVoucherId,
    string VoucherNumber,
    DateTime CreatedAt
);