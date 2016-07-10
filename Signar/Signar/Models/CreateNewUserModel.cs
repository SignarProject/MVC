using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signar.Models
{
    public class CreateNewUserModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(254, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(254, ErrorMessage = "Surname is too long")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(254, ErrorMessage = "Email is too long")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(254, ErrorMessage = "Password is too long")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfPassword")]
        public string ConfPassword { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}