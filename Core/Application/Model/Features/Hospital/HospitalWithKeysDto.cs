using Core.Application.Model.Features.Hospital;

namespace Core.Application.Model.Features;

public class HospitalWithKeysDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public string City { get; set; } = "";
    public string Department { get; set; } = "";
    public bool IsActive { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<TransferInstitutionKeyDto> Keys { get; set; } = new();
}

