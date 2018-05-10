using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.Classes;
using Microsoft.AspNet.Identity;
using GoPS.CustomFilters;
using GoPS.Filters;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class UsuariosAbonadosController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: UsuariosAbonados
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            var usuariosAbonados = db.UsuariosAbonados.Where(o => ID_Afiliados.Contains(o.ClientesAbonados.ID_Afiliado)).Include(u => u.ClientesAbonados);
            return View(usuariosAbonados.ToList());
        }

        // GET: UsuariosAbonados/Details/5
         
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosAbonados usuariosAbonados = db.UsuariosAbonados.Find(id);
            if (usuariosAbonados == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            return View(usuariosAbonados);
        }

        // GET: UsuariosAbonados/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value");
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value");
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre");
            UsuariosAbonados usuariosAbonados = new UsuariosAbonados();
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            usuariosAbonados.Habilitado = true;
            return View(usuariosAbonados);
        }

        // POST: UsuariosAbonados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_UsuarioAbonado,Nombre,Habilitado,Hora_Inicio,Hora_Fin,Limite_Credito,Dias,ID_ClienteAbonado,Fecha_Creacion,Fecha_Actualizacion,UserID")] UsuariosAbonados usuariosAbonados)
        {
            valid.ValidarUsuarioAbonado(ModelState, usuariosAbonados);
            if (ModelState.IsValid)
            {
                usuariosAbonados.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                usuariosAbonados.UserID = User.Identity.GetUserId();
                db.UsuariosAbonados.Add(usuariosAbonados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Fin);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre", usuariosAbonados.ID_ClienteAbonado);
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            return View(usuariosAbonados);
        }

        // GET: UsuariosAbonados/Edit/5
         
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosAbonados usuariosAbonados = db.UsuariosAbonados.Find(id);
            if (usuariosAbonados == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Fin);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre", usuariosAbonados.ID_ClienteAbonado);
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            return View(usuariosAbonados);
        }

        // POST: UsuariosAbonados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_UsuarioAbonado,Nombre,Habilitado,Hora_Inicio,Hora_Fin,Limite_Credito,Dias,ID_ClienteAbonado,Fecha_Creacion,Fecha_Actualizacion,UserID")] UsuariosAbonados usuariosAbonados)
        {
            valid.ValidarUsuarioAbonado(ModelState, usuariosAbonados);
            if (ModelState.IsValid)
            {
                usuariosAbonados.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                usuariosAbonados.UserID = User.Identity.GetUserId();
                db.Entry(usuariosAbonados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Hora_Inicio = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Inicio);
            ViewBag.Hora_Fin = new SelectList(util.ObtenerHoras(), "Key", "Value", usuariosAbonados.Hora_Fin);
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_ClienteAbonado = new SelectList(db.ClientesAbonados.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_ClienteAbonado", "Nombre", usuariosAbonados.ID_ClienteAbonado);
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            return View(usuariosAbonados);
        }

        // GET: UsuariosAbonados/Delete/5
         
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosAbonados usuariosAbonados = db.UsuariosAbonados.Find(id);
            if (usuariosAbonados == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            usuariosAbonados.diasList = util.ObtenerDiasList(usuariosAbonados.Dias, false);
            return View(usuariosAbonados);
        }

        // POST: UsuariosAbonados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarUsuarioAbonado(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerUsuariosAbonados(int ID_ClienteAbonado)
        {
            var usuariosAbonados = new SelectList(db.UsuariosAbonados.Where(c => c.ID_ClienteAbonado == ID_ClienteAbonado).OrderBy(o => o.Nombre), "ID_UsuarioAbonado", "Nombre");
            return Json(usuariosAbonados, JsonRequestBehavior.AllowGet);
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
