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
    public class EstatusReservaController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: EstatusReserva
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Estatus_Reserva.ToList());
        }

        // GET: EstatusReserva/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus_Reserva estatus_Reserva = db.Estatus_Reserva.Find(id);
            if (estatus_Reserva == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(estatus_Reserva);
        }

        // GET: EstatusReserva/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstatusReserva/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Estatus_Reserva,Nombre,Descripcion,Orden,Fecha_Creacion,Fecha_Actualizacion,UserID")] Estatus_Reserva estatus_Reserva)
        {
            valid.ValidarEstatusReservas(ModelState, estatus_Reserva);
            if (ModelState.IsValid)
            {
                estatus_Reserva.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                estatus_Reserva.UserID = User.Identity.GetUserId();
                db.Estatus_Reserva.Add(estatus_Reserva);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estatus_Reserva);
        }

        // GET: EstatusReserva/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus_Reserva estatus_Reserva = db.Estatus_Reserva.Find(id);
            if (estatus_Reserva == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(estatus_Reserva);
        }

        // POST: EstatusReserva/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Estatus_Reserva,Nombre,Descripcion,Orden,Fecha_Creacion,Fecha_Actualizacion,UserID")] Estatus_Reserva estatus_Reserva)
        {
            valid.ValidarEstatusReservas(ModelState, estatus_Reserva);
            if (ModelState.IsValid)
            {
                estatus_Reserva.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                estatus_Reserva.UserID = User.Identity.GetUserId();
                db.Entry(estatus_Reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estatus_Reserva);
        }

        // GET: EstatusReserva/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus_Reserva estatus_Reserva = db.Estatus_Reserva.Find(id);
            if (estatus_Reserva == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(estatus_Reserva);
        }

        // POST: EstatusReserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarEstatusReserva(id);
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
