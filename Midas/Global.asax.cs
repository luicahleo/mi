using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MIDAS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Errores",                                              // Route name
                "Error/{error}",                           // URL with parameters
                new { controller = "Error", action = "index" }  // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Account", action = "LogOn", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "Elementos",                                              // Route name
                "{controller}/{action}/{accion}/{idElemento}",                           // URL with parameters
                new { controller = "Account", action = "LogOn", accion = "0", idElemento = "0" }  // Parameter defaults
            );

            routes.MapRoute(
                "Inspecciones",                                              // Route name
                "{controller}/{action}/{elemento}/{revision}",                           // URL with parameters
                new { controller = "Account", action = "LogOn", elemento = "0", revision = "0" }  // Parameter defaults
            );

            routes.MapRoute(
                "Inspeccion",                                              // Route name
                "{controller}/{action}/{idElemento}/{idRevision}",                           // URL with parameters
                new { controller = "Account", action = "LogOn", idElemento = "0", idRevision = "0" }  // Parameter defaults
            );

            routes.MapRoute(
                "AccionMejora",                                              // Route name
                "{controller}/{action}/{id}/{modulo}",                           // URL with parameters
                new { controller = "AccionMejora", action = "eliminar_accionmejora", id = 0, modulo = 0 }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
