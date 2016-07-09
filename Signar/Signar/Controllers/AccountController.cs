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
using System.Web.Caching;

namespace Signar.Controllers
{
    [CustomAuthenticate]
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                using (var userService = new UserService())
                {
                    bool auth = userService.AuthenticateUser(model);
                    if (!auth)
                    {
                        ModelState.AddModelError("", "Sorry, your login or password are incorrect. Please try again.");
                        return View(model);
                    }
                }
                HttpContext.Cache[model.Login] = model;
                DateTime startDate = DateTime.Now;
                DateTime expDate = startDate.AddMinutes(20);
                var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user","/");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                if (!string.IsNullOrEmpty(headerToken))
                {
                    Response.Cookies.Add(new HttpCookie("auth", headerToken));
                    return RedirectToAction("DashBoard", "Home", new { area = "" });
                    //return string.IsNullOrEmpty(returnUrl) ? Redirect("/") : Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "failed to create token");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            //using (var userService = new UserService())
            //{
            //    UserDTO admin = new UserDTO();
            //    admin.Email = "fedorovsergiy94@gmail.com";
            //    admin.Password = "123iopJKL";
            //    admin.Name = "Sergiy";
            //    admin.Surname = "Fedorov";
            //    admin.IsAdmin = true;
            //    admin.Login = "Administrator";
            //    userService.CreateItem(admin);
            //}
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(EmailModel model)
        {
            return Redirect("ResetPassword");
        }

        }
}