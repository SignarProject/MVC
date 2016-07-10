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
        public ActionResult CreateNewUser(CreateNewUserModel model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(1, "Input data is invalid");
            if (!model.Password.Equals(model.ConfPassword)) return new HttpStatusCodeResult(4, "Passwords do not match");
            UserDTO user = new UserDTO();
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Login = model.Email;
            user.Email = model.Email;
            user.Password = model.Password;
            user.IsAdmin = model.IsAdmin;
            try
            {
                using (var userService = new UserService())
                {
                    if (!userService.CreateItem(user)) return new HttpStatusCodeResult(5, "This Email already exists. Try another");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(6, "Error during creation");
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(EditUserDataModel model)
        {
            UserDTO user = HttpContext.Cache[HttpContext.User.Identity.Name] as UserDTO;
            if (user.Name.Equals(model.Name) && user.Surname.Equals(model.Surname) && user.Email.Equals(model.Email)
                && user.IsAdmin == model.IsAdmin) return new HttpStatusCodeResult(3, "Nothing to update");
            if (ModelState.IsValid && user != null)
            {

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.IsAdmin = model.IsAdmin;
                using (var userService = new UserService())
                {
                    userService.UpdateItem(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Sorry, but there was an error");
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            if (model.OldPassword.Equals(model.NewPassword)) return new HttpStatusCodeResult(3, "Nothing to update");
            bool fail = false;
            using (UserService userService = new UserService())
            {
                UserDTO user = HttpContext.Cache[HttpContext.User.Identity.Name] as UserDTO;
                if (user == null) fail = true;
                else
                {
                    if (!userService.UpdatePassword(model.OldPassword, model.NewPassword, user.UserID)) fail = true;
                }
            }
            if (fail)
            {
                return new HttpStatusCodeResult(2, "Old password is incorrect");
            }
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

        [HttpPost]
        public bool DeleteProject(int ProjectID)
        {
            using (var projectService = new ProjectService())
            {
                bool res = projectService.DeleteItem(ProjectID);
                return res;
            }
        }

        [HttpPost]
        public bool DeleteUser(int UserID)
        {
            using (var userService = new UserService())
            {
                bool res = userService.DeleteItem(UserID);
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
            using (var userService = new UserService())
            {
                return View(userService.GetAllItems());
            }
        }

        [CustomAuthorize]
        public ActionResult Signout()
        {
            Response.Cookies.Add(new HttpCookie("auth", null));
            Session.Abandon();
            HttpContext.Cache.Remove(User.Identity.Name);
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}