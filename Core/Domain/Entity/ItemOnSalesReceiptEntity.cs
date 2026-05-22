using Core.Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entity;
[NotMapped]
public class ItemOnSalesReceiptEntity: AuditableEntity
{
    public int SalesReceiptId { get; set; } // Reference to the associated receipt
    public Guid ProductId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
    public decimal Discount { get; set; }

    public decimal TotalPrice { get; set; } // Calculate total price for the item

    public SalesReceiptEntity SalesReceipt { get; set; } // Navigation property to the parent receipt
}
