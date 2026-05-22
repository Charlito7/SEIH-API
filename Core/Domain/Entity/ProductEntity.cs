using Core.Domain.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity;
[NotMapped]
public class ProductEntity : AuditableEntity
{
    [Required]

    public string Name { get; set; }
    public string Description { get; set; }

    [RegularExpression(@"^\d*$", ErrorMessage = "BarCode can only contain digits")]

    public string BarCode { get; set; }

    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "ExpiredDate is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpiredDate { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Minimum Reorder Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Minimum Reorder Quantity must be a positive number")]
    public int MinimumReorderQuantity { get; set; }

    public bool IsReturnAccepted { get; set; }
    public int  ReturnTimeAccepted { get; set; }
    // Navigation property for the one-to-many relationship

    public CategoryEntity Category { get; set; }

}
