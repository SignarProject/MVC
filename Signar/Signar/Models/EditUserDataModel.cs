using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signar.Models
{
    public class EditUserDataModel
    {
        [Required]
        [Display(Name = "Email")]
        [StringLength(254, ErrorMessage = "Email is too long")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(35, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(35, ErrorMessage = "Name is too long")]
        public string Surname { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}