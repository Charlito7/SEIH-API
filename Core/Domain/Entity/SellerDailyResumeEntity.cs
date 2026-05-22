using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entity
{
    [Table("sellers_daily_resume")]
    public class SellerDailyResumeEntity
    {
        public Guid? SellerId { get; set; }
        public decimal AmountIN { get; set; }
        public decimal AmountOUT { get; set; }
        public bool? IsOpened { get; set; }
        public bool? IsClosed { get; set; }

        public DateOnly Date { get; set; }
        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
