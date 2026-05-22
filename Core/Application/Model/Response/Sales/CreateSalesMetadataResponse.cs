using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Sales
{
    public class CreateSalesMetadataResponse
    {
        public decimal Discount { get; set; }
        public string Notes { get; set; }
        [Required]
        public decimal CashReceived { get; set; }
        public string PaymentType { get; set; }
        public string PaymentCustomerName { get; set; }
        public string PaymentTypeTransactionID { get; set; }
    }
}
