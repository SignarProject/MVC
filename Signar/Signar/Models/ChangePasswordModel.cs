using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signar.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        [StringLength(254, ErrorMessage = "Old password is too long")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(254, ErrorMessage = "New password is too long")]
        public string NewPassword { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}