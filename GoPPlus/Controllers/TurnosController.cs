using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.Classes;
using Microsoft.AspNet.Identity;
using GoPS.ViewModels;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class TurnosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Turnos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var turnos = db.Turnos.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(d => d.Afiliados);
            TurnosViewModel turnosViewModel = new TurnosViewModel(turnos.ToList());
            return View(turnosViewModel);
        }

        // GET: Turnos/Details/5
         
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turnos turnos = db.Turnos.Find(id);
            if (turnos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            return View(turnos);
        }

        // GET: Turnos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value");
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value");
            Turnos turnos = new Turnos();
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            turnos.Habilitado = true;
            return View(turnos);
        }

        // POST: Turnos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Turno,Nombre,Habilitado,Hora_Inicio,Hora_Fin,Dias,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Turnos turnos)
        {
            valid.ValidarTurno(ModelState, turnos);
            if (ModelState.IsValid)
            {
                turnos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                turnos.UserID = User.Identity.GetUserId();
                db.Turnos.Add(turnos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", turnos.ID_Afiliado);
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Fin);
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            return View(turnos);
        }

        // GET: Turnos/Edit/5
         
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turnos turnos = db.Turnos.Find(id);
            if (turnos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Fin);
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", turnos.ID_Afiliado);
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            return View(turnos);
        }

        // POST: Turnos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Turno,Nombre,Habilitado,Hora_Inicio,Hora_Fin,Dias,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Turnos turnos)
        {
            valid.ValidarTurno(ModelState, turnos);
            if (ModelState.IsValid)
            {
                turnos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                turnos.UserID = User.Identity.GetUserId();
                db.Entry(turnos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", turnos.Hora_Fin);
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", turnos.ID_Afiliado);
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            return View(turnos);
        }

        // GET: Turnos/Delete/5
         
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turnos turnos = db.Turnos.Find(id);
            if (turnos == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            turnos.diasList = util.ObtenerDiasList(turnos.Dias, true);
            ViewBag.Eliminar = db.Conductores.Where(c => c.ID_Turno == turnos.ID_Turno).Count() == 0;
            return View(turnos);
        }

        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTurno(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerTurnos(int ID_Flota)
        {
            int ID_Afiliado = db.Flotas.Find(ID_Flota).ID_Afiliado;
            var turnos = new SelectList(db.Turnos.ToList().Where(c => c.ID_Afiliado == ID_Afiliado).OrderBy(o => o.Nombre), "ID_Turno", "Nombre");
            return Json(turnos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerTurnosPorAfiliado(int ID_Afiliado)
        {
            var turnos = new SelectList(db.Turnos.ToList().Where(c => c.ID_Afiliado == ID_Afiliado).OrderBy(o => o.Nombre), "ID_Turno", "Nombre");
            return Json(turnos, JsonRequestBehavior.AllowGet);
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
