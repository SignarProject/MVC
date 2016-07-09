using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Services;
using AsignarBusinessLayer.Services.ServiceInterfaces;
using CustomAuth.Filters;
using System.Web.Security;
using Signar.Models;


namespace Signar.Controllers
{
    
    [CustomAuthenticate]
    public class HomeController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(EditUserDataModel model)
        {
            UserDTO user = HttpContext.Cache[HttpContext.User.Identity.Name] as UserDTO;
            if (user.Name.Equals(model.Name) && user.Surname.Equals(model.Surname) && user.Email.Equals(model.Email)
                && user.IsAdmin == model.IsAdmin) return RedirectToAction("MyProfile");
            if (ModelState.IsValid || user == null)
            {
                
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.IsAdmin = model.IsAdmin;
                using (var userService = new UserService())
                {
                    userService.UpdateItem(user);
                }
            } else ModelState.AddModelError("", "Sorry, but there was an error");
            return RedirectToAction("MyProfile");
        }

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

        [CustomAuthorize]
        public ActionResult Signout()
        {
            Response.Cookies.Add(new HttpCookie("auth", null));
            Session.Abandon();
            HttpContext.Cache[User.Identity.Name] = null;
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}