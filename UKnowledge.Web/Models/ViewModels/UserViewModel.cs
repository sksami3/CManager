using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UKnowledge.Web.Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Must be between 4 and 50 characters", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        //public IList<string> Roles { get; set; }
        [Required]
        [DisplayName("Roles")]
        public string RoleId { get; set; }
        [Required]
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }
    }
}
