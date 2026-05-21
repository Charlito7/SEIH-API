namespace Core.Application.Model;

public class TaskRequestModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
