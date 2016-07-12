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
using CustomAuth.Filters;

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
                int ExpireTime = model.RememberMe ? 100500 : 20;
                DateTime expDate = startDate.AddMinutes(ExpireTime);
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
            if (Request.Cookies["auth"] == null) return View();
            foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            {
                UserDTO model = _user.Value as UserDTO;
                if (model == null) continue;
                DateTime startDate = DateTime.Now;
                int ExpireTime = model.RememberMe ? 100500 : 20;
                DateTime expDate = startDate.AddMinutes(ExpireTime);
                var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                Response.Cookies.Add(new HttpCookie("auth", null));
                Response.Cookies.Add(new HttpCookie("auth", headerToken));
                HttpContext.Cache[model.Login] = model;
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }
            return View();
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
        }
        
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (Request.Cookies["auth"] == null) return View();
            foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            {
                UserDTO model = _user.Value as UserDTO;
                if (model == null) continue;
                DateTime startDate = DateTime.Now;
                int ExpireTime = model.RememberMe ? 100500 : 20;
                DateTime expDate = startDate.AddMinutes(ExpireTime);
                var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                Response.Cookies.Add(new HttpCookie("auth", null));
                Response.Cookies.Add(new HttpCookie("auth", headerToken));
                HttpContext.Cache[model.Login] = model;
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            if (Request.Cookies["auth"] == null) return View();
            foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            {
                UserDTO model = _user.Value as UserDTO;
                if (model == null) continue;
                DateTime startDate = DateTime.Now;
                int ExpireTime = model.RememberMe ? 100500 : 20;
                DateTime expDate = startDate.AddMinutes(ExpireTime);
                var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                Response.Cookies.Add(new HttpCookie("auth", null));
                Response.Cookies.Add(new HttpCookie("auth", headerToken));
                HttpContext.Cache[model.Login] = model;
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }
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