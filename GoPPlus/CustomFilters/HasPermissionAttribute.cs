using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;

namespace GoPS.CustomFilters
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        private List<string> permisosList { get; set; }
        private string role { get; set; }
        IOwinContext context;
        string userId;
        List<string> permisosUser= new List<string>();
        AspNetUserRoles userRole;
        private GoPSEntities db;

        public HasPermissionAttribute(string permisos, string role = "")
        {
            permisosList = permisos.Split(',').ToList();
            this.role = role;
            context = HttpContext.Current.GetOwinContext();
            db = new GoPSEntities();
        }

        /// <summary>
        /// Check for Authorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        /// <summary>
        /// Method to check if the user is Authorized or not
        /// if yes continue to perform the action else redirect to error page
        /// </summary>
        /// <param name="filterContext"></param>
        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            bool isAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            var vr = new ViewResult();
            ViewDataDictionary dict = new ViewDataDictionary();
            
            if (isAuthenticated)
            {
                userId = filterContext.HttpContext.User.Identity.GetUserId();
                AspNetUsers userR = db.AspNetUsers.Find(userId);
                userRole = db.AspNetUserRoles.Where(h => h.RoleId == userR.roles.ToString() && h.UserId == userId).FirstOrDefault();                
                permisosUser = userRole.AspNetRoles.Permissions.Select(p => p.Name).Distinct().ToList();
                filterContext.RouteData.Values.Add("PermisosUser", permisosUser.ToArray());
                filterContext.RouteData.Values.Add("SAdministrador", userRole.AspNetRoles.Name.ToUpper() == "SADMINISTRADOR");
                bool hasPerm = String.IsNullOrEmpty(role) ?
                                permisosUser.Any(permisosList.Contains) :
                                role == userRole.AspNetRoles.Name;
                string empresa = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "EMPRESA").FirstOrDefault().Valor;
                string logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
                string color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
                if (hasPerm)
                {
                    List<int> ID_Afiliados = userRole.AspNetRoles.Name.ToUpper() == "SADMINISTRADOR" && empresa.ToUpper() == "SPEQTRUM" ?
                                                db.Afiliados.Select(a => a.ID_Afiliado).ToList()
                                                : (userRole.AspNetRoles.Afiliado ?
                                                
                                                userRole.Afiliados.Where(e => e.Empresas.Nombre == empresa).Select(a => a.ID_Afiliado).ToList()
                                                : db.Afiliados.Where(e => e.Empresas.Nombre == empresa).Select(a => a.ID_Afiliado).ToList());
                    filterContext.RouteData.Values.Add("ID_Afiliados", ID_Afiliados);
                    List<int> ID_Empresas = userRole.AspNetRoles.Afiliado ?
                                                userRole.Afiliados.Select(a => a.ID_Empresa).Distinct().ToList()
                                                : db.Empresas.Select(a => a.ID_Empresa).ToList();
                    filterContext.RouteData.Values.Add("ID_Empresas", ID_Empresas);
                    filterContext.RouteData.Values.Add("Logo", logo);
                    filterContext.RouteData.Values.Add("Color", color);
                }
                else
                {
                    dict.Add("Message", "Lo sentimos, no estás autorizado para ver este contenido.");
                    vr.ViewName = "AuthorizeFailed";
                    vr.ViewData = dict;
                    var result = vr;
                    filterContext.Result = result;
                }
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
}
