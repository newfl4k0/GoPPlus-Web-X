using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Filters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class EstatusController : _GeneralController 
    {
        // GET: Estatus
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Estatus.ToList());
        }

        // GET: Estatus/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus estatus = db.Estatus.Find(id);
            if (estatus == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View(estatus);
        }

        // GET: Estatus/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", ID_Empresa);
            return View();
        }

        // POST: Estatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Estatus,Nombre,Imagen,Peso_Imagen,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID")] Estatus estatus, HttpPostedFileBase File_Imagen)
        {
            GetTempFileNames(estatus, File_Imagen);
            valid.ValidarEstatus(ModelState, estatus);
            if (ModelState.IsValid)
            {
                GetFileNames(estatus, File_Imagen);
                estatus.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                estatus.UserID = User.Identity.GetUserId();
                db.Estatus.Add(estatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", estatus.ID_Empresa);
            return View(estatus);
        }

        private void GetFileNames(Estatus estatus, HttpPostedFileBase Imagen)
        {
            bool edit = estatus.ID_Estatus > 0;
            estatus.Imagen = util.SaveOrMoveFile(estatus.Imagen, Imagen, edit);
        }

        private void GetTempFileNames(Estatus estatus, HttpPostedFileBase Imagen)
        {
            bool edit = estatus.ID_Estatus > 0;
            string filename = estatus.Imagen;
            int sizefile = !String.IsNullOrEmpty(filename) && edit ? 1000 : estatus.Peso_Imagen;
            ModelState.Remove("Imagen");
            ModelState.Remove("Peso_Imagen");
            estatus.Imagen = util.SaveNewTempFile(filename, Imagen, edit);
            estatus.Peso_Imagen = Imagen != null ? Imagen.ContentLength : (sizefile);
        }


        // GET: Estatus/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus estatus = db.Estatus.Find(id);
            if (estatus == null)
            {
                return HttpNotFound();
            }
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", estatus.ID_Empresa);
            return View(estatus);
        }

        // POST: Estatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Estatus,Nombre,Imagen,Peso_Imagen,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID")] Estatus estatus, HttpPostedFileBase File_Imagen)
        {
            GetTempFileNames(estatus, File_Imagen);
            valid.ValidarEstatus(ModelState, estatus);
            if (ModelState.IsValid)
            {
                GetFileNames(estatus, File_Imagen);
                estatus.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                estatus.UserID = User.Identity.GetUserId();
                db.Entry(estatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", estatus.ID_Empresa);
            return View(estatus);
        }

        // GET: Estatus/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estatus estatus = db.Estatus.Find(id);
            if (estatus == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View(estatus);
        }

        // POST: Estatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarEstatus(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
