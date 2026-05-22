using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features.Hospital
{
    public class PublicHospitalDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public string? PublicKey { get; set; }
        public HospitalIntegrationType IntegrationType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
