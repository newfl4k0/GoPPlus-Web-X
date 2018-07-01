using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ColoniasController : _GeneralController
    {
        // GET: Colonias
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            string usuario = User.Identity.GetUserId();
            AspNetUsers cUser = db.AspNetUsers.Find(usuario);
            string[] listaafiliados = cUser.afiliados.Split(Convert.ToChar(","));
            List<Afiliados> afi = new List<Afiliados>();
            List<List<Colonias>> coloniass = new List<List<Colonias>>();
            List<Colonias> colonias = new List<Colonias>();

            foreach (var item in listaafiliados)
            {
                int idaf = item.ToString().Trim().Length > 0 ? Convert.ToInt32(item.ToString()) : 0;
                if (idaf == 0)
                {
                    List<Afiliados> af = db.Afiliados.OrderBy(x => x.ID_Afiliado).ToList();
                    //hardcode para pruebas. quitar afiliados solo dejar león
                    int[] id = new int[af.Count];
                    for (int i = 0; i < af.Count - 1; i++)
                    {
                        id[i] = af[i].ID_Afiliado;

                    }
                    int idAfiliadoActual = 0;
                    for (int i = 0; i < 1; i++)
                    {
                        idAfiliadoActual = id[i];
                        if (!(idAfiliadoActual == 0))
                        {
                            Afiliados lisafi = db.Afiliados.Include(c => c.Calles).Include(l => l.Calles.Colonias).Where(a => a.ID_Afiliado == idAfiliadoActual).FirstOrDefault();
                            Colonias col = db.Colonias.Where(co => co.ID_Colonia == lisafi.Calles.ID_Colonia).FirstOrDefault();
                            colonias.Add(col);
                        }

                    }
                }
                else
                {
                    Afiliados lisafi2 = db.Afiliados.Include(c => c.Calles).Include(l => l.Calles.Colonias).Where(a => a.ID_Afiliado == idaf).FirstOrDefault();
                    Colonias col2 = db.Colonias.Where(co => co.ID_Colonia == lisafi2.Calles.ID_Colonia).FirstOrDefault();
                    colonias.Add(col2);
                }
            }
            for (int i = 0; i < coloniass.Count; i++)
            {
                colonias = colonias.Union(coloniass[i]).ToList();
            }
            colonias = colonias.OrderBy(x => x.ID_Ciudad.ToString()).OrderBy(x => x.Nombre).ToList();
            //var colonias = db.Colonias.Where(c=>c.ID_Colonia<2000).Include(c => c.Ciudades).Where(c=>c.ID_Ciudad ==1 );
            return View(colonias);
        }

        // GET: Colonias/Details/5
         
        [HasPermission("Mapas_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = db.Colonias.Find(id);
            if (colonias == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatColonias";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(colonias);
        }

        // GET: Colonias/Create
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
        }

        // POST: Colonias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Colonia,Nombre,ID_Ciudad,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colonias colonias)
        {
            valid.ValidarColonia(ModelState, colonias);
            if (ModelState.IsValid)
            {
                colonias.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                colonias.UserID = User.Identity.GetUserId();
                db.Colonias.Add(colonias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ObtenerGeografiaSelectList(colonias);
            return View(colonias);
        }

        [HasPermission("Mapas_Edicion")]
        [HttpPost]
        public JsonResult AddColonia([Bind(Include = "ID_Colonia,Nombre,ID_Ciudad,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colonias colonias)
        {
            try
            {
                valid.ValidarColonia(ModelState, colonias);
                if (ModelState.IsValid)
                {
                    colonias.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    colonias.UserID = User.Identity.GetUserId();
                    db.Colonias.Add(colonias);
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

        public JsonResult GetColony(string Nombre)
        {
            var colonias = db.Colonias.Where(c => c.Nombre.Trim() == Nombre.Trim()).Select(x => new { x.Nombre, x.Latitud, x.Longitud, x.ID_Ciudad, x.Ciudades.ID_Estado, x.ID_Colonia, x.Fecha_Creacion }).FirstOrDefault();
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }

        [HasPermission("Mapas_Edicion")]
        [HttpPost]
        public JsonResult Editcolonia([Bind(Include = "ID_Colonia,Nombre,ID_Ciudad,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colonias colonias)
        {
            try
            {
                valid.ValidarColonia(ModelState, colonias);
                if (ModelState.IsValid)
                {
                    colonias.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                    colonias.UserID = User.Identity.GetUserId();
                    db.Entry(colonias).State = EntityState.Modified;
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
        // GET: Colonias/Edit/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = db.Colonias.Find(id);
            if (colonias == null)
            {
                TempData["Mess"] = "El elemento que deseas consultar/eliminar ya no se encuentra disponible en la base de datos";
                TempData["NavBar"] = "NavBar_CatColonias";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ObtenerGeografiaSelectList(colonias);
            return View(colonias);
        }

        // POST: Colonias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Colonia,Nombre,ID_Ciudad,Latitud,Longitud,Fecha_Creacion,Fecha_Actualizacion,UserID")] Colonias colonias)
        {
            valid.ValidarColonia(ModelState, colonias);
            if (ModelState.IsValid)
            {
                colonias.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                colonias.UserID = User.Identity.GetUserId();
                db.Entry(colonias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(colonias);
            return View(colonias);
        }

        // GET: Colonias/Delete/5
         
        [HasPermission("Mapas_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = db.Colonias.Find(id);
            if (colonias == null)
            {
                TempData["Mess"] = "El elemento que deseas consultar/eliminar ya no se encuentra disponible en la base de datos";
                TempData["NavBar"] = "NavBar_CatColonias";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            ViewBag.Eliminar = db.Conductores.Where(c => c.Calles.ID_Colonia == colonias.ID_Colonia).Count() == 0
                                && db.Conductores.Where(c => c.Flotas.Calles.ID_Colonia == colonias.ID_Colonia).Count() == 0
                                && db.Conductores.Where(c => c.Flotas.Afiliados.Calles.ID_Colonia == colonias.ID_Colonia).Count() == 0;
            return View(colonias);
        }

        // POST: Colonias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Mapas_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarColonia(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerColonias(int ID_Ciudad)
        {
            var colonias = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerColoniasPorAfiliado(int ID_Afiliado)
        {
            int ID_Ciudad = db.Afiliados.Find(ID_Afiliado).Calles.Colonias.ID_Ciudad;
            var colonias = new SelectList(db.Colonias.Where(c => c.ID_Ciudad == ID_Ciudad).OrderBy(o => o.Nombre), "ID_Colonia", "Nombre");
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatlangColony(int ID_Colonia)
        {
            var colonias = db.Colonias.Where(c => c.ID_Colonia == ID_Colonia).Select(x =>new { x.Latitud,x.Longitud,x.Nombre }).FirstOrDefault();
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatlangCalle(int ID_Calle)
        {
            var calles = db.Calles.Where(c => c.ID_Calle == ID_Calle).Select(x => new { x.Latitud, x.Longitud, x.Nombre }).FirstOrDefault();
            return Json(calles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatlangColonyByName(string Nombre_Colonia)
        {
            var colonias = db.Colonias.Where(c => c.Nombre == Nombre_Colonia).Select(x => new { x.Latitud, x.Longitud, x.Nombre }).FirstOrDefault();
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatlangCalleByName(string Nombre_Calle)
        {
            var calles = db.Calles.Where(c => c.Nombre == Nombre_Calle).Select(x => new { x.Latitud, x.Longitud, x.Nombre }).FirstOrDefault();
            return Json(calles, JsonRequestBehavior.AllowGet);
        }

        private void ObtenerGeografiaSelectList(Colonias colonias)
        {
            if (colonias.ID_Ciudad == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Estados estado = db.Ciudades.Find(colonias.ID_Ciudad).Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", colonias.ID_Ciudad);
            }
        }        
    }
}
