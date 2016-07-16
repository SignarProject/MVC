using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.SerializationSignatures;
using System.ComponentModel.DataAnnotations;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{    
    public class BugDTO
    {
        public int BugID { get; set; }
        
        public string Prefix { get; set; }
        [Required]
        [Display(Name = "Title")]
        [StringLength(254, ErrorMessage = "Title is too long")]
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? AssigneeID { get; set; }
        
        public UserDTO User { get; set; }
        [Required]
        public int ProjectID { get; set; }

        public ProjectDTO Project { get; set; }
        
        public ICollection<AttachmentDTO> Attachments { get; set; }


        public ICollection<CommentDTO> Comments { get; set; }


        public DateTime CreationDate { get; set; }


        public DateTime ModificationDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public Priority Priority { get; set; }


        public BugDTO()
        {
            CreationDate = DateTime.Now;
            Attachments = new HashSet<AttachmentDTO>();
            Comments = new HashSet<CommentDTO>();
        }
    }
}
