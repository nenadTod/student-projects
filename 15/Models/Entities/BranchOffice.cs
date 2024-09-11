using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("BranchOffices", Schema = "dbo")]
    public class BranchOffice
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Picture { get; set; }
        public string Addres { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

    }
}