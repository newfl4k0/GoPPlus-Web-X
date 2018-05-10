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
using GoPS.CustomFilters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]    
    [ValidateInput(false)]
    public class PuntosInteresController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: PuntosInteres
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var puntosInteres = db.PuntosInteres.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(p => p.Afiliados).Include(p => p.Calles).Include(p => p.TiposPuntosInteres);
            return View(puntosInteres.ToList());
        }

        // GET: PuntosInteres/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntosInteres puntosInteres = db.PuntosInteres.Find(id);
            if (puntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(puntosInteres);
        }

        // GET: PuntosInteres/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaSelectList();
            ViewBag.ID_TipoPuntoInteres = new SelectList(db.TiposPuntosInteres.OrderBy(o => o.Nombre), "ID_TipoPuntoInteres", "Nombre");
            return View(new PuntosInteres() { Habilitado = true });
        }

        // POST: PuntosInteres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_PuntoInteres,Nombre,Habilitado,ID_Calle,Numero,Latitud,Longitud,ID_TipoPuntoInteres,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] PuntosInteres puntosInteres)
        {
            valid.ValidarPuntoInteres(ModelState, puntosInteres);
            if (ModelState.IsValid)
            {
                puntosInteres.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                puntosInteres.UserID = User.Identity.GetUserId();
                db.PuntosInteres.Add(puntosInteres);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", puntosInteres.ID_Afiliado);
            ObtenerGeografiaSelectList(puntosInteres);
            ViewBag.ID_TipoPuntoInteres = new SelectList(db.TiposPuntosInteres.OrderBy(o => o.Nombre), "ID_TipoPuntoInteres", "Nombre", puntosInteres.ID_TipoPuntoInteres);
            return View(puntosInteres);
        }

        // GET: PuntosInteres/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntosInteres puntosInteres = db.PuntosInteres.Find(id);
            if (puntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", puntosInteres.ID_Afiliado);
            ObtenerGeografiaSelectList(puntosInteres);
            ViewBag.ID_TipoPuntoInteres = new SelectList(db.TiposPuntosInteres.OrderBy(o => o.Nombre), "ID_TipoPuntoInteres", "Nombre", puntosInteres.ID_TipoPuntoInteres);
            return View(puntosInteres);
        }

        // POST: PuntosInteres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_PuntoInteres,Nombre,Habilitado,ID_Calle,Numero,Latitud,Longitud,ID_TipoPuntoInteres,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] PuntosInteres puntosInteres)
        {
            valid.ValidarPuntoInteres(ModelState, puntosInteres);
            if (ModelState.IsValid)
            {
                puntosInteres.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                puntosInteres.UserID = User.Identity.GetUserId();
                db.Entry(puntosInteres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", puntosInteres.ID_Afiliado);
            ObtenerGeografiaSelectList(puntosInteres);
            ViewBag.ID_TipoPuntoInteres = new SelectList(db.TiposPuntosInteres.OrderBy(o => o.Nombre), "ID_TipoPuntoInteres", "Nombre", puntosInteres.ID_TipoPuntoInteres);
            return View(puntosInteres);
        }

        // GET: PuntosInteres/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntosInteres puntosInteres = db.PuntosInteres.Find(id);
            if (puntosInteres == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(puntosInteres);
        }

        // POST: PuntosInteres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarPuntoInteres(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        private void ObtenerGeografiaSelectList(PuntosInteres puntosInteres)
        {
            if (puntosInteres.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(puntosInteres.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", puntosInteres.ID_Calle);
            }
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
