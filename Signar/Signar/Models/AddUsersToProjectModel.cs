using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AsignarBusinessLayer.AsignarDatabaseDTOs;

namespace Signar.Models
{
    public class AddUsersToProjectModel
    {
        public ICollection<UserDTO> users { get; set; }
        public List<bool> user_checked { get; set; }
        public List<int> users_id { get; set; }
        public int ProjectID { get; set; }
        public int curIndex { get; set; } = 0;
    }
}