

namespace Ecclesia.Domain.Entities.Account;

public class AccountEntity : BaseEntity
{
    public string? Code { get; private set; }
    public string? Name { get; private set; }
    public AccountType Type { get; private set; }
    public Guid? ParentAccountId { get; private set; }
    public AccountEntity? ParentAccount { get; set; }
    public ICollection<AccountEntity> ChildAccounts { get; set; } = new List<AccountEntity>();

    private AccountEntity() { }

    public AccountEntity(AccountEntity account)
    {
        Code = account.Code;
        Name = account.Name;
        Type = account.Type;
        ParentAccountId = account.ParentAccountId;
    }
}