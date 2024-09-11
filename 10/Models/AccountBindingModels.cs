using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using RentApp.Models.Entities;

namespace RentApp.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "BirthDay")]
        public DateTime? BirthDay { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RentBindingModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start")]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End")]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "BranchStart")]
        public string BranchStart { get; set; }

        [Display(Name = "Vehicle")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class BranchBindingModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }
    }

    public class VehicleBindingModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Manufactor")]
        public string Manufactor { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "PricePerHour")]
        public decimal PricePerHour { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Unavailable")]
        public bool Unavailable { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Images")]
        public List<string> Images { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "VehicleImagesBase")]
        public string VehicleImagesBase { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Service")]
        public string Service { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
