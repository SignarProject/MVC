namespace AsignarDataAccessLayer.AzureADBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Attachment
    {
        public int AttachmentID { get; set; }

        public int BugID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string ContentPath { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual Bug Bug { get; set; }
    }
}
