using System;
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

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ParticionesCallesController : _GeneralController
    {
        // GET: ParticionesCalles
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var particionesCalles = db.ParticionesCalles.Include(p => p.Calles).Where(p=>p.Calles.ID_Colonia<2000);
            return View(particionesCalles.ToList());
        }

        // GET: ParticionesCalles/Details/5
         
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticionesCalles particionesCalles = db.ParticionesCalles.Find(id);
            if (particionesCalles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatParticionesCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(particionesCalles);
        }

        // GET: ParticionesCalles/Create
        [HasPermission("Mapas_Edicion")]
        public ActionResult Create()
        {
            ObtenerGeografiaSelectList();
            return View();
        }

        [HasPermission("Mapas_Visualizacion")]
        public ActionResult ConsultaLocalizacion()
        {
            var particionesCalles = db.ParticionesCalles.Include(p => p.Calles.Colonias).Where(p=>p.Calles.ID_Colonia<2000).ToList();
            return View(particionesCalles);
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        // POST: ParticionesCalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Create([Bind(Include = "ID_ParticionCalle,ID_Calle,Latitud,Longitud,Numero,Fecha_Creacion,Fecha_Actualizacion,UserID")] ParticionesCalles particionesCalles)
        {
            try
            {
                valid.ValidarLocalizacion(ModelState,particionesCalles);
                if (ModelState.IsValid)
                {
                    particionesCalles.Numero = 1;
                    particionesCalles.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    particionesCalles.UserID = User.Identity.GetUserId();                    
                    db.ParticionesCalles.Add(particionesCalles);                    
                    db.SaveChanges();
                    ObtenerGeografiaSelectList(particionesCalles);
                    //return RedirectToAction("Index");
                    return Json("Guardado exitosamente", JsonRequestBehavior.AllowGet);
                }
                return Json(string.Join("\n", ViewData.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage)),
                            JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Se produjo un error", JsonRequestBehavior.AllowGet);
            }
            //return View(particionesCalles);
        }

        // GET: ParticionesCalles/Edit/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticionesCalles particionesCalles = db.ParticionesCalles.Find(id);
            if (particionesCalles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatParticionesCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ObtenerGeografiaSelectList(particionesCalles);
            return View(particionesCalles);
        }

        // POST: ParticionesCalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_ParticionCalle,ID_Calle,Latitud,Longitud,Numero,Fecha_Creacion,Fecha_Actualizacion,UserID")] ParticionesCalles particionesCalles)
        {
            valid.ValidarLocalizacion(ModelState, particionesCalles);
            if (ModelState.IsValid)
            {
                particionesCalles.Numero = 1;
                particionesCalles.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                particionesCalles.UserID = User.Identity.GetUserId();
                db.Entry(particionesCalles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(particionesCalles);
            return View(particionesCalles);
        }

        // GET: ParticionesCalles/Delete/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticionesCalles particionesCalles = db.ParticionesCalles.Find(id);
            if (particionesCalles == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatParticionesCalles";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(particionesCalles);
        }

        // POST: ParticionesCalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarParticionCalle(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList(ParticionesCalles particionesCalles)
        {
            if (particionesCalles.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(particionesCalles.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", particionesCalles.ID_Calle);
            }
        }

        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Consultas()
        {
            var particionesCalles = db.ParticionesCalles.Include(p => p.Calles.Colonias).ToList();
            return View(particionesCalles);
        }

        public JsonResult GetLatLong(int ID_ParticionCalle)
        {
            var localizacion = db.ParticionesCalles.Where(c => c.ID_ParticionCalle == ID_ParticionCalle).Select(x => new { x.Latitud, x.Longitud }).FirstOrDefault();
            return Json(localizacion, JsonRequestBehavior.AllowGet);
        }
        
    }
}
