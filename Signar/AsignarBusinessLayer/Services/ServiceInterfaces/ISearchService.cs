using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.Services.ServiceInterfaces
{
    interface ISearchService<T> where T : class
    {
        ICollection<T> SearchBy(string value, ICollection<T> searchCollection);
    }
}
