
using MediatR;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Entities.Income;


public class CreateIncomeHandler : IRequestHandler<CreateIncomeCommand, Guid>
{
    private readonly IVoucherNumberGenerator _voucherNumberGenerator;
    private readonly IJournalVoucherRepository _voucherRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IAccountingPeriodService _accountingPeriodService;
    private readonly ICommunityRepository _communityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateIncomeHandler(
        IJournalVoucherRepository voucherRepository,
        IIncomeRepository incomeRepository,
        IAccountingPeriodService accountingPeriodService,
        ICommunityRepository communityRepository,
        IUnitOfWork unitOfWork,
        IVoucherNumberGenerator voucherNumberGenerator)
    {
        _voucherRepository = voucherRepository;
        _incomeRepository = incomeRepository;
        _unitOfWork = unitOfWork;
        _voucherNumberGenerator = voucherNumberGenerator;
        _accountingPeriodService = accountingPeriodService;
        _communityRepository = communityRepository;
    }

    public async Task<Guid> Handle(CreateIncomeCommand request, CancellationToken ct)
    {
        // 1. Crear voucher
        var voucher = new JournalVoucherEntity(
            voucherNumber: await _voucherNumberGenerator.GenerateAsync(VoucherType.Income, request.Date, ct),
            type: VoucherType.Income,
            date: request.Date,
            description: "Income",
            accountingPeriodId: await _accountingPeriodService.GetOpenPeriodIdAsync(request.Date, ct),
            rostroId: await _communityRepository.GetRostroIdAsync(request.CommunityId, ct),
            communityId: request.CommunityId
        );

        
        Guid accountCash = Guid.Empty;
        Guid accountIncome = Guid.Empty;

        // 2. Partida doble
        voucher.AddLine(accountCash, request.Amount, LineType.Debit);
        voucher.AddLine(accountIncome, request.Amount, LineType.Credit);

        voucher.Post();

        // 3. Crear Income
        var income = new IncomeEntity(
            request.Date,
            request.Amount,
            request.CashAccountId,
            request.CommunityId,
            voucher.Id,
            request.DonorId
        );

        // 4. Persistencia
        await _voucherRepository.AddAsync(voucher, ct);
        await _incomeRepository.AddAsync(income, ct);

        // 5. Commit
        await _unitOfWork.SaveChangesAsync(ct);

        return income.Id;
    }
}