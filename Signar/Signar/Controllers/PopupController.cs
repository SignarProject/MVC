using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}