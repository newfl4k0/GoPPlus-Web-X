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
    public class TiposAvisosController : _GeneralController 
    {
        // GET: TiposAvisos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposAvisos.ToList());
        }

        // GET: TiposAvisos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposAvisos tiposAvisos = db.TiposAvisos.Find(id);
            if (tiposAvisos == null)
            {
                return HttpNotFound();
            }
            return View(tiposAvisos);
        }

        // GET: TiposAvisos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposAvisos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoAviso,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposAvisos tiposAvisos)
        {
            valid.ValidarTipoAviso(ModelState, tiposAvisos);
            if (ModelState.IsValid)
            {
                tiposAvisos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposAvisos.UserID = User.Identity.GetUserId();
                db.TiposAvisos.Add(tiposAvisos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposAvisos);
        }

        // GET: TiposAvisos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposAvisos tiposAvisos = db.TiposAvisos.Find(id);
            if (tiposAvisos == null)
            {
                return HttpNotFound();
            }
            return View(tiposAvisos);
        }

        // POST: TiposAvisos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoAviso,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposAvisos tiposAvisos)
        {
            valid.ValidarTipoAviso(ModelState, tiposAvisos);
            if (ModelState.IsValid)
            {
                tiposAvisos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposAvisos.UserID = User.Identity.GetUserId();
                db.Entry(tiposAvisos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposAvisos);
        }

        // GET: TiposAvisos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposAvisos tiposAvisos = db.TiposAvisos.Find(id);
            if (tiposAvisos == null)
            {
                return HttpNotFound();
            }
            return View(tiposAvisos);
        }

        // POST: TiposAvisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoAviso(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
