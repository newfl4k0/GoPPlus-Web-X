using GoPS.Classes;
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

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ClientesHabitualesController : _GeneralController 
    {
        // GET: ClientesHabituales
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var clientesHabituales = db.ClientesHabituales.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(c => c.Calles).Include(c => c.TiposAvisos).Include(d => d.Afiliados);
            return View(clientesHabituales.ToList());
        }

        // GET: ClientesHabituales/Details/5
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesHabituales clientesHabituales = db.ClientesHabituales.Find(id);
            if (clientesHabituales == null)
            {
                return HttpNotFound();
            }
            return View(clientesHabituales);
        }

        // GET: ClientesHabituales/Create
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaSelectList();
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre");
            return View(new ClientesHabituales() { Habilitado = true });
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: ClientesHabituales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create([Bind(Include = "ID_ClienteHabitual,Nombre,Telefono,Habilitado,Servicios_Automaticos,ID_TipoAviso,ID_Calle,Numero_Exterior,Numero_Interior,Fecha_Alta,Fecha_Baja,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ClientesHabituales clientesHabituales)
        {
            valid.ValidarClienteHabitual(ModelState, clientesHabituales);
            if (ModelState.IsValid)
            {
                clientesHabituales.Fecha_Alta = util.ConvertToMexicanDate(DateTime.Now);
                clientesHabituales.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                clientesHabituales.UserID = User.Identity.GetUserId();
                db.ClientesHabituales.Add(clientesHabituales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesHabituales.ID_Afiliado);
            ObtenerGeografiaSelectList(clientesHabituales);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesHabituales.ID_TipoAviso);
            return View(clientesHabituales);
        }

        // GET: ClientesHabituales/Edit/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesHabituales clientesHabituales = db.ClientesHabituales.Find(id);
            if (clientesHabituales == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesHabituales.ID_Afiliado);
            ObtenerGeografiaSelectList(clientesHabituales);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesHabituales.ID_TipoAviso);
            return View(clientesHabituales);
        }

        // POST: ClientesHabituales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_ClienteHabitual,Nombre,Telefono,Habilitado,Servicios_Automaticos,ID_TipoAviso,ID_Calle,Numero_Exterior,Numero_Interior,Fecha_Alta,Fecha_Baja,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ClientesHabituales clientesHabituales)
        {
            valid.ValidarClienteHabitual(ModelState, clientesHabituales);
            if (ModelState.IsValid)
            {
                clientesHabituales.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                clientesHabituales.UserID = User.Identity.GetUserId();
                db.Entry(clientesHabituales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesHabituales.ID_Afiliado);
            ObtenerGeografiaSelectList(clientesHabituales);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesHabituales.ID_TipoAviso);
            return View(clientesHabituales);
        }

        // GET: ClientesHabituales/Delete/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesHabituales clientesHabituales = db.ClientesHabituales.Find(id);
            if (clientesHabituales == null)
            {
                return HttpNotFound();
            }
            return View(clientesHabituales);
        }

        // POST: ClientesHabituales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarClienteHabitual(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(ClientesHabituales clientesHabituales)
        {
            if (clientesHabituales.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(clientesHabituales.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", clientesHabituales.ID_Calle);
            }
        }
        
    }
}
