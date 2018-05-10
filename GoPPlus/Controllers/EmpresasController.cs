using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.Classes;
using GoPS.Filters;
using Microsoft.AspNet.Identity;
using GoPS.CustomFilters;
using System.Threading.Tasks;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class EmpresasController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        DBServicios serv = new DBServicios();
        DBValidaciones valid = new DBValidaciones();
        Utilities util = new Utilities();

        // GET: Empresas
        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Empresas.ToList());
        }

        // GET: Empresas/Details/5
        [HasPermission("General_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(empresas);
        }

        // GET: Empresas/Create
        [HasPermission("General_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Empresa,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Empresas empresas)
        {
            valid.ValidarEmpresa(ModelState, empresas);
            if (ModelState.IsValid)
            {
                empresas.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                empresas.UserID = User.Identity.GetUserId();
                db.Empresas.Add(empresas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresas);
        }

        // GET: Empresas/Edit/5
        [HasPermission("General_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return View(empresas);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Empresa,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Empresas empresas)
        {
            valid.ValidarEmpresa(ModelState, empresas);
            if (ModelState.IsValid)
            {
                empresas.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                empresas.UserID = User.Identity.GetUserId();
                db.Entry(empresas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresas);
        }

        // GET: Empresas/Delete/5
        [HasPermission("General_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.Afiliados.ID_Empresa == empresas.ID_Empresa).Count() == 0;
            return View(empresas);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await serv.EliminarEmpresa(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
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
