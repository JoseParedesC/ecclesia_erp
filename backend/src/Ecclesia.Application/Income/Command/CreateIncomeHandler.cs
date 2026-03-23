using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Accounting;
using Ecclesia.Domain.Entities.Income;
using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Interfaces;

namespace Ecclesia.Application.Incomes.Commands.CreateIncome;

public class CreateIncomeHandler
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IJournalVoucherRepository _journalVoucherRepository;
    private readonly IAccountingPeriodRepository _accountingPeriodRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly CreateIncomeValidator _validator;

    public CreateIncomeHandler(
        IIncomeRepository incomeRepository,
        IJournalVoucherRepository journalVoucherRepository,
        IAccountingPeriodRepository accountingPeriodRepository,
        ICommunityRepository communityRepository,
        CreateIncomeValidator validator)
    {
        _incomeRepository = incomeRepository;
        _journalVoucherRepository = journalVoucherRepository;
        _accountingPeriodRepository = accountingPeriodRepository;
        _communityRepository = communityRepository;
        _validator = validator;
    }

    public async Task<Result<Guid>> HandleAsync(CreateIncomeCommand command, CancellationToken cancellationToken = default)
    {
        // Validación
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Guid>.Failure(errors);
        }

        // Verificar periodo contable abierto
        var period = await _accountingPeriodRepository.GetOpenPeriodAsync(command.Dto.CommunityId, cancellationToken);
        if (period is null)
            return Result<Guid>.Failure("No existe un período contable abierto para esta comunidad.");

        // Generar número de voucher
        var voucherNumber = await _journalVoucherRepository.GenerateVoucherNumberAsync(cancellationToken);

        // Obtener Rostro desde la comunidad (asociación obligatoria en el modelo)
        var rostroId = await _communityRepository.GetRostroIdAsync(command.Dto.CommunityId, cancellationToken);

        // Crear JournalVoucher automáticamente
        var journalVoucher = new JournalVoucherEntity(
            voucherNumber,
            VoucherType.Income,
            command.Dto.Date,
            command.Dto.Description,
            period.Id,
            rostroId,
            command.Dto.CommunityId
        );

        await _journalVoucherRepository.AddAsync(journalVoucher, cancellationToken);

        // Crear Income
        var income = new IncomeEntity(
            command.Dto.Date,
            command.Dto.Amount,
            command.Dto.CashAccountId,
            command.Dto.CommunityId,
            journalVoucher.Id,
            command.Dto.DonorId
        );

        await _incomeRepository.AddAsync(income, cancellationToken);

        return Result<Guid>.Success(income.Id);
    }
}