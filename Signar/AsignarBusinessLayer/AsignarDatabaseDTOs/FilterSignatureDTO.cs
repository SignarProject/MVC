using AsignarDataAccessLayer.SerializationSignatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class FilterSignatureDTO
    {
        public string SearchString { get; set; }

        public ICollection<ProjectDTO> Projects { get; set; }

        public ICollection<UserDTO> Assignees { get; set; }

        public ICollection<Priority> Priorities { get; set; }

        public ICollection<Status> Statuses { get; set; }

        public FilterSignatureDTO()
        {

        }
    }
}
