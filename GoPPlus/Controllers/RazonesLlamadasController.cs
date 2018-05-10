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
    public class RazonesLlamadasController : _GeneralController 
    {
        // GET: RazonesLlamadas
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.RazonesLlamadas.ToList());
        }

        // GET: RazonesLlamadas/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesLlamadas razonesLlamadas = db.RazonesLlamadas.Find(id);
            if (razonesLlamadas == null)
            {
                return HttpNotFound();
            }
            return View(razonesLlamadas);
        }

        // GET: RazonesLlamadas/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View(new RazonesLlamadas() { Activo = true });
        }

        // POST: RazonesLlamadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_RazonLlamada,Nombre,Activo,Fecha_Creacion,Fecha_Actualizacion,UserID")] RazonesLlamadas razonesLlamadas)
        {
            valid.ValidarRazonLlamada(ModelState, razonesLlamadas);
            if (ModelState.IsValid)
            {
                razonesLlamadas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                razonesLlamadas.UserID = User.Identity.GetUserId();
                db.RazonesLlamadas.Add(razonesLlamadas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(razonesLlamadas);
        }

        // GET: RazonesLlamadas/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesLlamadas razonesLlamadas = db.RazonesLlamadas.Find(id);
            if (razonesLlamadas == null)
            {
                return HttpNotFound();
            }
            return View(razonesLlamadas);
        }

        // POST: RazonesLlamadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_RazonLlamada,Nombre,Activo,Fecha_Creacion,Fecha_Actualizacion,UserID")] RazonesLlamadas razonesLlamadas)
        {
            valid.ValidarRazonLlamada(ModelState, razonesLlamadas);
            if (ModelState.IsValid)
            {
                razonesLlamadas.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                razonesLlamadas.UserID = User.Identity.GetUserId();
                db.Entry(razonesLlamadas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(razonesLlamadas);
        }

        // GET: RazonesLlamadas/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesLlamadas razonesLlamadas = db.RazonesLlamadas.Find(id);
            if (razonesLlamadas == null)
            {
                return HttpNotFound();
            }
            return View(razonesLlamadas);
        }

        // POST: RazonesLlamadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarRazonLlamada(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        
    }
}
