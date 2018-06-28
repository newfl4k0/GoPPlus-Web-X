using GoPS.CustomFilters;
using GoPS.Models;
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
    public class PositionsController : _GeneralController 
    {
        // GET: Positions
        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.Positions.ToList());
        }

        // GET: Positions/Details/5
        [HasPermission("General_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions tblPosition = db.Positions.Find(id);
            if (tblPosition == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPosiciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tblPosition);
        }

        // GET: Positions/Create
        [HasPermission("General_Edicion")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Create([Bind(Include = "Id,Position")] Positions tblPosition)
        {
            valid.ValidarPosition(ModelState, tblPosition);
            if (ModelState.IsValid)
            {
                db.Positions.Add(tblPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPosition);
        }

        // GET: Positions/Edit/5
        [HasPermission("General_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions tblPosition = db.Positions.Find(id);
            if (tblPosition == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPosiciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(tblPosition);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Edit([Bind(Include = "Id,Position")] Positions tblPosition)
        {
            valid.ValidarPosition(ModelState, tblPosition);
            if (ModelState.IsValid)
            {
                db.Entry(tblPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPosition);
        }

        // GET: Positions/Delete/5
        [HasPermission("General_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions tblPosition = db.Positions.Find(id);
            if (tblPosition == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPosiciones";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(tblPosition);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarPosition(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        
    }
}
