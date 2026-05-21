using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Procedures
{
    public class GetSalesSummaryDto
    {
        public string SellerCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int NumberOfSales { get; set; }
        public decimal SalesTotalAmount { get; set; }
    }
}
