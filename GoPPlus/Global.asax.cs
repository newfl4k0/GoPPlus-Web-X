using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GoPS
{
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Elimina el cache de las páginas, para eviatr que al navegar hacia atrás se siga mostrando la aplicación
        /// </summary>
        /// <Autor>
        /// ETW. Gerarod A. Ornelas Guerrero
        /// </Autor>
        /// <Fecha>
        /// 19/12/2017
        /// </Fecha>
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

       protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Session["active"] == null)
            {
                Session["active"] = "on";
                Session["cerrar"] = "no";
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["active"] = "off";
            Session["cerrar"] = "yes";
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                string lcReqPath = Request.Path.ToLower();
                System.Web.SessionState.HttpSessionState curSession = HttpContext.Current.Session;
                if (curSession == null || curSession["active"].ToString() == "off" || Session["cerrar"].ToString() == "yes")
                {
                    // Redirect nicely
                    Context.Server.ClearError();
                    Context.Response.AddHeader("Location", "/Account/Login");
                    Context.Response.TrySkipIisCustomErrors = true;
                    Context.Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
                    // End now end the current request so we dont leak.
                    Context.Response.Output.Close();
                    Context.Response.End();
                    return;
                }
            }
            catch (Exception ex)
            {
                string _error = ex.Message;
                // todo: handle exceptions nicely!
            }
        }
    }
}
