using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoPS.Filters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class TarjetasCreditoController : _GeneralController 
    {
        // GET: TarjetasCredito
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.TarjetasCredito.ToList());
        }

        // GET: TarjetasCredito/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetasCredito tarjetasCredito = db.TarjetasCredito.Find(id);
            if (tarjetasCredito == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTarjetas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tarjetasCredito);
        }

        // GET: TarjetasCredito/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TarjetasCredito/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_TarjetaCredito,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TarjetasCredito tarjetasCredito)
        {
            valid.ValidarTarjetaCredito(ModelState, tarjetasCredito);
            if (ModelState.IsValid)
            {
                tarjetasCredito.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                tarjetasCredito.UserID = User.Identity.GetUserId();
                db.TarjetasCredito.Add(tarjetasCredito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tarjetasCredito);
        }

        // GET: TarjetasCredito/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetasCredito tarjetasCredito = db.TarjetasCredito.Find(id);
            if (tarjetasCredito == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTarjetas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tarjetasCredito);
        }

        // POST: TarjetasCredito/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_TarjetaCredito,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] TarjetasCredito tarjetasCredito)
        {
            valid.ValidarTarjetaCredito(ModelState, tarjetasCredito);
            if (ModelState.IsValid)
            {
                tarjetasCredito.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                tarjetasCredito.UserID = User.Identity.GetUserId();
                db.Entry(tarjetasCredito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tarjetasCredito);
        }

        // GET: TarjetasCredito/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetasCredito tarjetasCredito = db.TarjetasCredito.Find(id);
            if (tarjetasCredito == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatTarjetas";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(tarjetasCredito);
        }

        // POST: TarjetasCredito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarTarjetaCredito(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
