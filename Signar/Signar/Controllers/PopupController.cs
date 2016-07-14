using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Services;
using CustomAuth.Filters;

namespace Signar.Controllers
{
    [CustomAuthenticate]
    public class PopupController : Controller
    {
        // GET: Popup
        public ActionResult CreateNewUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateNewProject()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult EditUserData()
        {
            return View();
        }

        public ActionResult AddUsersToProject(ICollection<ProjectDTO> model)
        {
            return View(model);
        }
    }

}