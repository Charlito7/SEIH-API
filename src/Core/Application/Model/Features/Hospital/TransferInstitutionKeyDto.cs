using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features.Hospital
{
    public class TransferInstitutionKeyDto
{
    public string PublicKey { get; set; } = string.Empty;

    public int KeyVersion { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool IsActive { get; set; }
}
}
