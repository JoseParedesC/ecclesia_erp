

namespace Ecclesia.Domain.Entities.ThirdPartyBranches;

// Empleado
public class EmployeeInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public string? Position { get; private set; }
    public string? Department { get; private set; }
    public DateTime HireDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public decimal Salary { get; private set; }
    public string? BankAccount { get; private set; }

    protected EmployeeInfo() { }

    public EmployeeInfo(Guid thirdPartyId, string? position, string? department, decimal salary, string? bankAccount)
    {
        ThirdPartyId = thirdPartyId;
        Position = position;
        Department = department;
        Salary = salary;
        BankAccount = bankAccount;
        HireDate = DateTime.UtcNow;
    }
}