using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Filters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class TiposPrioridadesController : _GeneralController 
    {
        // GET: TiposPrioridades
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposPrioridades.ToList());
        }

        // GET: TiposPrioridades/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPrioridades tiposPrioridades = db.TiposPrioridades.Find(id);
            if (tiposPrioridades == null)
            {
                return HttpNotFound();
            }
            return View(tiposPrioridades);
        }

        // GET: TiposPrioridades/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposPrioridades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoPrioridad,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPrioridades tiposPrioridades)
        {
            valid.ValidarTipoPrioridad(ModelState, tiposPrioridades);
            if (ModelState.IsValid)
            {
                tiposPrioridades.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPrioridades.UserID = User.Identity.GetUserId();
                db.TiposPrioridades.Add(tiposPrioridades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposPrioridades);
        }

        // GET: TiposPrioridades/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPrioridades tiposPrioridades = db.TiposPrioridades.Find(id);
            if (tiposPrioridades == null)
            {
                return HttpNotFound();
            }
            return View(tiposPrioridades);
        }

        // POST: TiposPrioridades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoPrioridad,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposPrioridades tiposPrioridades)
        {
            valid.ValidarTipoPrioridad(ModelState, tiposPrioridades);
            if (ModelState.IsValid)
            {
                tiposPrioridades.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposPrioridades.UserID = User.Identity.GetUserId();
                db.Entry(tiposPrioridades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposPrioridades);
        }

        // GET: TiposPrioridades/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPrioridades tiposPrioridades = db.TiposPrioridades.Find(id);
            if (tiposPrioridades == null)
            {
                return HttpNotFound();
            }
            return View(tiposPrioridades);
        }

        // POST: TiposPrioridades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoPrioridad(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
