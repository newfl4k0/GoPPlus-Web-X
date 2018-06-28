using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
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
    public class BancosController : _GeneralController 
    {
        // GET: Bancos
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
			var _list = db.Bancos.ToList();
            return View(db.Bancos.ToList());
        }

        // GET: Bancos/Details/5
        [HasPermission("Configuraciones_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bancos bancos = db.Bancos.Find(id);
            if (bancos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatBancos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(bancos);
        }

        // GET: Bancos/Create
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create()
        {          
            return View();
        }

        // POST: Bancos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Banco,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Bancos bancos)
        {
            valid.ValidarBanco(ModelState, bancos);
            if (ModelState.IsValid)
            {
                bancos.Fecha_Creacion = util.ConvertToMexicanDate(DateTime.Now);
                bancos.UserID = User.Identity.GetUserId();
                db.Bancos.Add(bancos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bancos);
        }

        // GET: Bancos/Edit/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bancos bancos = db.Bancos.Find(id);
            if (bancos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatBancos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(bancos);
        }

        // POST: Bancos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Banco,Nombre,Fecha_Creacion,Fecha_Actualizacion,UserID")] Bancos bancos)
        {
            valid.ValidarBanco(ModelState, bancos);
            if (ModelState.IsValid)
            {
                bancos.Fecha_Actualizacion = util.ConvertToMexicanDate(DateTime.Now);
                bancos.UserID = User.Identity.GetUserId();
                db.Entry(bancos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bancos);
        }

        // GET: Bancos/Delete/5
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bancos bancos = db.Bancos.Find(id);
            if (bancos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatBancos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Eliminar = db.Conductores.Where(c => c.Flotas.ID_Banco == bancos.ID_Banco).Count() == 0;
			ViewBag.Mess = MensajeDelete;
            return View(bancos);
        }

        // POST: Bancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Configuraciones_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarBanco(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
