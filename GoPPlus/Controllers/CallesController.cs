﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Filters;
using System.Web.Routing;


namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]    

    public class CallesController : _GeneralController
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }             
       
                       
        // GET: Calles
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Index()
        {
            
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var calles = db.Calles.Include(c => c.Colonias).Where(x => x.Colonias.ID_Ciudad == 1 && x.Colonias.ID_Colonia<2000);            
            return View(calles);

        }

        // GET: Calles/Details/5
         
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calles calles = db.Calles.Find(id);
            if (calles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(calles);
        }

        // GET: Calles/Create
        [HasPermission("Mapas_Edicion")]
        public ActionResult Create()
        {
            ObtenerGeografiaSelectList();
            return View();
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
        }

        // POST: Calles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Calle,Nombre,ID_Colonia,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Calles calles)
        {
            valid.ValidarCalle(ModelState, calles);
            if (ModelState.IsValid)
            {
                calles.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                calles.UserID = User.Identity.GetUserId();
                db.Calles.Add(calles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ObtenerGeografiaSelectList(calles);
            return View(calles);
        }

        [HasPermission("Mapas_Edicion")]
        [HttpPost]
        public JsonResult AddCalle([Bind(Include = "ID_Calle,Nombre,ID_Colonia,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Calles calles)
        {
            try
            {
                valid.ValidarCalle(ModelState, calles);
                if (ModelState.IsValid)
                {
                    calles.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    calles.UserID = User.Identity.GetUserId();
                    db.Calles.Add(calles);
                    db.SaveChanges();
                    return Json("Registro Exitoso.", JsonRequestBehavior.AllowGet);
                }
                return Json(string.Join("\n", ViewData.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)),
                                JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Se produjo un error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCallle(string Nombre)
        {
            var calles = db.Calles.Where(c => c.Nombre.Trim() == Nombre.Trim()).Select(x => new { x.Nombre, x.Latitud, x.Longitud, x.Colonias.Ciudades.ID_Estado, x.Colonias.Ciudades.ID_Ciudad, x.ID_Colonia, x.ID_Calle,x.Fecha_Creacion }).FirstOrDefault();
            return Json(calles, JsonRequestBehavior.AllowGet);
        }

        [HasPermission("Mapas_Edicion")]
        [HttpPost]
        public JsonResult Editcalles([Bind(Include = "ID_Calle,Nombre,ID_Colonia,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Calles calles)
        {
            try
            {
                valid.ValidarCalle(ModelState, calles);
                if (ModelState.IsValid)
                {
                    calles.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                    calles.UserID = User.Identity.GetUserId();
                    db.Entry(calles).State = EntityState.Modified; 
                    db.SaveChanges();
                    return Json("Guardado exitosamente", JsonRequestBehavior.AllowGet);
                }
                return Json(string.Join("\n", ViewData.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)),
                                JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Se produjo un error", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Calles/Edit/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calles calles = db.Calles.Find(id);
            if (calles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ObtenerGeografiaSelectList(calles);
            return View(calles);
        }

        // POST: Calles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Calle,Nombre,ID_Colonia,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Calles calles)
        {
            valid.ValidarCalle(ModelState, calles);
            if (ModelState.IsValid)
            {
                calles.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                calles.UserID = User.Identity.GetUserId();
                db.Entry(calles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(calles);
            return View(calles);
        }

        // GET: Calles/Delete/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calles calles = db.Calles.Find(id);
            if (calles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Eliminar = db.Conductores.Where(c => c.ID_Calle == calles.ID_Calle).Count() == 0
                    && db.Conductores.Where(c => c.Flotas.ID_Calle == calles.ID_Calle).Count() == 0
                    && db.Conductores.Where(c => c.Flotas.Afiliados.ID_Calle == calles.ID_Calle).Count() == 0;
            ViewBag.Mess = MensajeDelete;
            return View(calles);
        }

        // POST: Calles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarCalle(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerCalles(int ID_Colonia)
        {
            var calles = new SelectList(db.Calles.Where(c => c.ID_Colonia == ID_Colonia).OrderBy(o => o.Nombre), "ID_Calle", "Nombre");
            return Json(calles, JsonRequestBehavior.AllowGet);
        }

        private void ObtenerGeografiaSelectList(Calles calles)
        {
            if (calles.ID_Colonia == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Ciudades ciudad = db.Colonias.Find(calles.ID_Colonia).Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", calles.ID_Colonia);
            }
        }
       
    }
}
public class CustomControllerClass : Controller
{
    string CurrentUser { get; set; }

    protected override void Initialize(RequestContext requestContext)
    {
        base.Initialize(requestContext);

        if (requestContext.HttpContext.User.Identity.IsAuthenticated)
        {
            string userName = requestContext.HttpContext.User.Identity.Name;
            CurrentUser = requestContext.HttpContext.User.Identity.GetUserId();
                
        }
        
        ViewData["UserID"] = CurrentUser;
    }
}   