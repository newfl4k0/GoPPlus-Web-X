using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;  
using Microsoft.AspNet.Identity;
using GoPS.Classes;
using GoPS.CustomFilters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [ValidateInput(false)]
    public class RutasController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Rutas
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var rutas = db.Rutas.Where(o => ID_Afiliados.Contains(o.UsuariosAbonados.ClientesAbonados.ID_Afiliado)).Include(r => r.Calles).Include(r => r.Calles1);
            return View(rutas.ToList());
        }

        // GET: Rutas/Details/5
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(rutas);
        }

        // GET: Rutas/Create
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create()
        {
            ObtenerUsuarioClienteSelectList();
            ObtenerGeografiaOrigenSelectList();
            ObtenerGeografiaDestinoSelectList();
            return View(new Rutas() { Habilitado = true });
        }

        private void ObtenerUsuarioClienteSelectList()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre");
            ViewBag.ID_UsuarioAbonado = new SelectList(db.UsuariosAbonados.OrderBy(o => o.Nombre).Take(0), "ID_UsuarioAbonado", "Nombre");
        }

        private void ObtenerGeografiaOrigenSelectList()
        {
            ViewBag.ID_Estado_Origen = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad_Origen = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia_Origen = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle_Origen = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        private void ObtenerGeografiaDestinoSelectList()
        {
            ViewBag.ID_Estado_Destino = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad_Destino = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia_Destino = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle_Destino = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: Rutas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Ruta,Nombre,Habilitado,Precio,ID_Calle_Origen,Numero_Exterior_Origen,Numero_Interior_Origen,ID_Calle_Destino,Numero_Exterior_Destino,Numero_Interior_Destino,ID_UsuarioAbonado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Rutas rutas)
        {
            valid.ValidarRuta(ModelState, rutas);
            if (ModelState.IsValid)
            {
                rutas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                rutas.UserID = User.Identity.GetUserId();
                db.Rutas.Add(rutas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerUsuarioClienteSelectList(rutas);
            ObtenerGeografiaSelectList(rutas);
            return View(rutas);
        }

        // GET: Rutas/Edit/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ObtenerUsuarioClienteSelectList(rutas);
            ObtenerGeografiaSelectList(rutas);
            return View(rutas);
        }

        // POST: Rutas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Ruta,Nombre,Habilitado,Precio,ID_Calle_Origen,Numero_Exterior_Origen,Numero_Interior_Origen,ID_Calle_Destino,Numero_Exterior_Destino,Numero_Interior_Destino,ID_UsuarioAbonado,Fecha_Creacion,Fecha_Actualizacion,UserID")] Rutas rutas)
        {
            valid.ValidarRuta(ModelState, rutas);
            if (ModelState.IsValid)
            {
                rutas.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                rutas.UserID = User.Identity.GetUserId();
                db.Entry(rutas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerUsuarioClienteSelectList(rutas);
            ObtenerGeografiaSelectList(rutas);
            return View(rutas);
        }

        // GET: Rutas/Delete/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(rutas);
        }

        // POST: Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarRuta(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerUsuarioClienteSelectList(Rutas rutas)
        {
            if (rutas.ID_UsuarioAbonado == 0)
                ObtenerUsuarioClienteSelectList();
            else
            {
                List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
                ClientesAbonados cliente_abonado = db.UsuariosAbonados.Find(rutas.ID_UsuarioAbonado).ClientesAbonados;
                ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre", cliente_abonado.ID_ClienteAbonado);
                ViewBag.ID_UsuarioAbonado = new SelectList(cliente_abonado.UsuariosAbonados.OrderBy(o => o.Nombre), "ID_UsuarioAbonado", "Nombre", rutas.ID_UsuarioAbonado);
            }
        }

        private void ObtenerGeografiaSelectList(Rutas rutas)
        {
            if (rutas.ID_Calle_Origen == 0)
                ObtenerGeografiaOrigenSelectList();
            else
            {
                Colonias colonia_origen = db.Calles.Find(rutas.ID_Calle_Origen).Colonias;
                Ciudades ciudad_origen = colonia_origen.Ciudades;
                Estados estado_origen = ciudad_origen.Estados;
                ViewBag.ID_Estado_Origen = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_origen.ID_Estado);
                ViewBag.ID_Ciudad_Origen = new SelectList(estado_origen.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_origen.ID_Ciudad);
                ViewBag.ID_Colonia_Origen = new SelectList(ciudad_origen.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_origen.ID_Colonia);
                ViewBag.ID_Calle_Origen = new SelectList(colonia_origen.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", rutas.ID_Calle_Origen);
            }
            if (rutas.ID_Calle_Destino == 0)
                ObtenerGeografiaDestinoSelectList();
            else
            {
                Colonias colonia_destino = db.Calles.Find(rutas.ID_Calle_Destino).Colonias;
                Ciudades ciudad_destino = colonia_destino.Ciudades;
                Estados estado_destino = ciudad_destino.Estados;
                ViewBag.ID_Estado_Destino = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_destino.ID_Estado);
                ViewBag.ID_Ciudad_Destino = new SelectList(estado_destino.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_destino.ID_Ciudad);
                ViewBag.ID_Colonia_Destino = new SelectList(ciudad_destino.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_destino.ID_Colonia);
                ViewBag.ID_Calle_Destino = new SelectList(colonia_destino.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", rutas.ID_Calle_Destino);
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
