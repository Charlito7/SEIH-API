using Core.Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entity;

[NotMapped]
public class SalesReceiptEntity: AuditableEntity
{
    public DateTime Date { get; set; } // Track when the receipt was generated

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhoneNumber { get; set; }

    public decimal TotalAmount { get; set; } // Calculate total based on items
    public decimal CashReceived { get; set; } // Amount paid by customer
    public decimal ChangeDue { get; set; } // Calculate change to be returned
    public decimal RefundAmount { get; set; } // If any refund is applied

    public ICollection<ItemOnSalesReceiptEntity> Items { get; set; } // Collection of items
}
