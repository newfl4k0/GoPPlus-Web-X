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
using GoPS.Classes;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class DatosTicketsController : _GeneralController 
    {
        // GET: DatosTickets
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var datosTickets = db.DatosTickets.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).Include(d => d.Afiliados);
            return View(datosTickets.ToList());
        }

        // GET: DatosTickets/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosTickets datosTickets = db.DatosTickets.Find(id);
            if (datosTickets == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDatosTicket";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(datosTickets);
        }

        // GET: DatosTickets/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View();
        }

        private void GetFileNames(DatosTickets datosTickets, HttpPostedFileBase Imagen)
        {
            bool edit = datosTickets.ID_DatoTicket > 0;
            datosTickets.Imagen = util.SaveOrMoveFile(datosTickets.Imagen, Imagen, edit);
        }

        private void GetTempFileNames(DatosTickets datosTickets, HttpPostedFileBase Imagen)
        {
            bool edit = datosTickets.ID_DatoTicket > 0;
            string filename = datosTickets.Imagen;
            int sizefile = !String.IsNullOrEmpty(filename) && edit ? 1000 : datosTickets.Peso_Imagen;
            ModelState.Remove("Imagen");
            ModelState.Remove("Peso_Imagen");
            datosTickets.Imagen = util.SaveNewTempFile(filename, Imagen, edit);
            datosTickets.Peso_Imagen = Imagen != null ? Imagen.ContentLength : (sizefile);
        }

        // POST: DatosTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_DatoTicket,Texto_1,Texto_2,Texto_3,Texto_4,Texto_5,Texto_6,Texto_7,Texto_8,Texto_9,Texto_10,Imagen,Peso_Imagen,Fecha_Bordo,Fecha_Finalizacion,No_Taxi,Placas,Nombre_Cliente,Origen,Destino,Importe,Observaciones,Despacho,Codigo,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] DatosTickets datosTickets, HttpPostedFileBase File_Imagen)
        {
            GetTempFileNames(datosTickets, File_Imagen);
            valid.ValidarDatosTicket(ModelState, datosTickets);
            if (ModelState.IsValid)
            {
                GetFileNames(datosTickets, File_Imagen);
                datosTickets.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                datosTickets.UserID = User.Identity.GetUserId();
                db.DatosTickets.Add(datosTickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", datosTickets.ID_Afiliado);
            return View(datosTickets);
        }

        // GET: DatosTickets/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosTickets datosTickets = db.DatosTickets.Find(id);
            if (datosTickets == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDatosTicket";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", datosTickets.ID_Afiliado);
            return View(datosTickets);
        }

        // POST: DatosTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_DatoTicket,Texto_1,Texto_2,Texto_3,Texto_4,Texto_5,Texto_6,Texto_7,Texto_8,Texto_9,Texto_10,Imagen,Peso_Imagen,Fecha_Bordo,Fecha_Finalizacion,No_Taxi,Placas,Nombre_Cliente,Origen,Destino,Importe,Observaciones,Despacho,Codigo,ID_Afiliado,Fecha_Creacion,Fecha_Actualizacion,UserID")] DatosTickets datosTickets, HttpPostedFileBase File_Imagen)
        {
            GetTempFileNames(datosTickets, File_Imagen);
            valid.ValidarDatosTicket(ModelState, datosTickets);
            if (ModelState.IsValid)
            {
                GetFileNames(datosTickets, File_Imagen);
                datosTickets.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                datosTickets.UserID = User.Identity.GetUserId();
                db.Entry(datosTickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", datosTickets.ID_Afiliado);
            return View(datosTickets);
        }

        // GET: DatosTickets/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosTickets datosTickets = db.DatosTickets.Find(id);
            if (datosTickets == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatDatosTicket";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(datosTickets);
        }

        // POST: DatosTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarDatoTicket(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
