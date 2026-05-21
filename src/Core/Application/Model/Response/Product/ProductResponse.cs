using System.ComponentModel.DataAnnotations;

namespace Core.Application.Model.Response.Product;

public class ProductResponse
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required")]

    public string Name { get; set; }

    public string Description { get; set; }

    public string? BarCode { get; set; }

    public string? CategoryName { get; set; }
    public DateTime ExpiredDate { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public int MinimumReorderQuantity { get; set; }
    public bool IsReturnAccepted { get; set; }
    public int ReturnTimeAccepted { get; set; }
}
