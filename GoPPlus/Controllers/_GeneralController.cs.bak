using GoPS.Models;
using System.Web.Mvc;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public partial class _GeneralController : Controller
    {
        //*******************************************************************
        //*** Variables locales
        //*******************************************************************

        #region Variables

        protected GoPSEntities db = new GoPSEntities();
        protected DBServicios serv = new DBServicios();
        protected DBValidaciones valid = new DBValidaciones();
        protected Utilities util = new Utilities();

        #endregion

        //*******************************************************************
        //*** Métodos
        //*******************************************************************

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
    }
}