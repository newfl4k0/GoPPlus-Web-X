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
    public class TiposSancionesController : _GeneralController 
    {
        // GET: TiposSanciones
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposSanciones.ToList());
        }

        // GET: TiposSanciones/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposSanciones tiposSanciones = db.TiposSanciones.Find(id);
            if (tiposSanciones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposSanciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tiposSanciones);
        }

        // GET: TiposSanciones/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposSanciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoSancion,Nombre,Horas_Penalizacion,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposSanciones tiposSanciones)
        {
            valid.ValidarTipoSancion(ModelState, tiposSanciones);
            if (ModelState.IsValid)
            {
                tiposSanciones.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposSanciones.UserID = User.Identity.GetUserId();
                db.TiposSanciones.Add(tiposSanciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposSanciones);
        }

        // GET: TiposSanciones/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposSanciones tiposSanciones = db.TiposSanciones.Find(id);
            if (tiposSanciones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposSanciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tiposSanciones);
        }

        // POST: TiposSanciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoSancion,Nombre,Horas_Penalizacion,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposSanciones tiposSanciones)
        {
            valid.ValidarTipoSancion(ModelState, tiposSanciones);
            if (ModelState.IsValid)
            {
                tiposSanciones.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposSanciones.UserID = User.Identity.GetUserId();
                db.Entry(tiposSanciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposSanciones);
        }

        // GET: TiposSanciones/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposSanciones tiposSanciones = db.TiposSanciones.Find(id);
            if (tiposSanciones == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTiposSanciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(tiposSanciones);
        }

        // POST: TiposSanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoSancion(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerHoraSancionPorTipo(int ID_TipoSancion)
        {
            var horas = db.TiposSanciones.Find(ID_TipoSancion).Horas_Penalizacion;
            return Json(horas, JsonRequestBehavior.AllowGet);
        }
    }
}
