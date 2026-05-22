using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request.Product;

public class ProductRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]

    public string Name { get; set; }

    public string Description { get; set; }

    //[RegularExpression("^[0-9]*$", ErrorMessage = "BarCode can only contain numbers")]
    public string BarCode { get; set; }

    public string CategoryName { get; set; }

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
    [Range(0, int.MaxValue, ErrorMessage = "Minimum Reorder Quantity must be a positive number")]
    public int MinimumReorderQuantity { get; set; }
    public bool IsReturnAccepted { get; set; }
    public int ReturnTimeAccepted { get; set; }

}
