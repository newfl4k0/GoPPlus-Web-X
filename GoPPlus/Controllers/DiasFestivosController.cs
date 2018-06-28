using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
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
    public class DiasFestivosController : _GeneralController
    {
        // GET: DiasFestivos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.DiasFestivos.ToList());
        }

        // GET: DiasFestivos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiasFestivos diasFestivos = db.DiasFestivos.Find(id);
            if (diasFestivos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDiasFestivos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(diasFestivos);
        }

        // GET: DiasFestivos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiasFestivos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_DiaFestivo,Nombre,Fecha,Fecha_Creacion,Fecha_Actualizacion,UserID")] DiasFestivos diasFestivos)
        {
            valid.ValidarDiaFestivo(ModelState, diasFestivos);
            if (ModelState.IsValid)
            {
                diasFestivos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                diasFestivos.UserID = User.Identity.GetUserId();
                db.DiasFestivos.Add(diasFestivos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diasFestivos);
        }

        // GET: DiasFestivos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiasFestivos diasFestivos = db.DiasFestivos.Find(id);
            if (diasFestivos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDiasFestivos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(diasFestivos);
        }

        // POST: DiasFestivos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_DiaFestivo,Nombre,Fecha,Fecha_Creacion,Fecha_Actualizacion,UserID")] DiasFestivos diasFestivos)
        {
            valid.ValidarDiaFestivo(ModelState, diasFestivos);
            if (ModelState.IsValid)
            {
                diasFestivos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                diasFestivos.UserID = User.Identity.GetUserId();
                db.Entry(diasFestivos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diasFestivos);
        }

        // GET: DiasFestivos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiasFestivos diasFestivos = db.DiasFestivos.Find(id);
            if (diasFestivos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDiasFestivos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(diasFestivos);
        }

        // POST: DiasFestivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarDiaFestivo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
