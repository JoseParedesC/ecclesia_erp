
using Ecclesia.Domain.Common.Enums;


namespace Ecclesia.Domain.Entities.ThirdPartyBranches;

// Partner
public class PartnerInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public string? Organization { get; private set; }
    public DateTime PartnerSince { get; private set; }
    public PartnerType PartnerType { get; private set; }
    public string? AgreementCode { get; private set; }

    protected PartnerInfo() { }

    public PartnerInfo(Guid thirdPartyId, string? organization, PartnerType partnerType, string? agreementCode)
    {
        ThirdPartyId = thirdPartyId;
        Organization = organization;
        PartnerType = partnerType;
        AgreementCode = agreementCode;
        PartnerSince = DateTime.UtcNow;
    }
}