using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Rent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }

        public bool Deleted { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        [Required]
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        [Required]
        public int ReturnBranchId { get; set; }
        [ForeignKey("ReturnBranchId")]
        public virtual Branch ReturnBranch { get; set; }
    }
}