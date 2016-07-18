using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.AzureADBModel;
using System.Xml.Serialization;

namespace AsignarDataAccessLayer.SerializationSignatures
{
    public enum Status
    {
        Open = 0,
        InProgress = 1,
        Done = 2,
        InTesting = 3,
        Closed = 4
    }
    
    public enum Priority
    {
        Critical = 0,
        Urgent = 1,
        Major = 2,
        Minor = 3
    }

    [Serializable]
    [XmlInclude(typeof(Project))]
    [XmlInclude(typeof(User))]
    public class FilterSignature
    {
        public string SearchString { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<User> Assignees { get; set; }

        public ICollection<Priority> Priorities { get; set; }

        public ICollection<Status> Statuses { get; set; }

        public FilterSignature()
        {

        }
    }
}
