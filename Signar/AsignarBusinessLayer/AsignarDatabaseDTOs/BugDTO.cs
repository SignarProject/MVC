using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
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


    public class BugDTO
    {
        public int BugID { get; set; }


        public string Prefix { get; set; }


        public string Subject { get; set; }


        public string Description { get; set; }


        public int? AssigneeID { get; set; }


        public int ProjectID { get; set; }


        public ICollection<AttachmentDTO> Attachments { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public DateTime CreationDate { get; set; }


        public DateTime ModificationDate { get; set; }


        public Status Status { get; set; }


        public Priority Priority { get; set; }


        public BugDTO()
        {
            Attachments = new HashSet<AttachmentDTO>();
            Comments = new HashSet<CommentDTO>();
        }
    }
}
