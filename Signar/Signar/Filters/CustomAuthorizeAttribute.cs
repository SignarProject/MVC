using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomAuth.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //Simple checker
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        //Core authentication, called before each action. False result will cause HandleUnauthorizedRequest triggered
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User.Identity;

            if (!user.IsAuthenticated)
            {
                return false;
            }

            return (string.IsNullOrEmpty(this.Users) || this.Users.Split(',').Any(x => x.Equals(user.Name))) &&
                   (string.IsNullOrEmpty(this.Roles) || this.Roles.Split(',').Any(x => httpContext.User.IsInRole(x)));
        }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //User isn't logged in

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
            }

            //User is logged in but has no access
            else {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
            }
        }
    }
}