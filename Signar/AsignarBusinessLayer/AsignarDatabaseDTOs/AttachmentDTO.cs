using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class AttachmentDTO
    {
        public int AttachmentID { get; set; }
        
        public int BugID { get; set; }
        
        public string Name { get; set; }
        
        public string ContentPath { get; set; }

        public Stream FileStream { get; set; }

        public AttachmentDTO()
        {

        }
    }
}
