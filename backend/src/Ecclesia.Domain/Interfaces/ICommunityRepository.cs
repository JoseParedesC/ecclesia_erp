public interface ICommunityRepository
{
    Task<Guid> GetRostroIdAsync(Guid communityId, CancellationToken ct);
}