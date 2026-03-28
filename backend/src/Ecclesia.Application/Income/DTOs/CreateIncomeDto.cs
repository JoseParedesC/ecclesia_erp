namespace Ecclesia.Application.Incomes.DTOs;

public record CreateIncomeDto(
    DateTime Date,
    decimal Amount,
    Guid DonorId,
    Guid CashAccountId,
    Guid CommunityId,
    string Description
);