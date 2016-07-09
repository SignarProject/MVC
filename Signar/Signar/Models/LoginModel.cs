using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CustomAuth.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Login")]
        [StringLength(254, ErrorMessage = "Login name is too long")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}