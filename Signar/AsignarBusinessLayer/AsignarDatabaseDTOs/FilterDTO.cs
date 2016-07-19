using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.SerializationSignatures;
using System.ComponentModel.DataAnnotations;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class FilterDTO
    {
        public int FilterID { get; set; }
        
        public int UserID { get; set; }
        [Required]
        [Display(Name = "Title")]
        [StringLength(30, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        public FilterSignatureDTO FilterSignarute { get; set; }

        public FilterDTO()
        {

        }
    }
}
