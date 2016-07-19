using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.SerializationSignatures;
using System.ComponentModel.DataAnnotations;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class FilterInfoDTO
    {
        public FilterDTO filter = new FilterDTO();
        public ICollection<UserDTO> users;
        public ICollection<ProjectDTO> projects;
        public ICollection<int> users_id;
        public ICollection<int> projects_id;

        public FilterInfoDTO()
        {
            users = new List<UserDTO>();
            projects = new List<ProjectDTO>();
            users_id = new List<int>();
            projects_id = new List<int>();
        }
    }
}
