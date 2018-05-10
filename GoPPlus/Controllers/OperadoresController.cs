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

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public class OperadoresController : Controller
    {
        //private Entities ent = new Entities();
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Operadores
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var operadores = db.Operadores.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(o => o.Afiliados).Include(o => o.Calles).Include(o => o.Turnos);
            OperadoresViewModel operadoresViewModel = new OperadoresViewModel(operadores.ToList());
            return View(operadoresViewModel);
        }

        // GET: Operadores/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadores operadores = db.Operadores.Find(id);
            if (operadores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(operadores);
        }

        // GET: Operadores/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaSelectList();
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre");
            ViewBag.UserID_Operador = new SelectList(db.AspNetUsers.Where(u => !db.Operadores.Select(c => c.UserID_Operador).Contains(u.Id)).OrderBy(o => o.UserName), "Id", "UserName");
            return View(new Operadores() { Habilitado = true });
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: Operadores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Operador,Nombre,ID_Calle,Numero_Exterior,Numero_Interior,Codigo_Postal,Telefono,RFC,Habilitado,Objetivo_Mensual,ID_Afiliado,ID_Turno,ID_NivelPermiso,Fecha_Creacion,Fecha_Actualizacion,UserID,UserID_Operador")] Operadores operadores)
        {
            valid.ValidarOperador(ModelState, operadores);
            if (ModelState.IsValid)
            {
                operadores.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                operadores.UserID = User.Identity.GetUserId();
                db.Operadores.Add(operadores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", operadores.ID_Afiliado);
            ObtenerGeografiaSelectList(operadores);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", operadores.ID_Turno);
            ViewBag.UserID_Operador = new SelectList(db.AspNetUsers.Where(u => !db.Operadores.Select(c => c.UserID_Operador).Contains(u.Id)).OrderBy(o => o.UserName), "Id", "UserName", operadores.UserID_Operador);
            return View(operadores);
        }

        // GET: Operadores/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadores operadores = db.Operadores.Find(id);
            if (operadores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", operadores.ID_Afiliado);
            ObtenerGeografiaSelectList(operadores);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", operadores.ID_Turno);
            ViewBag.UserID_Operador = new SelectList(db.AspNetUsers.Where(u => !db.Operadores.Select(c => c.UserID_Operador).Contains(u.Id)).OrderBy(o => o.UserName), "Id", "UserName", operadores.UserID_Operador);
            return View(operadores);
        }

        // POST: Operadores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Operador,Nombre,ID_Calle,Numero_Exterior,Numero_Interior,Codigo_Postal,Telefono,RFC,Habilitado,Objetivo_Mensual,ID_Afiliado,ID_Turno,ID_NivelPermiso,Fecha_Creacion,Fecha_Actualizacion,UserID,UserID_Operador")] Operadores operadores)
        {
            valid.ValidarOperador(ModelState, operadores);
            if (ModelState.IsValid)
            {
                operadores.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                operadores.UserID = User.Identity.GetUserId();
                db.Entry(operadores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).ToList().OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", operadores.ID_Afiliado);
            ObtenerGeografiaSelectList(operadores);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", operadores.ID_Turno);
            ViewBag.UserID_Operador = new SelectList(db.AspNetUsers.Where(u => !db.Operadores.Select(c => c.UserID_Operador).Contains(u.Id)).OrderBy(o => o.UserName), "Id", "UserName", operadores.UserID_Operador);
            return View(operadores);
        }

        // GET: Operadores/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadores operadores = db.Operadores.Find(id);
            if (operadores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(operadores);
        }

        // POST: Operadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarOperador(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(Operadores operadores)
        {
            if (operadores.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(operadores.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", operadores.ID_Calle);
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
