using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Sales
{
    public class GetSellerDailySalesResumeResponse
    {
        public decimal AmountIN { get; set; }
        public decimal TotalPriceSales { get; set; }
        public decimal Total { get; set; }
        public decimal TotalQuantitySales { get; set; }
    }
}
