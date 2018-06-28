using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using GoPS.ViewModels;
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
    public class ClientesAbonadosController : _GeneralController 
    {
        
        // GET: ClientesAbonados
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
                                                                               {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var clientesAbonados = db.ClientesAbonados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(c => c.Bancos).Include(c => c.Calles).Include(c => c.FormasPago).Include(c => c.TiposAvisos).Include(d => d.Afiliados);
            ClientesAbonadosViewModel clientesAbonadosViewModel = new ClientesAbonadosViewModel(clientesAbonados.ToList());
            return View(clientesAbonadosViewModel);
        }

        // GET: ClientesAbonados/Details/5
        [HasPermission("Clientes_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesAbonados clientesAbonados = db.ClientesAbonados.Find(id);
            if (clientesAbonados == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatClienteAbonado";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(clientesAbonados);
        }

        // GET: ClientesAbonados/Create
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre");
            ObtenerGeografiaSelectList();
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre");
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre");
            ViewBag.Dia_Pago = new SelectList(util.ObtenerDias(), "Key", "Value");
            return View(new ClientesAbonados() { Habilitado = true });
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: ClientesAbonados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Create([Bind(Include = "ID_ClienteAbonado,Nombre,Telefono,Telefono_EsCelular,Limite_Credito,Email,Clave_Automatica,Observaciones,Habilitado,Reportes,Servicios_Automaticos,Fecha_Alta,Fecha_Baja,ID_Calle,Numero_Exterior,Numero_Interior,CP,RFC,Dia_Pago,ID_Banco,Clabe,ID_TipoAviso,ID_FormaPago,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ClientesAbonados clientesAbonados)
        {
            valid.ValidarClienteAbonado(ModelState, clientesAbonados);
            if (ModelState.IsValid)
            {
                clientesAbonados.Fecha_Alta = util.ConvertToMexicanDate(DateTime.Now);
                clientesAbonados.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                clientesAbonados.UserID = User.Identity.GetUserId();
                db.ClientesAbonados.Add(clientesAbonados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dia_Pago = new SelectList(util.ObtenerDias(), "Key", "Value", clientesAbonados.Dia_Pago);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesAbonados.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", clientesAbonados.ID_Banco);
            ObtenerGeografiaSelectList(clientesAbonados);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", clientesAbonados.ID_FormaPago);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesAbonados.ID_TipoAviso);
            return View(clientesAbonados);
        }

        // GET: ClientesAbonados/Edit/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesAbonados clientesAbonados = db.ClientesAbonados.Find(id);
            if (clientesAbonados == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatClienteAbonado";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Dia_Pago = new SelectList(util.ObtenerDias(), "Key", "Value", clientesAbonados.Dia_Pago);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesAbonados.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", clientesAbonados.ID_Banco);
            ObtenerGeografiaSelectList(clientesAbonados);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", clientesAbonados.ID_FormaPago);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesAbonados.ID_TipoAviso);
            return View(clientesAbonados);
        }

        // POST: ClientesAbonados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_ClienteAbonado,Nombre,Telefono,Telefono_EsCelular,Limite_Credito,Email,Clave_Automatica,Observaciones,Habilitado,Reportes,Servicios_Automaticos,Fecha_Alta,Fecha_Baja,ID_Calle,Numero_Exterior,Numero_Interior,CP,RFC,Dia_Pago,ID_Banco,Clabe,ID_TipoAviso,ID_FormaPago,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ClientesAbonados clientesAbonados)
        {
            valid.ValidarClienteAbonado(ModelState, clientesAbonados);
            if (ModelState.IsValid)
            {
                clientesAbonados.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                clientesAbonados.UserID = User.Identity.GetUserId();
                db.Entry(clientesAbonados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dia_Pago = new SelectList(util.ObtenerDias(), "Key", "Value", clientesAbonados.Dia_Pago);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", clientesAbonados.ID_Afiliado);
            ViewBag.ID_Banco = new SelectList(db.Bancos.OrderBy(o => o.Nombre), "ID_Banco", "Nombre", clientesAbonados.ID_Banco);
            ObtenerGeografiaSelectList(clientesAbonados);
            ViewBag.ID_FormaPago = new SelectList(db.FormasPago.OrderBy(o => o.Nombre), "ID_FormaPago", "Nombre", clientesAbonados.ID_FormaPago);
            ViewBag.ID_TipoAviso = new SelectList(db.TiposAvisos.OrderBy(o => o.Nombre), "ID_TipoAviso", "Nombre", clientesAbonados.ID_TipoAviso);
            return View(clientesAbonados);
        }

        // GET: ClientesAbonados/Delete/5
        [HasPermission("Clientes_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientesAbonados clientesAbonados = db.ClientesAbonados.Find(id);
            if (clientesAbonados == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatClienteAbonado";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(clientesAbonados);
        }

        // POST: ClientesAbonados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Clientes_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarClienteAbonado(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(ClientesAbonados clientesAbonados)
        {
            if (clientesAbonados.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(clientesAbonados.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", clientesAbonados.ID_Calle);
            }
        }
        
    }
}
