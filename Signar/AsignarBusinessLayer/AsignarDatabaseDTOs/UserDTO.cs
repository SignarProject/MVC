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


        [Required]
        public string Name { get; set; }


        public string Surname { get; set; }


        public string Email { get; set; }


        public string AvatarPath { get; set; }


        public string Login { get; set; }


        public string Password { get; set; }


        public bool IsAdmin { get; set; }


        public ICollection<BugDTO> Bugs { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public ICollection<FilterDTO> Filters { get; set; }


        //public ICollection<UserToProjectDTO> UsersToProjects { get; set; }

        public UserDTO()
        {
            Bugs = new HashSet<BugDTO>();
            Comments = new HashSet<CommentDTO>();
            Filters = new HashSet<FilterDTO>();
            //UsersToProjects = new HashSet<UserToProjectDTO>();
        }
    }
}
