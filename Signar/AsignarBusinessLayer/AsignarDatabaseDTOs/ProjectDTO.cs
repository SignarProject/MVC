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


        public string Name { get; set; }


        public string Prefix { get; set; }


        public bool IsDeleted { get; set; }


        public int BugsAmount { get; set; }


        public int UsersAmount { get; set; }


        public ICollection<BugDTO> Bugs { get; set; }


        //public ICollection<UsersToProjectsDTO> UsersToProjects { get; set; }


        public ProjectDTO()
        {
            Bugs = new HashSet<BugDTO>();
            //UsersToProjects = new HashSet<UsersToProjectsDTO>();
        }
    }
}
