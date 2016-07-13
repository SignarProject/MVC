using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomAuth.Filters;

namespace Signar.Controllers
{

    public class ErrorController : Controller
    {
        [CustomAuthenticate]
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}