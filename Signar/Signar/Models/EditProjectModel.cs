using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Signar.Models
{
    public class EditProjectModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(254, ErrorMessage = "Title is too long")]
        public string Title { get; set; }
        public int ProjectID { get; set; }
    }
}