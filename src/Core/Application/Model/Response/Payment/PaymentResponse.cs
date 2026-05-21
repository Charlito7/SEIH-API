using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Model.Response.Payment
{
    public class PaymentResponse
    {
        public string? paymentId {  get; set; }
        public string? paymentClientSecret { get; set; }
    }
}
