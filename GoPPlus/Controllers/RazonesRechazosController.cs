using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
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
    public class RazonesRechazosController : _GeneralController 
    {
        // GET: RazonesRechazos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var razonesRechazos = db.RazonesRechazos.Include(r => r.TiposRechazos);
            return View(razonesRechazos.ToList());
        }

        // GET: RazonesRechazos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesRechazos razonesRechazos = db.RazonesRechazos.Find(id);
            if (razonesRechazos == null)
            {
                return HttpNotFound();
            }
            return View(razonesRechazos);
        }

        // GET: RazonesRechazos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            ViewBag.ID_TipoRechazo = new SelectList(db.TiposRechazos.OrderBy(o => o.Nombre), "ID_TipoRechazo", "Nombre");
            return View(new RazonesRechazos() { Activo = true });
        }

        // POST: RazonesRechazos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_RazonRechazo,Nombre,Activo,ID_TipoRechazo,Fecha_Creacion,Fecha_Actualizacion,UserID")] RazonesRechazos razonesRechazos)
        {
            valid.ValidarRazonRechazo(ModelState, razonesRechazos);
            if (ModelState.IsValid)
            {
                razonesRechazos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                razonesRechazos.UserID = User.Identity.GetUserId();
                db.RazonesRechazos.Add(razonesRechazos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_TipoRechazo = new SelectList(db.TiposRechazos.OrderBy(o => o.Nombre), "ID_TipoRechazo", "Nombre", razonesRechazos.ID_TipoRechazo);
            return View(razonesRechazos);
        }

        // GET: RazonesRechazos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesRechazos razonesRechazos = db.RazonesRechazos.Find(id);
            if (razonesRechazos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_TipoRechazo = new SelectList(db.TiposRechazos.OrderBy(o => o.Nombre), "ID_TipoRechazo", "Nombre", razonesRechazos.ID_TipoRechazo);
            return View(razonesRechazos);
        }

        // POST: RazonesRechazos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_RazonRechazo,Nombre,Activo,ID_TipoRechazo,Fecha_Creacion,Fecha_Actualizacion,UserID")] RazonesRechazos razonesRechazos)
        {
            valid.ValidarRazonRechazo(ModelState, razonesRechazos);
            if (ModelState.IsValid)
            {
                razonesRechazos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                razonesRechazos.UserID = User.Identity.GetUserId();
                db.Entry(razonesRechazos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_TipoRechazo = new SelectList(db.TiposRechazos.OrderBy(o => o.Nombre), "ID_TipoRechazo", "Nombre", razonesRechazos.ID_TipoRechazo);
            return View(razonesRechazos);
        }

        // GET: RazonesRechazos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RazonesRechazos razonesRechazos = db.RazonesRechazos.Find(id);
            if (razonesRechazos == null)
            {
                return HttpNotFound();
            }
            return View(razonesRechazos);
        }

        // POST: RazonesRechazos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarRazonRechazo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Regresar")]
        public ActionResult Regresar()
        {
            return RedirectToAction("Index");
        }
        
    }
}
