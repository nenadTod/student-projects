using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentApp.Models.Entities
{
    public class BranchOffice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Adress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}