using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class ModelosController : _GeneralController
    {
        // GET: Modelos
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var modelos = db.Modelos.Include(m => m.Marcas);
            return View(modelos.ToList());
        }

        // GET: Modelos/Details/5
         
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelos modelos = db.Modelos.Find(id);
            if (modelos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatModelos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(modelos);
        }

        // GET: Modelos/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre");
            return View();
        }

        // POST: Modelos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Modelo,Nombre,ID_Marca,Fecha_Creacion,Fecha_Actualizacion,UserID")] Modelos modelos)
        {
            valid.ValidarModelo(ModelState, modelos);
            if (ModelState.IsValid)
            {
                modelos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                modelos.UserID = User.Identity.GetUserId();
                db.Modelos.Add(modelos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre", modelos.ID_Marca);
            return View(modelos);
        }

        // GET: Modelos/Edit/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelos modelos = db.Modelos.Find(id);
            if (modelos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatModelos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre", modelos.ID_Marca);
            return View(modelos);
        }

        // POST: Modelos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Modelo,Nombre,ID_Marca,Fecha_Creacion,Fecha_Actualizacion,UserID")] Modelos modelos)
        {
            valid.ValidarModelo(ModelState, modelos);
            if (ModelState.IsValid)
            {
                modelos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                modelos.UserID = User.Identity.GetUserId();
                db.Entry(modelos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Marca = new SelectList(db.Marcas.OrderBy(o => o.Nombre), "ID_Marca", "Nombre", modelos.ID_Marca);
            return View(modelos);
        }

        // GET: Modelos/Delete/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelos modelos = db.Modelos.Find(id);
            if (modelos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatModelos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(modelos);
        }

        // POST: Modelos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarModelo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerModelos(int ID_Marca)
        {
            var modelos = new SelectList(db.Modelos.Where(c => c.ID_Marca == ID_Marca).OrderBy(o => o.Nombre), "ID_Modelo", "Nombre");
            return Json(modelos, JsonRequestBehavior.AllowGet);
        }
        
    }
}