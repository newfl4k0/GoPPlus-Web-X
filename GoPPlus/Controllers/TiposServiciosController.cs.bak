using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GoPS.Controllers
{
    [ValidateInput(false)]
    public class TiposServiciosController : _GeneralController 
    {
        // GET: TiposServicios
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TiposServicios.ToList());
        }

        // GET: TiposServicios/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposServicios tiposServicios = db.TiposServicios.Find(id);
            if (tiposServicios == null)
            {
                return HttpNotFound();
            }
            return View(tiposServicios);
        }

        // GET: TiposServicios/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposServicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TipoServicio,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposServicios tiposServicios)
        {
            valid.ValidarTipoServicio(ModelState, tiposServicios);
            if (ModelState.IsValid)
            {
                tiposServicios.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposServicios.UserID = User.Identity.GetUserId();
                db.TiposServicios.Add(tiposServicios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposServicios);
        }

        // GET: TiposServicios/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposServicios tiposServicios = db.TiposServicios.Find(id);
            if (tiposServicios == null)
            {
                return HttpNotFound();
            }
            return View(tiposServicios);
        }

        // POST: TiposServicios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TipoServicio,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TiposServicios tiposServicios)
        {
            valid.ValidarTipoServicio(ModelState, tiposServicios);
            if (ModelState.IsValid)
            {
                tiposServicios.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tiposServicios.UserID = User.Identity.GetUserId();
                db.Entry(tiposServicios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposServicios);
        }

        // GET: TiposServicios/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposServicios tiposServicios = db.TiposServicios.Find(id);
            if (tiposServicios == null)
            {
                return HttpNotFound();
            }
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.Afiliados.ID_TipoServicio == tiposServicios.ID_TipoServicio).Count() == 0;
            return View(tiposServicios);
        }

        // POST: TiposServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await serv.EliminarTipoServicio(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
