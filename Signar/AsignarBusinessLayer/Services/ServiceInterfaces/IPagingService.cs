using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.SortEnum;
using AsignarBusinessLayer.AsignarDatabaseDTOs;

namespace AsignarBusinessLayer.Services.ServiceInterfaces
{
    interface IPagingService<T> where T : class
    {
        ICollection<T> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce);
    }
}
