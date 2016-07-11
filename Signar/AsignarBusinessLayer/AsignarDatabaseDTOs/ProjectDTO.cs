using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class ProjectDTO
    {
        public int ProjectID { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(254, ErrorMessage = "Title is too long")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Prefix")]
        [StringLength(10, ErrorMessage = "Prefix is too long")]
        public string Prefix { get; set; }


        public bool IsDeleted { get; set; }


        public int BugsAmount { get; set; }


        public int UsersAmount { get; set; }


        public ICollection<UserDTO> Users { get; set; }


        public ICollection<BugDTO> Bugs { get; set; }



        public ProjectDTO()
        {
            this.Users = new HashSet<UserDTO>();
            this.Bugs = new HashSet<BugDTO>();
        }
    }
}
