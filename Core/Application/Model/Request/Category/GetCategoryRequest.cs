namespace Core.Application.Model.Request;

public class GetCategoryRequest
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; }


    public string Description { get; set; }

}
