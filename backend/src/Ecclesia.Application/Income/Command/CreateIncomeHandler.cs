using Ecclesia.Application.Incomes.DTOs;
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

    public async Task<Result<Guid>> HandleAsync(CreateIncomeDto createDto, CancellationToken cancellationToken = default)
    {
        // Validación
        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Guid>.Failure(errors);
        }

        // Verificar periodo contable abierto
        var period = await _accountingPeriodRepository.GetOpenPeriodAsync(createDto.CommunityId, cancellationToken);
        if (period is null)
            return Result<Guid>.Failure("No existe un período contable abierto para esta comunidad.");

        // Generar número de voucher
        var voucherNumber = await _journalVoucherRepository.GenerateVoucherNumberAsync(cancellationToken);

        // Obtener Rostro desde la comunidad (asociación obligatoria en el modelo)
        var rostroId = await _communityRepository.GetRostroIdAsync(createDto.CommunityId, cancellationToken);
        if (rostroId == Guid.Empty)
            return Result<Guid>.Failure("La comunidad no tiene un rostro asociado, lo cual es obligatorio para crear un ingreso.");

        // Crear JournalVoucher automáticamente
        var journalVoucher = new JournalVoucherEntity(
            voucherNumber,
            VoucherType.Income,
            createDto.Date,
            createDto.Description,
            period.Id,
            rostroId,
            createDto.CommunityId
        );

        await _journalVoucherRepository.AddAsync(journalVoucher, cancellationToken);

        // Crear Income
        var income = new IncomeEntity(
            createDto.Date,
            createDto.Amount,
            createDto.CashAccountId,
            createDto.CommunityId,
            journalVoucher.Id,
            createDto.DonorId
        );

        await _incomeRepository.AddAsync(income, cancellationToken);

        return Result<Guid>.Success(income.Id);
    }
}