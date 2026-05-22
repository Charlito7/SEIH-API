using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request.Sales;

public class CreateProductSalesRequest
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public decimal UnitCost { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal Subtotal => UnitCost * Quantity;
}
