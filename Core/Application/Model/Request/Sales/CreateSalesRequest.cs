using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Request.Sales;

public class CreateSalesRequest
{
    public CreateSalesMetadataRequest SalesMetadata { get; set; }
    public List<CreateProductSalesRequest> ProductSales { get; set; }
}
