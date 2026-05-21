namespace Core.Application.Model.Features;

public class TransferReceiveResponseDto
{
    public Guid TransferId { get; set; }
    public string Status { get; set; } = "RECEIVED";
}


