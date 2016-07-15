using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.Services.ServiceInterfaces
{
    interface IExtendedService<T> : IService<T>, IPagingService<T> where T: class
    {
    }
}
