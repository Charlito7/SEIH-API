using Core.Domain.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity;

[NotMapped]
public class CategoryEntity : AuditableEntity
{
    [Required(ErrorMessage = "CategoryName is required")]

    public string Name { get; set; }


    public string Description { get; set; }



    public ICollection<ProductEntity> Products { get; set; }

}
