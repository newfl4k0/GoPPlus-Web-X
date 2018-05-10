using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
 
using Microsoft.AspNet.Identity;
using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class SegurosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Seguros
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Seguros.ToList());
        }

        // GET: Seguros/Details/5
         
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seguros seguros = db.Seguros.Find(id);
            if (seguros == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(seguros);
        }

        // GET: Seguros/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Seguros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Seguro,Nombre,Telefono,EsCelular,Fecha_Creacion,Fecha_Actualizacion,UserID")] Seguros seguros)
        {
            valid.ValidarSeguro(ModelState, seguros);
            if (ModelState.IsValid)
            {
                seguros.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                seguros.UserID = User.Identity.GetUserId();
                db.Seguros.Add(seguros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seguros);
        }

        // GET: Seguros/Edit/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seguros seguros = db.Seguros.Find(id);
            if (seguros == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(seguros);
        }

        // POST: Seguros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Seguro,Nombre,Telefono,EsCelular,Fecha_Creacion,Fecha_Actualizacion,UserID")] Seguros seguros)
        {
            valid.ValidarSeguro(ModelState, seguros);
            if (ModelState.IsValid)
            {
                seguros.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                seguros.UserID = User.Identity.GetUserId();
                db.Entry(seguros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seguros);
        }

        // GET: Seguros/Delete/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seguros seguros = db.Seguros.Find(id);
            if (seguros == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(seguros);
        }

        // POST: Seguros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarSeguro(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
