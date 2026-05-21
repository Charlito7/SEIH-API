namespace Core.Application.Model.Features;

public class TransferRequestDto
{
    public Guid Id { get; set; }
    public Guid IdHospitalFrom { get; set; }
    public Guid IdHospitalTo { get; set; }
    public string InfoPatient { get; set; } = string.Empty;
    public Guid IdConsent { get; set; }
    public string Status { get; set; } = string.Empty;
}
