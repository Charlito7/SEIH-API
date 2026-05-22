using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Product
{
    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }
    }
}
