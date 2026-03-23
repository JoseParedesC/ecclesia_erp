using Ecclesia.Domain.Entities.Income;

namespace Ecclesia.Application.Incomes.DTOs;

public static class IncomeMapper
{
    public static IncomeDto ToDto(this IncomeEntity entity) => new(
        entity.Id,
        entity.Date,
        entity.Amount,
        entity.DonorId,
        entity.Donor.Name,
        entity.CashAccountId,
        entity.CashAccount.Name,
        entity.CommunityId,
        entity.JournalVoucherId,
        entity.JournalVoucher.VoucherNumber,
        entity.CreatedAt
    );
}