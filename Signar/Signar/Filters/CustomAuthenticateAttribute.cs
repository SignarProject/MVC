using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using System.Web.Security;

namespace CustomAuth.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthenticateAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (SkipAuthorization(filterContext.ActionDescriptor))
            {
                return;
            }

            var cookieValue = filterContext.HttpContext.Request.Cookies.Get("auth");

            if (cookieValue != null && !string.IsNullOrEmpty(cookieValue.Value))
            {
                var user = FormsAuthentication.Decrypt(cookieValue.Value);

                if (user != null && !user.Expired)
                {
                    filterContext.Principal = new GenericPrincipal(new GenericIdentity(user.Name), user.UserData.Split(','));
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (SkipAuthorization(filterContext.ActionDescriptor) || filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login",
                            //returnUrl = filterContext.HttpContext.Request.Url.LocalPath
                        }));
        }

        private static bool SkipAuthorization(ActionDescriptor actionDescriptor)
        {
            Contract.Assert(actionDescriptor != null);

            return actionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                         || actionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }
    }
}