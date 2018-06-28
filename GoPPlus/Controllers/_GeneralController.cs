using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using System.Linq;
using System.Web.Mvc;
using System;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public partial class _GeneralController : Controller
    {        
        #region Variables

        protected GoPSEntities db = new GoPSEntities();
        protected DBServicios serv = new DBServicios();
        protected DBValidaciones valid = new DBValidaciones();
        protected Utilities util = new Utilities();

        #endregion

        #region Properties

        
        public string MensajeDelete
        {
            get
            {
                string mess = db.ObtenerConfiguracion("MensajeEliminacion").FirstOrDefault();
                return mess.ToString();
            }
        }

        public int AtrasoYellow
        {
            get
            {
                string mess = db.ObtenerConfiguracion("AtrasoYellow").FirstOrDefault();
                return Convert.ToInt32(mess);
            }
        }

        public int AtrasoWhite
        {
            get
            {
                string mess = db.ObtenerConfiguracion("AtrasoWhite").FirstOrDefault();
                return Convert.ToInt32(mess);
            }
        }

        public string MensajeNotFound
        {
            get
            {
                string mnf = db.ObtenerConfiguracion("MensajeNotFound").FirstOrDefault();
                return mnf.ToString();
            }
        }

        #endregion

        #region Métodos

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Acciones

        [HasPermission("Configuraciones_Edicion")]
        public ActionResult ItemNotFound(int? id)
        {

           ViewBag.Mess = TempData["Mess"];
           ViewBag.NavBar = TempData["NavBar"];
           ViewBag.BackLink = TempData["BackLink"];

            return View();
        }

        #endregion

    }
}