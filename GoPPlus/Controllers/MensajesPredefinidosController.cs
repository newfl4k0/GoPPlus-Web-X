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
    public class MensajesPredefinidosController : _GeneralController 
    {
        // GET: MensajesPredefinidos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.MensajesPredefinidos.ToList());
        }

        // GET: MensajesPredefinidos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajesPredefinidos mensajesPredefinidos = db.MensajesPredefinidos.Find(id);
            if (mensajesPredefinidos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatMensajes";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(mensajesPredefinidos);
        }

        // GET: MensajesPredefinidos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MensajesPredefinidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_MensajePredefinido,Texto,Fecha_Creacion,Fecha_Actualizacion,UserID")] MensajesPredefinidos mensajesPredefinidos)
        {
            valid.ValidarMensajePredefinido(ModelState, mensajesPredefinidos);
            if (ModelState.IsValid)
            {
                mensajesPredefinidos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                mensajesPredefinidos.UserID = User.Identity.GetUserId();
                db.MensajesPredefinidos.Add(mensajesPredefinidos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mensajesPredefinidos);
        }

        // GET: MensajesPredefinidos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajesPredefinidos mensajesPredefinidos = db.MensajesPredefinidos.Find(id);
            if (mensajesPredefinidos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatMensajes";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(mensajesPredefinidos);
        }

        // POST: MensajesPredefinidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_MensajePredefinido,Texto,Fecha_Creacion,Fecha_Actualizacion,UserID")] MensajesPredefinidos mensajesPredefinidos)
        {
            valid.ValidarMensajePredefinido(ModelState, mensajesPredefinidos);
            if (ModelState.IsValid)
            {
                mensajesPredefinidos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                mensajesPredefinidos.UserID = User.Identity.GetUserId();
                db.Entry(mensajesPredefinidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mensajesPredefinidos);
        }

        // GET: MensajesPredefinidos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajesPredefinidos mensajesPredefinidos = db.MensajesPredefinidos.Find(id);
            if (mensajesPredefinidos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatMensajes";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(mensajesPredefinidos);
        }

        // POST: MensajesPredefinidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarMensajePredefinido(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
