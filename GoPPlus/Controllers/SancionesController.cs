using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class SancionesController : _GeneralController 
    {
        // GET: Sanciones
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            if (TempData["Baja"] != null)
            {
                ViewBag.Baja = true;
                TempData.Remove("Baja");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            //var sanciones = db.Sanciones.Where(o => ID_Afiliados.Contains(o.Conductores.Flotas.ID_Afiliado)).Include(s => s.Conductores).Include(s => s.Operadores).Include(s => s.Operadores1).Include(s => s.TiposSanciones);
            //var sanciones = db.Sanciones;
            //var sanciones = db.Sanciones.Where(o => ID_Afiliados.Contains(o.Conductores.Flotas.ID_Afiliado));
            var sanciones = db.Sanciones.Include(s => s.Operadores).Include(s => s.Operadores1).Include(s => s.TiposSanciones);
            sanciones = sanciones.OrderByDescending(fec => fec.Fecha_Inicio);
            return View(sanciones.ToList());
        }

        // GET: Sanciones/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanciones sanciones = db.Sanciones.Find(id);
            if (sanciones == null)
            {
                return HttpNotFound();
            }
            return View(sanciones);
        }

        // GET: Sanciones/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            ObtenerConductorFlotaSelectList();
            ViewBag.ID_TipoSancion = new SelectList(db.TiposSanciones.OrderBy(o => o.Nombre), "ID_TipoSancion", "Nombre");
            return View();
        }

        private void ObtenerConductorFlotaSelectList()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre");
            ViewBag.ID_Conductor = new SelectList(db.Conductores.ToList().OrderBy(o => o.NombreCompleto).Take(0), "ID_Conductor", "NombreCompleto");
        }

        // POST: Sanciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Sancion,Fecha_Inicio,Fecha_Fin,ID_Operador_Alta,ID_Operador_Baja,Observaciones,ID_TipoSancion,ID_Conductor,Fecha_Creacion,Fecha_Actualizacion,UserID")] Sanciones sanciones)
        {
            valid.ValidarSancion(ModelState, sanciones);
            if (ModelState.IsValid)
            {
                sanciones.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                sanciones.UserID = User.Identity.GetUserId();
                sanciones.ID_Operador_Alta = db.Conductores.Find(sanciones.ID_Conductor).Flotas.Afiliados.Operadores.FirstOrDefault().ID_Operador;
                db.Sanciones.Add(sanciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ObtenerConductorFlotaSelectList(sanciones);
            ViewBag.ID_TipoSancion = new SelectList(db.TiposSanciones.OrderBy(o => o.Nombre), "ID_TipoSancion", "Nombre", sanciones.ID_TipoSancion);
            return View(sanciones);
        }

        // GET: Sanciones/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanciones sanciones = db.Sanciones.Find(id);
            if (sanciones == null)
            {
                return HttpNotFound();
            }
            ObtenerConductorFlotaSelectList(sanciones);
            //List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            //ViewBag.ID_Operador_Alta = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", sanciones.ID_Operador_Alta);
            //ViewBag.ID_Operador_Baja = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", sanciones.ID_Operador_Baja);
            ViewBag.ID_TipoSancion = new SelectList(db.TiposSanciones.OrderBy(o => o.Nombre), "ID_TipoSancion", "Nombre", sanciones.ID_TipoSancion);
            return View(sanciones);
        }

        // POST: Sanciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Sancion,Fecha_Inicio,Fecha_Fin,ID_Operador_Alta,ID_Operador_Baja,Observaciones,ID_TipoSancion,ID_Conductor,Fecha_Creacion,Fecha_Actualizacion,UserID")] Sanciones sanciones)
        {
            valid.ValidarSancion(ModelState, sanciones);
            if (ModelState.IsValid)
            {
                sanciones.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                sanciones.UserID = User.Identity.GetUserId();
                db.Entry(sanciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerConductorFlotaSelectList(sanciones);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            //ViewBag.ID_Operador_Alta = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", sanciones.ID_Operador_Alta);
            //ViewBag.ID_Operador_Baja = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", sanciones.ID_Operador_Baja);
            ViewBag.ID_TipoSancion = new SelectList(db.TiposSanciones.OrderBy(o => o.Nombre), "ID_TipoSancion", "Nombre", sanciones.ID_TipoSancion);
            return View(sanciones);
        }

        // GET: Sanciones/Cancel/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanciones sanciones = db.Sanciones.Find(id);
            if (sanciones == null)
            {
                return HttpNotFound();
            }
            return View(sanciones);
        }

        // POST: Sanciones/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult CancelConfirmed(int id)
        {
            Sanciones sanciones = db.Sanciones.Find(id);
            db.EliminarSancion_Baja(id);
            sanciones.ID_Operador_Baja = db.Conductores.Find(sanciones.ID_Conductor).Flotas.Afiliados.Operadores.FirstOrDefault().ID_Operador;
            sanciones.Fecha_Fin = util.ConvertToMexicanDate(DateTime.Now);
            db.Entry(sanciones).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Baja"] = true;
            return RedirectToAction("Index");
        }

        // GET: Sanciones/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanciones sanciones = db.Sanciones.Find(id);
            if (sanciones == null)
            {
                return HttpNotFound();
            }
            return View(sanciones);
        }

        // POST: Sanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarSancion(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerConductorFlotaSelectList(Sanciones sanciones)
        {
            if (sanciones.ID_Conductor == 0)
                ObtenerConductorFlotaSelectList();
            else
            {
                List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
                Flotas flotas = db.Conductores.Find(sanciones.ID_Conductor).Flotas;
                ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", flotas.ID_Flota);
                ViewBag.ID_Conductor = new SelectList(flotas.Conductores.ToList().OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto", sanciones.ID_Conductor);
            }
        }

        public JsonResult ObtenerHoraFinSancion(int ID_TipoSancion, string Fecha_Inicio)
        {
            DateTime fecha_ini = Convert.ToDateTime(Fecha_Inicio);
            var horas = db.TiposSanciones.Find(ID_TipoSancion).Horas_Penalizacion;
            DateTime fecha_fin = fecha_ini.AddHours(horas);
            string Fecha_Final = fecha_fin.ToString("dd/MM/yyyy HH:mm");
            return Json(Fecha_Final, JsonRequestBehavior.AllowGet);
        }
    }
}
