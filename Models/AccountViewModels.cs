using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExwhyzeeEDI.Web.Models
{

    //phonenumber verify
    public class UniquePhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userWithTheSamePhone = db.Users.FirstOrDefault(
                u => u.PhoneNumber == (string)value);
            return userWithTheSamePhone == null;
        }
    }


    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class NewRegisterViewModel
    {
        //[Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public string Phone { get; set; }

    }

    public class RegisterViewModel
    {

        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

        [Display(Name = "BVN")]
        public string BVN { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [UniquePhone(ErrorMessage = "This Phone Number is alread taken.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public string VerifyCode { get; set; }

        public bool ComfirmVerifyCode { get; set; }

        public int? SchoolId { get; set; }

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


    public class JDPCRegisterViewModel
    {

        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

        [Display(Name = "BVN")]
        public string BVN { get; set; }

        public string Gender { get; set; }
        [Display(Name = "Date of birth")]
        public DateTime Dateofbirth { get; set; }

        [Display(Name = "Mode Of Identification")]
        public string ModeOfIdentification { get; set; }
        [Display(Name = "Identification Number")]
        public string IdentificationNumber { get; set; }
        [Display(Name = "Contact Address")]
        public string ContactAddress { get; set; }
        [Display(Name = "State of Origin")]
        public string StateofOrigin { get; set; }
        [Display(Name = "Local Government Area")]
        public string LocalGovernmentArea { get; set; }
        [Display(Name = "Current Occupation")]
        public string CurrentOccupation { get; set; }
        
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [UniquePhone(ErrorMessage = "This Phone Number is alread taken.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public string VerifyCode { get; set; }
        public string Sector { get; set; }
        public string SubSector { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Parish { get; set; }
        public string ParishState { get; set; }


        public int? SchoolId { get; set; }

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

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
