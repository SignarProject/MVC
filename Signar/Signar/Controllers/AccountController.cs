﻿using System;
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
                DateTime expDate = model.RememberMe ? startDate.AddDays(365) : startDate.AddMinutes(20);
                var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user","/");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                if (!string.IsNullOrEmpty(headerToken))
                {
                    var x = new HttpCookie("auth", headerToken);
                    if (model.RememberMe) x.Expires = DateTime.Now.AddDays(365);
                    Response.Cookies.Add(x);
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
        [CustomAuthenticate]
        public ActionResult Login()
        {
            /*using (var filterService = new FilterService())
            {
                using (var userService = new UserService())
                {
                    using (var projectService = new ProjectService())
                    {
                        var filterDTO = new FilterDTO();
                        filterDTO.Title = "The Second Filter";
                        filterDTO.UserID = 47;
                        filterDTO.FilterSignarute.SearchString = "bug";

                        filterDTO.FilterSignarute.Projects.Add(projectService.GetItem(4));
                        filterDTO.FilterSignarute.Assignees.Add(userService.GetItem(1));
                        filterDTO.FilterSignarute.Assignees.Add(userService.GetItem(6));
                        filterDTO.FilterSignarute.Assignees.Add(userService.GetItem(46));

                        filterDTO.FilterSignarute.Priorities.Add(PriorityDTO.Major);
                        filterDTO.FilterSignarute.Priorities.Add(PriorityDTO.Urgent);
                        filterDTO.FilterSignarute.Statuses.Add(StatusDTO.InTesting);
                        filterDTO.FilterSignarute.Statuses.Add(StatusDTO.Closed);
                        filterDTO.FilterSignarute.Statuses.Add(StatusDTO.Done);

                        filterService.CreateItem(filterDTO);
                    }
                }
            }*/

            /*
            using (var filterService = new FilterService())
            {
                var filter = filterService.GetItem(13);

                using (var bugService = new BugService())
                {
                    var bugCollection = bugService.AdvancedSearch(filter);
                }
            }*/

                if (Request.Cookies["auth"] != null) return RedirectToAction("Dashboard", "Home", new { area = "" });                               
            //if (Request.Cookies["auth"] == null) return View();
            //foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            //{
            //    UserDTO model = _user.Value as UserDTO;
            //    if (model == null) continue;
            //    DateTime startDate = DateTime.Now;
            //    DateTime expDate = model.RememberMe ? startDate.AddDays(365) : startDate.AddMinutes(20);
            //    var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
            //    var headerToken = FormsAuthentication.Encrypt(userToken);
            //    Response.Cookies.Add(new HttpCookie("auth", null));
            //    var x = new HttpCookie("auth", headerToken);
            //    if (model.RememberMe) x.Expires = DateTime.Now.AddDays(365);
            //    Response.Cookies.Add(x);
            //    HttpContext.Cache[model.Login] = model;
            //    return RedirectToAction("Dashboard", "Home", new { area = "" });
            //}
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
        [CustomAuthenticate]
        public ActionResult ForgotPassword()
        {
            if (Request.Cookies["auth"] != null) return RedirectToAction("Dashboard", "Home", new { area = "" });
            //if (Request.Cookies["auth"] == null) return View();
            //foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            //{
            //    UserDTO model = _user.Value as UserDTO;
            //    if (model == null) continue;
            //    DateTime startDate = DateTime.Now;
            //    DateTime expDate = model.RememberMe ? startDate.AddDays(365) : startDate.AddMinutes(20);
            //    var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
            //    var headerToken = FormsAuthentication.Encrypt(userToken);
            //    Response.Cookies.Add(new HttpCookie("auth", null));
            //    var x = new HttpCookie("auth", headerToken);
            //    if (model.RememberMe) x.Expires = DateTime.Now.AddDays(365);
            //    Response.Cookies.Add(x); ;
            //    HttpContext.Cache[model.Login] = model;
            //    return RedirectToAction("Dashboard", "Home", new { area = "" });
            //}
            return View();
        }
        
        [AllowAnonymous]
        [CustomAuthenticate]
        public ActionResult ResetPassword(int id)
        {
            if (Request.Cookies["auth"] != null) return RedirectToAction("Dashboard", "Home", new { area = "" });

            UserDTO me = HttpContext.Cache[User.Identity.Name] as UserDTO;

            var notificationQueue = new NotificationQueueService();
            //if (Request.Cookies["auth"] == null) return View();
            //foreach (System.Collections.DictionaryEntry _user in HttpContext.Cache)
            //{
            //    UserDTO model = _user.Value as UserDTO;
            //    if (model == null) continue;
            //    DateTime startDate = DateTime.Now;
            //    DateTime expDate = model.RememberMe ? startDate.AddDays(365) : startDate.AddMinutes(20);
            //    var userToken = new FormsAuthenticationTicket(1, model.Login, startDate, expDate, model.RememberMe, model.IsAdmin ? "admin" : "user", "/");
            //    var headerToken = FormsAuthentication.Encrypt(userToken);
            //    Response.Cookies.Add(new HttpCookie("auth", null));
            //    var x = new HttpCookie("auth", headerToken);
            //    if (model.RememberMe) x.Expires = DateTime.Now.AddDays(365);
            //    Response.Cookies.Add(x);
            //    HttpContext.Cache[model.Login] = model;
            //    return RedirectToAction("Dashboard", "Home", new { area = "" });
            //}
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(EmailModel model)
        {
            if (!ModelState.IsValid) ModelState.AddModelError("", "Please enter correct email.");

            var userService = new UserService();

            UserDTO user = userService.GetItemByEmail(model.Email);

            userService.ResetUserPassword(user);

            return RedirectToAction("Login");
        }

        }
}