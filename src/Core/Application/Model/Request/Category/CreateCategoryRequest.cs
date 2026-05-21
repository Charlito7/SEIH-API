using System.ComponentModel.DataAnnotations;

namespace Core.Application.Model.Request;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "CategoryName is required")]

    public string Name { get; set; }


    public string Description { get; set; }

}
