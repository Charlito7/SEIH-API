namespace Core.Application.Model.Features;

public class GlobalTransferDto
{
    public Guid Id { get; set; }
    public Guid IdHospitalFrom { get; set; }
    public Guid IdHospitalTo { get; set; }
    public string Status { get; set; } = "";
    public DateTime Created { get; set; }
    public string? Message { get; set; }
    public string? PatientReference { get; set; }
}
