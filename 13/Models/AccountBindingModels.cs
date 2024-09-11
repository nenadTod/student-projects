using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

    public class FinishBindingModel
    {
        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }

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

    public class VehicleBindingModel
    {
        [Required]
        [Display(Name = "Deleted")]
        public bool Deleted { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model{ get; set; }

        [Required]
        [Display(Name = "Manufactor")]
        public string Manufactor { get; set; }

        [Required]
        [Display(Name = "Year")]
        public double Year { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "PricePerHour")]
        public double PricePerHour { get; set; }

        [Required]
        [Display(Name = "Unvailable")]
        public bool Unvailable { get; set; }

        [Required]
        [Display(Name = "Images")]
        public string Images { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }
    }

    public class BranchBindingModel
    {
        [Required]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }


    }

    public class RateBindingModel
    {
        
        [Required]
        [Display(Name = "Rating")]
        public double Rating { get; set; }
    }

    public class RentBindingModel
    {

        [Required]
        [Display(Name = "GetBranchId")]
        public int  GetBranchId { get; set; }

        [Required]
        [Display(Name = "RetBranchId")]
        public int RetBranchId { get; set; }

        [Required]
        [Display(Name = "VehicleId")]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Start")]
        public string Start { get; set; }

        [Required]
        [Display(Name = "End")]
        public string End { get; set; }
    }

    public class EmailBindingModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class CommentBindingModel
    {
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }
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
