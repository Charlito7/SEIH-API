using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Procedures;

public class SalesMetadataAndProductResponse
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PaymentType { get; set; }
    public string? PaymentCustomerName { get; set; }
    public string? PaymentTypeTransactionId { get; set; }
    public string? SellerFullName { get; set; }
    public string? ProductSalesDetails { get; set; }
    public decimal Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public string? ShippingAddress { get; set; }
    public decimal OrderTaxPercentage { get; set; }

}

public class SalesMetadataAndProductResponseDTO
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PaymentType { get; set; }
    public string? PaymentCustomerName { get; set; }
    public string? PaymentTypeTransactionId { get; set; }
    public string? SellerFullName { get; set; }
    public decimal Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public string? ShippingAddress { get; set; }
    public decimal OrderTaxPercentage { get; set; }
    public ProductSaleDetail[]? ProductSales { get; set; }
}
public class ProductSaleDetail
{
    public string? ProductCode { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string? ProductName { get; set; }
}