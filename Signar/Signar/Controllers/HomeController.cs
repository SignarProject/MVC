using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Services;
using AsignarBusinessLayer.Services.ServiceInterfaces;

namespace Signar.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public bool DeleteProject(int ProjectID)
        {
            using (var projectService = new ProjectService())
            {
                var res = projectService.DeleteItem(ProjectID);
                return res;
            }
        }


        public ActionResult Projects()
        {
            using (var projectService = new ProjectService())
            {
                return View(projectService.GetAllItems());
            }
        }

        public ActionResult Project()
        {
            return View();
        }

        public ActionResult Task()
        {
            return View();
        }

        public ActionResult DashBoard()
        {
            return View();
        }


        public ActionResult MyProfile()
        {
            return View();
        }

        
        public ActionResult Filters()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}