using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class CommentDTO
    {
        public int CommentID { get; set; }


        public int BugID { get; set; }


        public int? UserID { get; set; }


        public string Text { get; set; }


        public DateTime CreationDate { get; set; }


        public DateTime? ModificationDate { get; set; }


        public CommentDTO()
        {

        }
    }
}
