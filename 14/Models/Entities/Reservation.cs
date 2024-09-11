using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Service Service { get; set; }
        public DateTime? Start { get; set; }
        public BranchOffice StartBranchOffice { get; set; }
        public DateTime? End { get; set; }
        public BranchOffice EndBranchOffice { get; set; }
        public Vehicle Vehicle { get; set; }

  


    }
}