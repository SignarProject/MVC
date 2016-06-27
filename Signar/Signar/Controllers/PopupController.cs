using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Signar.Models;

namespace Signar.Controllers
{
    public class PopupController : Controller
    {
        // GET: Popup
        public ActionResult CreateNewUser()
        {
            return View();
        }
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
    }

}