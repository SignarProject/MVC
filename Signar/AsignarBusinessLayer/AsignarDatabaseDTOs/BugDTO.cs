using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.SerializationSignatures;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{    
    public class BugDTO
    {
        public int BugID { get; set; }
        
        public string Prefix { get; set; }
        
        public string Subject { get; set; }
        
        public string Description { get; set; }
        
        public int? AssigneeID { get; set; }
        
        public UserDTO User { get; set; }
        
        public int ProjectID { get; set; }

        public ProjectDTO Project { get; set; }
        
        public ICollection<AttachmentDTO> Attachments { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public DateTime CreationDate { get; set; }


        public DateTime ModificationDate { get; set; }


        public Status Status { get; set; }


        public Priority Priority { get; set; }


        public BugDTO()
        {
            CreationDate = DateTime.Now;
            Attachments = new HashSet<AttachmentDTO>();
            Comments = new HashSet<CommentDTO>();
        }
    }
}
