
using MediatR;

public record CreateIncomeCommand(
    DateTime Date,
    decimal Amount,
    Guid CashAccountId,
    Guid CommunityId,
    Guid? DonorId
) : IRequest<Guid>;