using GoPS.CustomFilters;
using GoPS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GoPS.Filters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class AspNetRolesController : _GeneralController 
    {
        // GET: AspNetRoles
        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            return View(db.AspNetRoles.ToList());
        }

        // GET: AspNetRoles/Details/5
        [HasPermission("General_Visualizacion")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Create
        [HasPermission("General_Edicion")]
        public ActionResult Create()
        {
            AspNetRoles aspNetRole = new AspNetRoles();
            aspNetRole.permisosList = util.ObtenerPermisosList(aspNetRole.permisos);
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public ActionResult Create([Bind(Include = "Id,Name,Afiliado,permisos")] AspNetRoles aspNetRole)
        {
            valid.ValidarPerfil(ModelState, aspNetRole);
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
                if (!roleManager.RoleExists(aspNetRole.Name))
                {
                    var role = new ApplicationRole();
                    role.Name = aspNetRole.Name;
                    role.Afiliado = aspNetRole.Afiliado;
                    roleManager.Create(role);
                    aspNetRole.Id = role.Id;
                    AgregarPermisos(aspNetRole);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Name", "Este rol ya existe");
                }
            }
            aspNetRole.permisosList = util.ObtenerPermisosList(aspNetRole.permisos);
            return View(aspNetRole);
        }

        private void AgregarPermisos(AspNetRoles roles)
        {
            AspNetRoles newRole = new AspNetRoles();
            List<string> permisosChecked = String.IsNullOrEmpty(roles.permisos) ? new List<string>() : roles.permisos.Split(',').ToList();
            foreach (string perm in permisosChecked)
            {
                Permissions permission = db.Permissions.Find(Int32.Parse(perm));
                newRole.Permissions.Add(permission);
            }

            var existingRole = db.AspNetRoles.Include("Permissions").Where(e => e.Id == roles.Id).FirstOrDefault<AspNetRoles>();
            existingRole = existingRole == null ? new AspNetRoles() : existingRole;

            var deletedPermisos = existingRole.Permissions.Except(newRole.Permissions).ToList<Permissions>();
            deletedPermisos.ForEach(c => existingRole.Permissions.Remove(c));

            var addedPermisos = newRole.Permissions.Except(existingRole.Permissions).ToList<Permissions>();

            foreach (Permissions r in addedPermisos)
            {
                if (db.Entry(r).State == EntityState.Detached)
                    db.Permissions.Attach(r);
                existingRole.Permissions.Add(r);
            }

            db.SaveChanges();
        }

        // GET: AspNetRoles/Edit/5
        [HasPermission("General_Edicion")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            aspNetRole.permisos = string.Join(",", aspNetRole.Permissions.Select(a => a.Id).Select(a => a.ToString()).ToArray());
            aspNetRole.permisosList = util.ObtenerPermisosList(aspNetRole.permisos);
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Afiliado,permisos")] AspNetRoles aspNetRole)
        {
            valid.ValidarPerfil(ModelState, aspNetRole);
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
                var role = await roleManager.FindByIdAsync(aspNetRole.Id);
                role.Name = aspNetRole.Name;
                role.Afiliado = aspNetRole.Afiliado;
                await roleManager.UpdateAsync(role);
                AgregarPermisos(aspNetRole);
                return RedirectToAction("Index");
            }
            aspNetRole.permisosList = util.ObtenerPermisosList(aspNetRole.permisos);
            return View(aspNetRole);
        }

        // GET: AspNetRoles/Delete/5
        [HasPermission("General_Edicion")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await serv.EliminarPerfil(id);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        
    }
}
