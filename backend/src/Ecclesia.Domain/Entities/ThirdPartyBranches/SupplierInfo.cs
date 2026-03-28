
namespace Ecclesia.Domain.Entities.ThirdPartyBranches;


//Proveedores
public class SupplierInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public string? BankAccount { get; private set; }
    public string? BankName { get; private set; }
    public int PaymentTermDays { get; private set; }
    public string? TaxRegime { get; private set; }

    protected SupplierInfo() { }

    public SupplierInfo(Guid thirdPartyId, string? bankAccount, string? bankName, int paymentTermDays, string? taxRegime)
    {
        ThirdPartyId = thirdPartyId;
        BankAccount = bankAccount;
        BankName = bankName;
        PaymentTermDays = paymentTermDays;
        TaxRegime = taxRegime;
    }
}