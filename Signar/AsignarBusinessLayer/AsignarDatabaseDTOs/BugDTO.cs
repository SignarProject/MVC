using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class BugDTO
    {
        public int BugID { get; set; }


        public string BugName { get; set; }


        public string Subject { get; set; }


        public string Description { get; set; }


        public int? AssigneeID { get; set; }


        public int ProjectID { get; set; }


        public ICollection<AttachmentDTO> Attachments { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public DateTime CreationDate { get; set; }


        public DateTime ModificationDate { get; set; }


        public sbyte BugStatus { get; set; }


        public sbyte Priority { get; set; }


        public BugDTO()
        {
            Attachments = new HashSet<AttachmentDTO>();
            Comments = new HashSet<CommentDTO>();
        }
    }
}
