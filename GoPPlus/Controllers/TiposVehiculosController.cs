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
    public class TiposVehiculosController : _GeneralController 
    {
        // GET: TiposVehiculos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var tiposVehiculos = db.TiposVehiculos.Include(t => t.Tarifas);
            return View(tiposVehiculos.ToList());
        }

        // GET: TiposVehiculos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposVehiculos tiposVehiculos = db.TiposVehiculos.Find(id);
            if (tiposVehiculos == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View(tiposVehiculos);
        }

        // GET: TiposVehiculos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", ID_Empresa);
            ViewBag.ID_Tarifa = new SelectList(db.Tarifas.OrderBy(o => o.Nombre), "ID_Tarifa", "Nombre");
            return View();
        }

        private void GetFileNames(TiposVehiculos tiposVehiculos, HttpPostedFileBase Imagen)
        {
            bool edit = tiposVehiculos.ID_TipoVehiculo > 0;
            tiposVehiculos.Imagen = util.SaveOrMoveFile(tiposVehiculos.Imagen, Imagen, edit);            
        }

        private void GetFileNames2(TiposVehiculos tiposVehiculos, HttpPostedFileBase Imagen)
        {
            bool edit = tiposVehiculos.ID_TipoVehiculo > 0;
            tiposVehiculos.ImagenRed = util.SaveOrMoveFile(tiposVehiculos.ImagenRed, Imagen, edit);
        }

        private void GetTempFileNames(TiposVehiculos tiposVehiculos, HttpPostedFileBase Imagen)
        {
            bool edit = tiposVehiculos.ID_TipoVehiculo > 0;
            string filename = tiposVehiculos.Imagen;
            int sizefile = !String.IsNullOrEmpty(filename) && edit ? 1000 : tiposVehiculos.Peso_Imagen;
            ModelState.Remove("Imagen");
            ModelState.Remove("Peso_Imagen");
            tiposVehiculos.Imagen = util.SaveNewTempFile(filename, Imagen, edit);
            tiposVehiculos.Peso_Imagen = Imagen != null ? Imagen.ContentLength : (sizefile);
        }

        private void GetTempFileNames2(TiposVehiculos tiposVehiculos, HttpPostedFileBase Imagen)
        {
            bool edit = tiposVehiculos.ID_TipoVehiculo > 0;
            string filename = tiposVehiculos.ImagenRed;
            int sizefile = !String.IsNullOrEmpty(filename) && edit ? 1000 : tiposVehiculos.Peso_ImagenRed;
            ModelState.Remove("ImagenRed");
            ModelState.Remove("Peso_ImagenRed");
            tiposVehiculos.ImagenRed = util.SaveNewTempFile(filename, Imagen, edit);
            tiposVehiculos.Peso_ImagenRed = Imagen != null ? Imagen.ContentLength : (sizefile);
        }

        // POST: TiposVehiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoVehiculo,Nombre,Imagen,Peso_Imagen,ID_Tarifa,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID,ImagenRed")] TiposVehiculos tiposVehiculos, HttpPostedFileBase File_Imagen, HttpPostedFileBase File_ImagenRed)
        {
            GetTempFileNames(tiposVehiculos, File_Imagen);
            GetTempFileNames2(tiposVehiculos, File_ImagenRed);
            valid.ValidarTipoVehiculo(ModelState, tiposVehiculos);
            if (ModelState.IsValid)
            {
                GetFileNames(tiposVehiculos, File_Imagen);
                GetFileNames2(tiposVehiculos, File_ImagenRed);
                tiposVehiculos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposVehiculos.UserID = User.Identity.GetUserId();
                db.TiposVehiculos.Add(tiposVehiculos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", tiposVehiculos.ID_Empresa);
            ViewBag.ID_Tarifa = new SelectList(db.Tarifas.OrderBy(o => o.Nombre), "ID_Tarifa", "Nombre", tiposVehiculos.ID_Tarifa);
            return View(tiposVehiculos);
        }

        // GET: TiposVehiculos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposVehiculos tiposVehiculos = db.TiposVehiculos.Find(id);
            if (tiposVehiculos == null)
            {
                return HttpNotFound();
            }
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", tiposVehiculos.ID_Empresa);
            ViewBag.ID_Tarifa = new SelectList(db.Tarifas.OrderBy(o => o.Nombre), "ID_Tarifa", "Nombre", tiposVehiculos.ID_Tarifa);
            return View(tiposVehiculos);
        }

        // POST: TiposVehiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoVehiculo,Nombre,Imagen,Peso_Imagen,ID_Tarifa,ID_Empresa,Fecha_Creacion,Fecha_Actualizacion,UserID,ImagenRed")] TiposVehiculos tiposVehiculos, HttpPostedFileBase File_Imagen, HttpPostedFileBase File_ImagenRed)
        {
            GetTempFileNames(tiposVehiculos, File_Imagen);
            GetTempFileNames2(tiposVehiculos, File_ImagenRed);
            valid.ValidarTipoVehiculo(ModelState, tiposVehiculos);
            if (ModelState.IsValid)
            {
                GetFileNames(tiposVehiculos, File_Imagen);
                GetFileNames2(tiposVehiculos, File_ImagenRed);
                tiposVehiculos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposVehiculos.UserID = User.Identity.GetUserId();
                db.Entry(tiposVehiculos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            int ID_Empresa = ID_Empresas.Count == 1 ? ID_Empresas.FirstOrDefault() : 0;
            ViewBag.ID_Empresa = new SelectList(db.Empresas.Where(e => ID_Empresas.Contains(e.ID_Empresa)).OrderBy(o => o.Nombre), "ID_Empresa", "Nombre", tiposVehiculos.ID_Empresa);
            ViewBag.ID_Tarifa = new SelectList(db.Tarifas.OrderBy(o => o.Nombre), "ID_Tarifa", "Nombre", tiposVehiculos.ID_Tarifa);
            return View(tiposVehiculos);
        }

        // GET: TiposVehiculos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposVehiculos tiposVehiculos = db.TiposVehiculos.Find(id);
            if (tiposVehiculos == null)
            {
                return HttpNotFound();
            }
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View(tiposVehiculos);
        }

        // POST: TiposVehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTipoVehiculo(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
