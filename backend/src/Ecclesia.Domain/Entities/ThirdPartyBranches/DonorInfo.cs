
using Ecclesia.Domain.Common.Enums;

namespace Ecclesia.Domain.Entities.ThirdPartyBranches;

// Donante
public class DonorInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public DateTime FirstDonationDate { get; private set; }
    public DateTime? LastDonationDate { get; private set; }
    public decimal TotalDonated { get; private set; }
    public bool IsRecurrent { get; private set; }

    protected DonorInfo() { }

    public DonorInfo(Guid thirdPartyId, bool isRecurrent)
    {
        ThirdPartyId = thirdPartyId;
        FirstDonationDate = DateTime.UtcNow;
        IsRecurrent = isRecurrent;
        TotalDonated = 0;
    }
}