namespace AsignarDataAccessLayer.AzureADBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Filter
    {
        public int FilterID { get; set; }

        public int UserID { get; set; }

        [StringLength(254)]
        public string Title { get; set; }

        public string FilterContent { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual User User { get; set; }
    }
}
