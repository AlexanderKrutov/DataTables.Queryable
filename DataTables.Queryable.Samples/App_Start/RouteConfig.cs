using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataTables.Queryable.Samples
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "WebUI", action = "Sample1" }
            );

            routes.MapRoute(
                name: "DataTables",
                url: "{controller}/{action}",
                defaults: new { controller = "DataTables", action = "" }
            );
        }
    }
}
