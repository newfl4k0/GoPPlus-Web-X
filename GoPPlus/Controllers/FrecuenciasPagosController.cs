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
    public class FrecuenciasPagosController : _GeneralController 
    {
        // GET: FrecuenciasPagos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.FrecuenciasPago.ToList());
        }

        // GET: FrecuenciasPagos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrecuenciasPago frecuenciasPago = db.FrecuenciasPago.Find(id);
            if (frecuenciasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFrecuenciaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(frecuenciasPago);
        }

        // GET: FrecuenciasPagos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FrecuenciasPagos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_FrecuenciaPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] FrecuenciasPago frecuenciasPago)
        {
            valid.ValidarFrecuenciaPago(ModelState, frecuenciasPago);
            if (ModelState.IsValid)
            {
                frecuenciasPago.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                frecuenciasPago.UserID = User.Identity.GetUserId();
                db.FrecuenciasPago.Add(frecuenciasPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(frecuenciasPago);
        }

        // GET: FrecuenciasPagos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrecuenciasPago frecuenciasPago = db.FrecuenciasPago.Find(id);
            if (frecuenciasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFrecuenciaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(frecuenciasPago);
        }

        // POST: FrecuenciasPagos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_FrecuenciaPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] FrecuenciasPago frecuenciasPago)
        {
            valid.ValidarFrecuenciaPago(ModelState, frecuenciasPago);
            if (ModelState.IsValid)
            {
                frecuenciasPago.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                frecuenciasPago.UserID = User.Identity.GetUserId();
                db.Entry(frecuenciasPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(frecuenciasPago);
        }

        // GET: FrecuenciasPagos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrecuenciasPago frecuenciasPago = db.FrecuenciasPago.Find(id);
            if (frecuenciasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFrecuenciaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.Afiliados.ID_FrecuenciaPago == frecuenciasPago.ID_FrecuenciaPago).Count() == 0;
            return View(frecuenciasPago);
        }

        // POST: FrecuenciasPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await serv.EliminarFrecuenciaPago(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        
    }
}
