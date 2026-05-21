using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Procedures
{
    public class GetSellerSalesTotalPriceAndQuantityToday
    {
        public decimal TotalPriceSales { get; set; }
        public decimal TotalQuantitySales { get; set; }
    }
}
