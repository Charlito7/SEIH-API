using Core.Domain.Entity.SEIH;

namespace Core.Application.Model.Features.Hospital
{
    public class KeyInfoDto
    {
        public int KeyVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
