using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.AsignarDatabaseDTOs
{
    public class FilterDTO
    {
        public int FilterID { get; set; }


        public int UserID { get; set; }


        public string Title { get; set; }


        public string FilterContent { get; set; }


        public FilterDTO()
        {

        }
    }
}
