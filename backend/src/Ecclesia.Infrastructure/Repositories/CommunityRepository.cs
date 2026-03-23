using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class CommunityRepository : ICommunityRepository
{
    private readonly AppDbContext _context;

    public async Task<Guid> GetRostroIdAsync(Guid communityId, CancellationToken ct)
    {
        var community = await _context.Communities
            .Where(x => x.Id == communityId)
            .Select(x => new { x.RostroId })
            .FirstOrDefaultAsync(ct);

        if (community == null)
            throw new Exception("Community not found");

        return community.RostroId;
    }
}