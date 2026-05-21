using Core.Application.Model.Request.Product;
using Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Product;

public class GetProductPaginationResponse
{
    public IEnumerable<ProductRequest> Products { get; set; }
    public PaginationInfo Pagination { get; set; }
}
