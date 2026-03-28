
using Ecclesia.Domain.Common.Enums;

namespace Ecclesia.Domain.Entities.ThirdPartyBranches;

// Miembro
public class MemberInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public string? MemberCode { get; private set; }
    public DateTime MemberSince { get; private set; }
    public string? Ministry { get; private set; }
    public MemberStatus Status { get; private set; }

    protected MemberInfo() { }

    public MemberInfo(Guid thirdPartyId, string? memberCode, string? ministry)
    {
        ThirdPartyId = thirdPartyId;
        MemberCode = memberCode;
        Ministry = ministry;
        MemberSince = DateTime.UtcNow;
        Status = MemberStatus.Active;
    }
}