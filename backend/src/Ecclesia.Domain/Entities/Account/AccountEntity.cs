namespace Ecclesia.Domain.Entities.Account;

public class AccountEntity : BaseEntity
{
    public string? Code { get; private set; }
    public string? Name { get; private set; }
    public AccountType Type { get; private set; }
    public Guid? ParentAccountId { get; private set; }
    public AccountEntity? ParentAccount { get; set; }
    public ICollection<AccountEntity> ChildAccounts { get; set; } = new List<AccountEntity>();

    protected AccountEntity() { }

    public AccountEntity(string? code, string? name, AccountType type, Guid? parentAccountId = null)
    {
        Code            = code;
        Name            = name;
        Type            = type;
        ParentAccountId = parentAccountId;
    }

    public void Update(string? code, string? name, AccountType type, Guid? parentAccountId)
    {
        Code            = code            ?? Code;
        Name            = name            ?? Name;
        Type            = type;
        ParentAccountId = parentAccountId ?? ParentAccountId;
    }
}