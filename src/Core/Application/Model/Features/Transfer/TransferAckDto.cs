namespace Core.Application.Model.Features;

public class TransferAckDto
{
    public Guid TransferId { get; set; }
    public Guid HospitalId { get; set; }
    public DateTime ReceivedAt { get; set; }
}
