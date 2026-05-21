using Core.Domain.Entity.SEIH;

public class CreateHospitalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PublicKey { get; set; } = default!;
    public HospitalIntegrationType IntegrationType { get; set; }
}

public class CreateHospitalResponse
{
    public Guid HospitalId { get; set; }
    public string ApiKey { get; set; } = default!;
    public bool IsActive { get; set; }

    public string PublicKeyFingerprint { get; set; } = default!;
    public DateTime PublicKeyCreateDate { get; set; }
    public DateTime PublicKeyExpireDate { get; set; }
}