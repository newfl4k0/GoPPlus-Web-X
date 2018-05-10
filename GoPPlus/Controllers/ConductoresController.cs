using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class ConductoresController : Controller
    {
        //private Entities ent = new Entities();
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Conductores
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var conductores = db.Conductores.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).OrderBy(o => o.Flotas.ID_Flota).ThenBy(o => o.Flotas.Nombre).Include(c => c.Calles).Include(c => c.Turnos);
            return View(conductores.ToList());
        }

        // GET: Conductores/Details/5
         
        [HasPermission("Vehiculos_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(conductores);
        }

        // GET: Conductores/Create
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create()
        {
            ObtenerGeografiaSelectList();
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre");
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre");
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver").OrderBy(o => o.UserName), "Id", "UserName");
            return View(new Conductores() { Habilitado = true, Liquido = true, Fecha_Nacimiento = util.ConvertToMexicanDate(DateTime.Now) });
        }

        // POST: Conductores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Conductor,Nombre,Apellido,ID_Calle,Numero_Exterior,Numero_Interior,Telefono,Celular,RFC,NoLicencia,Habilitado,Fecha_Nacimiento,ID_Turno,ID_Flota,NoTarjeton,Vigencia_Licencia,Vigencia_Tarjeton,Liquido,Archivo_CartaFianza,Archivo_INE,Archivo_Domicilio,Archivo_Tarjeton,Archivo_Licencia,Archivo_Antidoping,Archivo_NoAntecedentes,Peso_Archivo_CartaFianza,Peso_Archivo_INE,Peso_Archivo_Domicilio,Peso_Archivo_Tarjeton,Peso_Archivo_Licencia,Peso_Archivo_Antidoping,Peso_Archivo_NoAntecedentes,Fecha_Creacion,Fecha_Actualizacion,UserID,UserID_Conductor")] Conductores conductores, HttpPostedFileBase File_Archivo_CartaFianza, HttpPostedFileBase File_Archivo_INE, HttpPostedFileBase File_Archivo_Domicilio, HttpPostedFileBase File_Archivo_Tarjeton, HttpPostedFileBase File_Archivo_Licencia, HttpPostedFileBase File_Archivo_Antidoping, HttpPostedFileBase File_Archivo_NoAntecedentes)
        {
            GetTempFileNames(conductores, File_Archivo_CartaFianza, File_Archivo_INE, File_Archivo_Domicilio, File_Archivo_Tarjeton, File_Archivo_Licencia, File_Archivo_Antidoping, File_Archivo_NoAntecedentes);
            valid.ValidarConductor(ModelState, conductores);
            if (ModelState.IsValid)
            {
                GetFileNames(conductores, File_Archivo_CartaFianza, File_Archivo_INE, File_Archivo_Domicilio, File_Archivo_Tarjeton, File_Archivo_Licencia, File_Archivo_Antidoping, File_Archivo_NoAntecedentes);
                CheckUser(conductores);
                conductores.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                conductores.UserID = User.Identity.GetUserId();
                db.Conductores.Add(conductores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerGeografiaSelectList(conductores);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", conductores.ID_Flota);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", conductores.ID_Turno);
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver").OrderBy(o => o.UserName), "Id", "UserName", conductores.UserID_Conductor);
            return View(conductores);
        }

        // GET: Conductores/Edit/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            ObtenerGeografiaSelectList(conductores);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", conductores.ID_Flota);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", conductores.ID_Turno);
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver").OrderBy(o => o.UserName), "Id", "UserName", conductores.UserID_Conductor);
            return View(conductores);
        }

        // POST: Conductores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Conductor,Nombre,Apellido,ID_Calle,Numero_Exterior,Numero_Interior,Telefono,Celular,RFC,NoLicencia,Habilitado,Fecha_Nacimiento,ID_Turno,ID_Flota,NoTarjeton,Vigencia_Licencia,Vigencia_Tarjeton,Liquido,Archivo_CartaFianza,Archivo_INE,Archivo_Domicilio,Archivo_Tarjeton,Archivo_Licencia,Archivo_Antidoping,Archivo_NoAntecedentes,Peso_Archivo_CartaFianza,Peso_Archivo_INE,Peso_Archivo_Domicilio,Peso_Archivo_Tarjeton,Peso_Archivo_Licencia,Peso_Archivo_Antidoping,Peso_Archivo_NoAntecedentes,Usuario,Contrasena,token,Fecha_Creacion,Fecha_Actualizacion,UserID,UserID_Conductor")] Conductores conductores, HttpPostedFileBase File_Archivo_CartaFianza, HttpPostedFileBase File_Archivo_INE, HttpPostedFileBase File_Archivo_Domicilio, HttpPostedFileBase File_Archivo_Tarjeton, HttpPostedFileBase File_Archivo_Licencia, HttpPostedFileBase File_Archivo_Antidoping, HttpPostedFileBase File_Archivo_NoAntecedentes)
        {
            GetTempFileNames(conductores, File_Archivo_CartaFianza, File_Archivo_INE, File_Archivo_Domicilio, File_Archivo_Tarjeton, File_Archivo_Licencia, File_Archivo_Antidoping, File_Archivo_NoAntecedentes);
            valid.ValidarConductor(ModelState, conductores);
            if (ModelState.IsValid)
            {
                GetFileNames(conductores, File_Archivo_CartaFianza, File_Archivo_INE, File_Archivo_Domicilio, File_Archivo_Tarjeton, File_Archivo_Licencia, File_Archivo_Antidoping, File_Archivo_NoAntecedentes);
                CheckUser(conductores);
                conductores.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                conductores.UserID = User.Identity.GetUserId();
                db.Entry(conductores).State = EntityState.Modified;            
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ObtenerGeografiaSelectList(conductores);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Flota = new SelectList(db.Flotas.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Flota", "Nombre", conductores.ID_Flota);
            ViewBag.ID_Turno = new SelectList(db.Turnos.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Turno", "Nombre", conductores.ID_Turno);
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver").OrderBy(o => o.UserName), "Id", "UserName", conductores.UserID_Conductor);
            return View(conductores);
        }

        private void CheckUser(Conductores conductores)
        {
            bool user_selected = !String.IsNullOrEmpty(conductores.UserID_Conductor);
            //if (user_selected)
            //    DesasociarConductor(conductores.UserID_Conductor);
            conductores.Habilitado = user_selected;
        }

        private void GetFileNames(Conductores conductores, HttpPostedFileBase Archivo_CartaFianza, HttpPostedFileBase Archivo_INE, HttpPostedFileBase Archivo_Domicilio, HttpPostedFileBase Archivo_Tarjeton, HttpPostedFileBase Archivo_Licencia, HttpPostedFileBase Archivo_Antidoping, HttpPostedFileBase Archivo_NoAntecedentes)
        {
            bool edit = conductores.ID_Conductor > 0;
            conductores.Archivo_CartaFianza = util.SaveOrMoveFile(conductores.Archivo_CartaFianza, Archivo_CartaFianza, edit);
            conductores.Archivo_INE = util.SaveOrMoveFile(conductores.Archivo_INE, Archivo_INE, edit);
            conductores.Archivo_Domicilio = util.SaveOrMoveFile(conductores.Archivo_Domicilio, Archivo_Domicilio, edit);
            conductores.Archivo_Tarjeton = util.SaveOrMoveFile(conductores.Archivo_Tarjeton, Archivo_Tarjeton, edit);
            conductores.Archivo_Licencia = util.SaveOrMoveFile(conductores.Archivo_Licencia, Archivo_Licencia, edit);
            conductores.Archivo_Antidoping = util.SaveOrMoveFile(conductores.Archivo_Antidoping, Archivo_Antidoping, edit);
            conductores.Archivo_NoAntecedentes = util.SaveOrMoveFile(conductores.Archivo_NoAntecedentes, Archivo_NoAntecedentes, edit);
        }

        private void GetTempFileNames(Conductores conductores, HttpPostedFileBase Archivo_CartaFianza, HttpPostedFileBase Archivo_INE, HttpPostedFileBase Archivo_Domicilio, HttpPostedFileBase Archivo_Tarjeton, HttpPostedFileBase Archivo_Licencia, HttpPostedFileBase Archivo_Antidoping, HttpPostedFileBase Archivo_NoAntecedentes)
        {
            bool edit = conductores.ID_Conductor > 0;
            string filename = conductores.Archivo_CartaFianza;
            int sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_CartaFianza;
            ModelState.Remove("Archivo_CartaFianza");
            ModelState.Remove("Peso_Archivo_CartaFianza");
            conductores.Archivo_CartaFianza = util.SaveNewTempFile(filename, Archivo_CartaFianza, edit);
            conductores.Peso_Archivo_CartaFianza = Archivo_CartaFianza != null ? Archivo_CartaFianza.ContentLength : (sizefile);
            filename = conductores.Archivo_INE;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_INE;
            ModelState.Remove("Archivo_INE");
            ModelState.Remove("Peso_Archivo_INE");
            conductores.Archivo_INE = util.SaveNewTempFile(filename, Archivo_INE, edit);
            conductores.Peso_Archivo_INE = Archivo_INE != null ? Archivo_INE.ContentLength : sizefile;
            filename = conductores.Archivo_Domicilio;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_Domicilio;
            ModelState.Remove("Archivo_Domicilio");
            ModelState.Remove("Peso_Archivo_Domicilio");
            conductores.Archivo_Domicilio = util.SaveNewTempFile(filename, Archivo_Domicilio, edit);
            conductores.Peso_Archivo_Domicilio = Archivo_Domicilio != null ? Archivo_Domicilio.ContentLength : sizefile;
            filename = conductores.Archivo_Tarjeton;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_Tarjeton;
            ModelState.Remove("Archivo_Tarjeton");
            ModelState.Remove("Peso_Archivo_Tarjeton");
            conductores.Archivo_Tarjeton = util.SaveNewTempFile(filename, Archivo_Tarjeton, edit);
            conductores.Peso_Archivo_Tarjeton = Archivo_Tarjeton != null ? Archivo_Tarjeton.ContentLength : sizefile;
            filename = conductores.Archivo_Licencia;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_Tarjeton;
            ModelState.Remove("Archivo_Licencia");
            ModelState.Remove("Peso_Archivo_Licencia");
            conductores.Archivo_Licencia = util.SaveNewTempFile(filename, Archivo_Licencia, edit);
            conductores.Peso_Archivo_Licencia = Archivo_Licencia != null ? Archivo_Licencia.ContentLength : sizefile;
            filename = conductores.Archivo_Antidoping;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_Antidoping;
            ModelState.Remove("Archivo_Antidoping");
            ModelState.Remove("Peso_Archivo_Antidoping");
            conductores.Archivo_Antidoping = util.SaveNewTempFile(filename, Archivo_Antidoping, edit);
            conductores.Peso_Archivo_Antidoping = Archivo_Antidoping != null ? Archivo_Antidoping.ContentLength : sizefile;
            filename = conductores.Archivo_NoAntecedentes;
            sizefile = !String.IsNullOrEmpty(filename) && conductores.ID_Conductor > 0 ? 1000 : conductores.Peso_Archivo_NoAntecedentes;
            ModelState.Remove("Archivo_NoAntecedentes");
            ModelState.Remove("Peso_Archivo_NoAntecedentes");
            conductores.Archivo_NoAntecedentes = util.SaveNewTempFile(filename, Archivo_NoAntecedentes, edit);
            conductores.Peso_Archivo_NoAntecedentes = Archivo_NoAntecedentes != null ? Archivo_NoAntecedentes.ContentLength : sizefile;
        }

        // GET: Conductores/Assign/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            Vehiculos_Conductores vehiculos_Conductores = conductores.Vehiculos_Conductores.Where(c => c.Activo).OrderByDescending(c => c.Fecha_Asignacion).FirstOrDefault();
            if (vehiculos_Conductores != null)
                ViewBag.ID_Vehiculo = new SelectList(db.Vehiculos.Where(o => o.ID_Flota == conductores.ID_Flota && o.Habilitado == true).OrderBy(o => o.Matricula), "ID_Vehiculo", "Matricula", vehiculos_Conductores.ID_Vehiculo);
            else
            {
                vehiculos_Conductores = new Vehiculos_Conductores();
                vehiculos_Conductores.ID_Conductor = conductores.ID_Conductor;
                vehiculos_Conductores.Conductores = conductores;
                ViewBag.ID_Vehiculo = new SelectList(db.Vehiculos.Where(o => o.ID_Flota == conductores.ID_Flota).OrderBy(o => o.Matricula), "ID_Vehiculo", "Matricula");
            }
            return View(vehiculos_Conductores);
        }

        // POST: Conductores/Assign/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Assign([Bind(Include = "ID_Vehiculo_Conductor,ID_Vehiculo,ID_Conductor,Fecha_Asignacion,Activo,Fecha_Creacion,Fecha_Actualizacion,UserID")] Vehiculos_Conductores vehiculos_Conductores)
        {
            Conductores conductores = db.Conductores.Find(vehiculos_Conductores.ID_Conductor);
            
            Vehiculos_Conductores vh = conductores.Vehiculos_Conductores.Where(vc => vc.Activo).OrderByDescending(vc => vc.Fecha_Asignacion).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (vh == null || vh.ID_Vehiculo != vehiculos_Conductores.ID_Vehiculo)
                {
                    vehiculos_Conductores.Fecha_Asignacion = util.ConvertToMexicanDate(DateTime.Now);
                    vehiculos_Conductores.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                    vehiculos_Conductores.UserID = User.Identity.GetUserId();
                    db.Vehiculos_Conductores.Add(vehiculos_Conductores);
                    db.SaveChanges();
                    db.DesactivarVehiculosConductoresPorConductor(vehiculos_Conductores.ID_Vehiculo_Conductor, vehiculos_Conductores.ID_Conductor, User.Identity.GetUserId());
                }
                return RedirectToAction("Index");
            }
            vehiculos_Conductores.Conductores = conductores;
            ViewBag.ID_Vehiculo = new SelectList(db.Vehiculos.OrderBy(o => o.Matricula), "ID_Vehiculo", "Matricula", vehiculos_Conductores.ID_Vehiculo);
            return View(vehiculos_Conductores);
        }


        // GET: Conductores/Assign/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult AssignUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver").OrderBy(o => o.UserName), "Id", "UserName", conductores.UserID_Conductor);
            return View(conductores);
        }

        // POST: Conductores/Assign/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult AssignUser([Bind(Include = "ID_Conductor,UserID_Conductor")] Conductores conductores)
        {
            Conductores conductores_user = db.Conductores.Find(conductores.ID_Conductor);
            conductores_user.UserID_Conductor = conductores.UserID_Conductor;
            //if (ModelState.IsValid)
            //{
            CheckUser(conductores_user);
            conductores_user.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
            conductores_user.UserID = User.Identity.GetUserId();
            db.Entry(conductores_user).State = EntityState.Modified;
            
            if (valid.ValidaAssignUser(ModelState, conductores_user))
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            //}
            ViewBag.UserID_Conductor = new SelectList(db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToLower() == "driver" && !db.Conductores.Select(c => c.UserID_Conductor).Contains(u.Id)).OrderBy(o => o.UserName), "Id", "UserName", conductores.UserID_Conductor);
            return View(conductores);
        }

        private void DesasociarConductor(string UserID)
        {
            Conductores old_conductor = db.Conductores.AsNoTracking().Where(c => c.UserID_Conductor == UserID).FirstOrDefault();
            if (old_conductor != null)
            {
                old_conductor.UserID_Conductor = null;
                old_conductor.Habilitado = false;
                old_conductor.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                old_conductor.UserID = User.Identity.GetUserId();
                db.Entry(old_conductor).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Conductores/Delete/5
         
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(conductores);
        }

        // POST: Conductores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Vehiculos_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarConductor(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            ViewBag.ID_Calle = new SelectList(db.Calles.OrderBy(o => o.Nombre).Take(0), "ID_Calle", "Nombre");
        }

        private void ObtenerGeografiaSelectList(Conductores conductores)
        {
            if (conductores.ID_Calle == 0)
                ObtenerGeografiaSelectList();
            else
            {
                Colonias colonia = db.Calles.Find(conductores.ID_Calle).Colonias;
                Ciudades ciudad = colonia.Ciudades;
                Estados estado = ciudad.Estados;
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);
                ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                ViewBag.ID_Calle = new SelectList(colonia.Calles.OrderBy(o => o.Nombre), "ID_Calle", "Nombre", conductores.ID_Calle);
            }
        }

        public JsonResult ObtenerConductores(int ID_Flota)
        {
            var conductores = new SelectList(db.Conductores.ToList().Where(c => c.ID_Flota == ID_Flota).OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto");
            return Json(conductores, JsonRequestBehavior.AllowGet);
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
