namespace Core.Application.Model.Features;

public class TransferRequestCreateDto
{
    public Guid IdHospitalTo { get; set; }
    public string InfoPatient { get; set; } = string.Empty;
    public Guid IdConsent { get; set; }
}
