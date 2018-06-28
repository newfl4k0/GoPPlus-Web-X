using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Classes;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class FormasPagoController : _GeneralController 
    {
        // GET: FormasPago
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.FormasPago.ToList());
        }

        // GET: FormasPago/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormasPago formasPago = db.FormasPago.Find(id);
            if (formasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFormaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(formasPago);
        }

        // GET: FormasPago/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormasPago/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_FormaPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] FormasPago formasPago)
        {
            valid.ValidarFormaPago(ModelState, formasPago);
            if (ModelState.IsValid)
            {
                formasPago.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                formasPago.UserID = User.Identity.GetUserId();
                db.FormasPago.Add(formasPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formasPago);
        }

        // GET: FormasPago/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormasPago formasPago = db.FormasPago.Find(id);
            if (formasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFormaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(formasPago);
        }

        // POST: FormasPago/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_FormaPago,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] FormasPago formasPago)
        {
            valid.ValidarFormaPago(ModelState, formasPago);
            if (ModelState.IsValid)
            {
                formasPago.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                formasPago.UserID = User.Identity.GetUserId();
                db.Entry(formasPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formasPago);
        }

        // GET: FormasPago/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormasPago formasPago = db.FormasPago.Find(id);
            if (formasPago == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatFormaPago";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.ID_FormaPago == formasPago.ID_FormaPago).Count() == 0;
            return View(formasPago);
        }

        // POST: FormasPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarFormaPago(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
