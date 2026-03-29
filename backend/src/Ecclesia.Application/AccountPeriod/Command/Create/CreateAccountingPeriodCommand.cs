using Ecclesia.Domain.Common.Enums;

namespace Ecclesia.Application.AccountingPeriod.Commands.CreateAccountingPeriod;

public record CreateAccountingPeriodCommand(
    DateTime Period
);