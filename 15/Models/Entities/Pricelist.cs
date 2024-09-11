using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Pricelists", Schema = "dbo")]
    public class PriceList
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }

        [Column("FromDate", TypeName = "datetime2")]
        public DateTime FromDate { get; set; }
        [Column("ToDate", TypeName = "datetime2")]
        public DateTime ToDate { get; set; }

    }
}