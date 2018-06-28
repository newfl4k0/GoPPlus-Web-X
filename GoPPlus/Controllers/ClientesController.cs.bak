using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ClientesController : _GeneralController 
    {
        
        // GET: Clientes
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var vwC = db.vwClientes.ToList();
            //var clientes = db.Clientes.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(c => c.Calles).Include(c => c.Calles1).Include(d => d.Afiliados);
            return View(vwC);
        }

        // GET: Clientes/Details/5
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaOrigenSelectList();
            ObtenerGeografiaDestinoSelectList();
            return View();
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

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Cliente,Nombre,ID_Calle_Origen,Numero_Exterior_Origen,Numero_Interior_Origen,ID_Calle_Destino,Numero_Exterior_Destino,Numero_Interior_Destino,ID_Afiliado,Fecha_Creacion")] Clientes clientes)
        {
            valid.ValidarCliente(ModelState, clientes);
            if (ModelState.IsValid)
            {
                clientes.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                db.Clientes.Add(clientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientes.ID_Afiliado);
            ObtenerGeografiaSelectList(clientes);
            return View(clientes);
        }

        // GET: Clientes/Edit/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientes.ID_Afiliado);
            ObtenerGeografiaSelectList(clientes);
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Cliente,Nombre,ID_Calle_Origen,Numero_Exterior_Origen,Numero_Interior_Origen,ID_Calle_Destino,Numero_Exterior_Destino,Numero_Interior_Destino,ID_Afiliado,Fecha_Creacion")] Clientes clientes)
        {
            valid.ValidarCliente(ModelState, clientes);
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientes.ID_Afiliado);
            ObtenerGeografiaSelectList(clientes);
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarCliente(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(Clientes clientes)
        {
            if (clientes.ID_Calle_Origen == 0)
                ObtenerGeografiaOrigenSelectList();
            else
            {
                Colonias colonia_origen = db.Calles.Find(clientes.ID_Calle_Origen).Colonias;
                Ciudades ciudad_origen = colonia_origen.Ciudades;
                Estados estado_origen = ciudad_origen.Estados;
                ViewBag.ID_Estado_Origen = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_origen.ID_Estado);
                ViewBag.ID_Ciudad_Origen = new SelectList(estado_origen.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_origen.ID_Ciudad);
                ViewBag.ID_Colonia_Origen = new SelectList(ciudad_origen.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_origen.ID_Colonia);
                ViewBag.ID_Calle_Origen = new SelectList(colonia_origen.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", clientes.ID_Calle_Origen);
            }
            if (clientes.ID_Calle_Destino == 0)
                ObtenerGeografiaDestinoSelectList();
            else
            { 
                Colonias colonia_destino = db.Calles.Find(clientes.ID_Calle_Destino).Colonias;
                Ciudades ciudad_destino = colonia_destino.Ciudades;
                Estados estado_destino = ciudad_destino.Estados;
                ViewBag.ID_Estado_Destino = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado_destino.ID_Estado);
                ViewBag.ID_Ciudad_Destino = new SelectList(estado_destino.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad_destino.ID_Ciudad);
                ViewBag.ID_Colonia_Destino = new SelectList(ciudad_destino.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia_destino.ID_Colonia);
                ViewBag.ID_Calle_Destino = new SelectList(colonia_destino.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", clientes.ID_Calle_Destino);
            }
        }
        
    }
}
