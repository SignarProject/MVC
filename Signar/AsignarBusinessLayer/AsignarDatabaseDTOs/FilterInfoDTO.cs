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
        public string Statuses { get; set; } = "";
        public string Priorities { get; set; } = "";
        public string UserIDs { get; set; } = "";
        public string ProjectIDs { get; set; } = "";

        public FilterInfoDTO()
        {
            users = new List<UserDTO>();
            projects = new List<ProjectDTO>();
        }
    }
}
