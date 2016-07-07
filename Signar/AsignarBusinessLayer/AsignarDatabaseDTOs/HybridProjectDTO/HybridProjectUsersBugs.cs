using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs.HybridProjectDTO
{
    // This DTO is using in Project_View, and maybe in Search_View.
    public class HybridProjectUsersBugs
    {

        public int ProjectID { get; set; }


        public string Name { get; set; }


        public bool IsDeleted { get; set; }


        public ICollection<UserDTO> Users { get; set; }


        public ICollection<BugDTO> Bugs { get; set; }


        public HybridProjectUsersBugs()
        {
            this.Users = new HashSet<UserDTO>();
            this.Bugs = new HashSet<BugDTO>();
        }
    }
}
