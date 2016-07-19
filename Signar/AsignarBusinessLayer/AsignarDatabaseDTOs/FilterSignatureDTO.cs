using AsignarDataAccessLayer.SerializationSignatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public enum StatusDTO
    {
        Open = 0,
        InProgress = 1,
        Done = 2,
        InTesting = 3,
        Closed = 4
    }

    public enum PriorityDTO
    {
        Critical = 0,
        Urgent = 1,
        Major = 2,
        Minor = 3
    }

    public class FilterSignatureDTO
    {
        [Display(Name = "SearchString")]
        [StringLength(254, ErrorMessage = "Search string is too long")]
        public string SearchString { get; set; }

        public ICollection<ProjectDTO> Projects { get; set; }

        public ICollection<UserDTO> Assignees { get; set; }

        public ICollection<PriorityDTO> Priorities { get; set; }

        public ICollection<StatusDTO> Statuses { get; set; }

        public FilterSignatureDTO()
        {
            Projects = new HashSet<ProjectDTO>();
            Assignees = new HashSet<UserDTO>();
            Priorities = new HashSet<PriorityDTO>();
            Statuses = new HashSet<StatusDTO>();
            SearchString = "";
        }
    }
}
