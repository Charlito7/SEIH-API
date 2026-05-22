namespace Core.Application.Model.Features;

public class PrepareDto
{
    public string SenderHospitalId { get; set; } = string.Empty;
    public string RecipientHospitalId { get; set; } = string.Empty;
    public long Size { get; set; }
    public string ContentType { get; set; } = string.Empty;
}
