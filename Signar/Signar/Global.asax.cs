﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Signar.Controllers;

namespace Signar
{
    public class MvcApplication : System.Web.HttpApplication
    {
    //    protected void Application_EndRequest()
    //    {
    //        if (Context.Response.StatusCode == 404)
    //        {
    //            Response.Clear();

    //            var rd = new RouteData();
    //            rd.Values["controller"] = "Error";
    //            rd.Values["action"] = "Error404";

    //            IController c = new ErrorController();
    //            c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
    //        }
    //    }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
