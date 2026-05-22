using Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Procedures;

public class GetAllSalesMetadata
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string SellerFullName { get; set; }
    public string? PaymentCustomerName { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentType { get; set; }

}
