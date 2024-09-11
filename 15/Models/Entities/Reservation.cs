using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Reservations", Schema = "dbo")]
    public class Reservation
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int AppUserId { get; set; }
        public int VehicleId { get; set; }

        [ForeignKey("FirstBranchOffice")]
        public int FirstBranchOfficeId { get; set; }
        public BranchOffice FirstBranchOffice { get; set; }
        [ForeignKey("SecundBranchOffice")]
        public int SecundBranchOfficeId { get; set; }
        public BranchOffice SecundBranchOffice { get; set; }


        [Column("DateRezervation", TypeName = "datetime2")]
        public DateTime DateRezervation { get; set; }

        [Column("ReturnDate", TypeName = "datetime2")]
        public DateTime ReturnDate { get; set; }
    }
}