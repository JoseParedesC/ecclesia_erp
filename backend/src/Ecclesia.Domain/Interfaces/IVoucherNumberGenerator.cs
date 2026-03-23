
public interface IVoucherNumberGenerator
{
    Task<string> GenerateAsync(VoucherType type, DateTime date, CancellationToken ct);
}