using Core.Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entity;

[NotMapped]
public class ProductSalesEntity : AuditableEntity
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public decimal UnitCost { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }

    // Other possible fields
    public decimal Subtotal => UnitCost * Quantity; // Calculated field for convenience

    // Foreign key relationship
    public Guid? SalesMetadataId { get; set; }
    public SalesMetadataEntity SalesMetadata { get; set; }
}
