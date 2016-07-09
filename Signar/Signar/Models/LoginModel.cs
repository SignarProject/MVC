using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signar.Models
{
    public class EmailModel
    {
        [Required]
        [Display(Name = "Email")]
        [StringLength(254, ErrorMessage = "Email is too long")]
        public string Email { get; set; }
    }
}