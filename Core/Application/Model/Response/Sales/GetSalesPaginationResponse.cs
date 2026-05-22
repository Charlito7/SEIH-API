using Core.Application.Model.Request.Product;
using Core.Application.Model.Response.Product;
using Core.Domain.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Sales;

public class GetSalesPaginationResponse
{
    public IEnumerable<GetAllSalesMetadata> Sales { get; set; }
    public PaginationInfo Pagination { get; set; }
}
