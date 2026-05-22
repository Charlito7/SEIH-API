using Core.Domain.Commons;


namespace Core.Domain.Entity.SEIH
{
    public class UsedNonceEntity
    {
   public Guid Id { get; set; }
    public Guid HospitalId { get; set; }
    public string Value { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    }
}
