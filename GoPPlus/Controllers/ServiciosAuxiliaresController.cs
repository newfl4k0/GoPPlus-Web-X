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
    public class ServiciosAuxiliaresController : _GeneralController 
    {
        // GET: ServiciosAuxiliares
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var serviciosAuxiliares = db.ServiciosAuxiliares.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(d => d.Afiliados);
            return View(serviciosAuxiliares.ToList());
        }

        // GET: ServiciosAuxiliares/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAuxiliares serviciosAuxiliares = db.ServiciosAuxiliares.Find(id);
            if (serviciosAuxiliares == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatServAux";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(serviciosAuxiliares);
        }

        // GET: ServiciosAuxiliares/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View();
        }

        // POST: ServiciosAuxiliares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_ServicioAuxiliar,Nombre,Telefono,EsCelular,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ServiciosAuxiliares serviciosAuxiliares)
        {
            valid.ValidarServicioAuxiliar(ModelState, serviciosAuxiliares);
            if (ModelState.IsValid)
            {
                serviciosAuxiliares.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                serviciosAuxiliares.UserID = User.Identity.GetUserId();
                db.ServiciosAuxiliares.Add(serviciosAuxiliares);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", serviciosAuxiliares.ID_Afiliado);
            return View(serviciosAuxiliares);
        }

        // GET: ServiciosAuxiliares/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAuxiliares serviciosAuxiliares = db.ServiciosAuxiliares.Find(id);
            if (serviciosAuxiliares == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatServAux";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", serviciosAuxiliares.ID_Afiliado);
            return View(serviciosAuxiliares);
        }

        // POST: ServiciosAuxiliares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_ServicioAuxiliar,Nombre,Telefono,EsCelular,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] ServiciosAuxiliares serviciosAuxiliares)
        {
            valid.ValidarServicioAuxiliar(ModelState, serviciosAuxiliares);
            if (ModelState.IsValid)
            {
                serviciosAuxiliares.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                serviciosAuxiliares.UserID = User.Identity.GetUserId();
                db.Entry(serviciosAuxiliares).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", serviciosAuxiliares.ID_Afiliado);
            return View(serviciosAuxiliares);
        }

        // GET: ServiciosAuxiliares/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAuxiliares serviciosAuxiliares = db.ServiciosAuxiliares.Find(id);
            if (serviciosAuxiliares == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatServAux";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(serviciosAuxiliares);
        }

        // POST: ServiciosAuxiliares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarServicioAuxiliar(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        
    }
}
