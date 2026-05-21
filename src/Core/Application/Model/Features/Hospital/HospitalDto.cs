namespace Core.Application.Model.Features;

public class HospitalDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public bool IsActive { get; set; }
    public string? PublicKey { get; set; }
    public DateTime? PublicKeyCreateDate { get; set; }
    public DateTime? PublicKeyExpireDate { get; set; }
    public string? PublicKeyFingerprint { get; set; }
}
