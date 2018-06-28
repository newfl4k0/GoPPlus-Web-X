using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GoPS.Filters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class TiposPagosController : _GeneralController 
    {
        // GET: TiposPagos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposPagos.ToList());
        }

        // GET: TiposPagos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPagos tiposPagos = db.TiposPagos.Find(id);
            if (tiposPagos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposPagos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tiposPagos);
        }

        // GET: TiposPagos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposPagos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPagos tiposPagos)
        {
            valid.ValidarTipoPago(ModelState, tiposPagos);
            if (ModelState.IsValid)
            {
                tiposPagos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPagos.UserID = User.Identity.GetUserId();
                db.TiposPagos.Add(tiposPagos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposPagos);
        }

        // GET: TiposPagos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPagos tiposPagos = db.TiposPagos.Find(id);
            if (tiposPagos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposPagos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tiposPagos);
        }

        // POST: TiposPagos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPagos tiposPagos)
        {
            valid.ValidarTipoPago(ModelState, tiposPagos);
            if (ModelState.IsValid)
            {
                tiposPagos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPagos.UserID = User.Identity.GetUserId();
                db.Entry(tiposPagos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposPagos);
        }

        // GET: TiposPagos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPagos tiposPagos = db.TiposPagos.Find(id);
            if (tiposPagos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposPagos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.Afiliados.ID_TipoPago == tiposPagos.ID_TipoPago).Count() == 0;
            return View(tiposPagos);
        }

        // POST: TiposPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [HasPermission("Configuraciones_Edicion")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await serv.EliminarTipoPago(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
