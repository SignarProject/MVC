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
using CustomAuth.Models;

namespace Signar.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginMe(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                using (var userService = new UserService())
                {
                    userService.AuthenticateUser(model);
                }
                var userToken = new FormsAuthenticationTicket(1, model.Login, DateTime.Now, DateTime.Now.AddMinutes(10), false, model.IsAdmin ? "admin" : "user");
                var headerToken = FormsAuthentication.Encrypt(userToken);
                if (!string.IsNullOrEmpty(headerToken))
                {
                    Response.Cookies.Add(new HttpCookie("auth", headerToken));

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
    }
}