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
    public class TiposRechazosController : _GeneralController 
    {
        // GET: TiposRechazos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposRechazos.ToList());
        }

        // GET: TiposRechazos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposRechazos tiposRechazos = db.TiposRechazos.Find(id);
            if (tiposRechazos == null)
            {
                return HttpNotFound();
            }
            return View(tiposRechazos);
        }

        // GET: TiposRechazos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposRechazos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoRechazo,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposRechazos tiposRechazos)
        {
            valid.ValidarTipoRechazo(ModelState, tiposRechazos);
            if (ModelState.IsValid)
            {
                tiposRechazos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposRechazos.UserID = User.Identity.GetUserId();
                db.TiposRechazos.Add(tiposRechazos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposRechazos);
        }

        // GET: TiposRechazos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposRechazos tiposRechazos = db.TiposRechazos.Find(id);
            if (tiposRechazos == null)
            {
                return HttpNotFound();
            }
            return View(tiposRechazos);
        }

        // POST: TiposRechazos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoRechazo,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposRechazos tiposRechazos)
        {
            valid.ValidarTipoRechazo(ModelState, tiposRechazos);
            if (ModelState.IsValid)
            {
                tiposRechazos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposRechazos.UserID = User.Identity.GetUserId();
                db.Entry(tiposRechazos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposRechazos);
        }

        // GET: TiposRechazos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposRechazos tiposRechazos = db.TiposRechazos.Find(id);
            if (tiposRechazos == null)
            {
                return HttpNotFound();
            }
            return View(tiposRechazos);
        }

        // POST: TiposRechazos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoRechazo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
