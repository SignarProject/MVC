using AsignarDataAccessLayer.SerializationSignatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class FilterSignatureDTO
    {
        [Display(Name = "SearchString")]
        [StringLength(254, ErrorMessage = "Search string is too long")]
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
