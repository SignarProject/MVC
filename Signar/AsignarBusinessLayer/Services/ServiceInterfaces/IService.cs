using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.Services.ServiceInterfaces
{
    interface IService<T> where T : class
    {
        T GetItem(int id);
        ICollection<T> GetAllItems();
        bool CreateItem(T newItem);
        bool UpdateItem(T updatedItem);
        bool DeleteItem(int id);

    }
}
