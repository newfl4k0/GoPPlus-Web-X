using GoPS.CustomFilters;
using GoPS.Models;
using System;
using System.Collections.Generic;
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
    public class PermissionsController : _GeneralController 
    {
        // GET: Permisos
        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            var permisos = db.Permissions;
            return View(permisos.ToList());
        }

        // GET: Permisos/Details/5
        [HasPermission("General_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permissions permisos = db.Permissions.Find(id);
            if (permisos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPermisos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            return View(permisos);
        }

        // GET: Permisos/Create
        [HasPermission("General_Edicion")]
        public ActionResult Create()
        {
            Permissions permisos = new Permissions();
            permisos.rolesList = util.ObtenerCheckBoxRolesList(permisos.roles);
            permisos.Able = true;
            return View(permisos);
        }

        // POST: Permisos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Able,roles")] Permissions permisos)
        {
            valid.ValidarPermiso(ModelState, permisos);
            if (ModelState.IsValid)
            {
                db.Permissions.Add(permisos);
                db.SaveChanges();
                AgregarRoles(permisos);
                return RedirectToAction("Index");
            }
            permisos.rolesList = util.ObtenerCheckBoxRolesList(permisos.roles);
            return View(permisos);
        }

        private void AgregarRoles(Permissions permisos)
        {
            Permissions newPermission = new Permissions();
            List<string> rolesChecked = String.IsNullOrEmpty(permisos.roles) ? new List<string>() : permisos.roles.Split(',').ToList();
            foreach (string rol in rolesChecked)
            {
                AspNetRoles role = db.AspNetRoles.Find(rol);
                newPermission.AspNetRoles.Add(role);
            }

            var existingPermission = db.Permissions.Include("AspNetRoles").Where(e => e.Id == permisos.Id).FirstOrDefault<Permissions>();

            var deletedRoles = existingPermission.AspNetRoles.Except(newPermission.AspNetRoles).ToList<AspNetRoles>();
            deletedRoles.ForEach(c => existingPermission.AspNetRoles.Remove(c));

            var addedRoles = newPermission.AspNetRoles.Except(existingPermission.AspNetRoles).ToList<AspNetRoles>();

            foreach (AspNetRoles r in addedRoles)
            {
                if (db.Entry(r).State == EntityState.Detached)
                    db.AspNetRoles.Attach(r);
                existingPermission.AspNetRoles.Add(r);
            }

            db.SaveChanges();
        }

        // GET: Permisos/Edit/5
        [HasPermission("General_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permissions permisos = db.Permissions.Find(id);
            if (permisos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPermisos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            permisos.roles = string.Join(",", permisos.AspNetRoles.Select(a => a.Id).Select(a => a.ToString()).ToArray());
            permisos.rolesList = util.ObtenerCheckBoxRolesList(permisos.roles);
            return View(permisos);
        }

        // POST: Permisos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Able,roles")] Permissions permisos)
        {
            valid.ValidarPermiso(ModelState, permisos);
            if (ModelState.IsValid)
            {
                db.Entry(permisos).State = EntityState.Modified;
                db.SaveChanges();
                AgregarRoles(permisos);
                return RedirectToAction("Index");
            }
            permisos.rolesList = util.ObtenerCheckBoxRolesList(permisos.roles);
            return View(permisos);
        }

        // GET: Permisos/Delete/5
        [HasPermission("General_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permissions permisos = db.Permissions.Find(id);
            if (permisos == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatPermisos";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(permisos);
        }

        // POST: Permisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            serv.EliminarPermiso(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
    }
}
