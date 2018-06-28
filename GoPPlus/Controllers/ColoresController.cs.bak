using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.Classes;
using GoPS.CustomFilters;
using Microsoft.AspNet.Identity;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ColoresController : _GeneralController 
    {
        // GET: Colores
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Colores.ToList());
        }

        // GET: Colores/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colores colores = db.Colores.Find(id);
            if (colores == null)
            {
                return HttpNotFound();
            }
            return View(colores);
        }

        // GET: Colores/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Color,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colores colores)
        {
            valid.ValidarColor(ModelState, colores);
            if (ModelState.IsValid)
            {
                colores.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                colores.UserID = User.Identity.GetUserId();
                db.Colores.Add(colores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(colores);
        }

        // GET: Colores/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colores colores = db.Colores.Find(id);
            if (colores == null)
            {
                return HttpNotFound();
            }
            return View(colores);
        }

        // POST: Colores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Color,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colores colores)
        {
            valid.ValidarColor(ModelState, colores);
            if (ModelState.IsValid)
            {
                colores.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                colores.UserID = User.Identity.GetUserId();
                db.Entry(colores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(colores);
        }

        // GET: Colores/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colores colores = db.Colores.Find(id);
            if (colores == null)
            {
                return HttpNotFound();
            }
            return View(colores);
        }

        // POST: Colores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarColor(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
