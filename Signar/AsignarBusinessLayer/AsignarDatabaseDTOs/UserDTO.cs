using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class UserDTO
    {
        
        public int UserID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }


        public string AvatarPath { get; set; }

        [Required]
        [Display(Name = "Login")]
        [StringLength(254, ErrorMessage = "Login name is too long")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        public bool IsAdmin { get; set; }


        public ICollection<ProjectDTO> Projects { get; set; }


        public ICollection<BugDTO> Bugs { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public ICollection<FilterDTO> Filters { get; set; }


        public UserDTO()
        {
            Projects = new HashSet<ProjectDTO>();
            Bugs = new HashSet<BugDTO>();
            Comments = new HashSet<CommentDTO>();
            Filters = new HashSet<FilterDTO>();
        }
    }
}
