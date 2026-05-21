using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features.Hospital

{
    public class RotateKeyDto
    {
        public Guid HospitalId { get; set; }
        public string PublicKey { get; set; } = default!;
        public int KeyVersion { get; set; }
    }
}
