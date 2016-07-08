using AsignarBusinessLayer.AsignarDatabaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.Services.ServiceInterfaces
{
    interface IAuthenticationService<T> where T : UserDTO
    {
        bool AuthenticateUser(UserDTO user);
    }
}
