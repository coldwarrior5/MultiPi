using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using System.Web.Security;

namespace PPIJ.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool Admin { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lozinka mora biti najmanje {2} znakova duga.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Korisničko ime je potrebno za prijavu")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lozinka je potrebna za prijavu")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }


    public class RegisterModel
    {
        [Required(ErrorMessage = "Korisničko ime je potrebno")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email adresa je potrebna")]
        [EmailAddress(ErrorMessage = "Email adresa nije u dobrom formatu")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "TestEmail")]
        public string TestEmail { get; set; }

        [Required(ErrorMessage = "Lozinka je potrebna")]
        [StringLength(100, ErrorMessage = "Lozinka mora biti najmanje {2} znakova duga.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potrebno je potvrditi lozinku")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ime je potrebno")]
        [Display(Name = "First name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je potrebno")]
        [Display(Name = "Last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }

    public class AdminModel
    {
        public string Username { get; set; }

        public bool Admin { get; set; }

    }

    public class UserEditModel
    {
        public string Username { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail adresa")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Stara lozinka")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi novu lozinku")]
        public string NewPasswordConfirm { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
