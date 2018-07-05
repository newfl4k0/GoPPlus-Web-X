using GoPS.Classes;
using GoPS.CustomFilters;
using GoPS.Models;
using GoPS.ViewModels;
using GoPS.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class AccountController : _GeneralController
    {
        
        #region Variables

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        #endregion
        
        #region Constructores

        public AccountController()
        {
            UserManager = null;
            RoleManager = null;
            SignInManager = null;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        #endregion
        
        #region Propiedades

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }

        #endregion

        [HasPermission("General_Visualizacion")]
        public ActionResult DashboardUser()
        {
            bool isSAdmin = Boolean.Parse(RouteData.Values["SAdministrador"].ToString());
            List<int> ID_Empresas = RouteData.Values["ID_Empresas"] as List<int>;
            ViewBag.MostrarEmpresas = ID_Empresas.Count() > 1 && isSAdmin;
            return View();
        }

        [HasPermission("General_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }        
            var Users = db.AspNetUsers.Include(c => c.Positions).Include(c => c.AspNetUserRoles);            
            return View(Users.ToList());
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                LogOff();
            }
            catch (HttpAntiForgeryException ex)
            {
                string str_error = ex.Message;
            }
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AspNetUsers getUser = db.AspNetUsers.Where(e => e.Email.ToLower().Trim() == model.Email.ToLower().Trim() || e.UserName.ToLower() == model.Email.ToLower().Trim()).SingleOrDefault();

            if (getUser == null)
            {
                ModelState.AddModelError("", "Intento de inicio de sesión inválido");
                return View(model);
            }
            //Add this to check if the email was confirmed.
            if (!getUser.EmailConfirmed)
            {
                ViewBag.ResendEmail = true;
                ModelState.AddModelError("", "Para habilitar tu usuario, debes confirmar tu correo electrónico. Si no recibiste el link, reenvíalo aquí:");
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(getUser.UserName, model.Password, model.RememberMe, shouldLockout: false);

            if (getUser != null && result.ToString().ToUpper() == "SUCCESS")
            {
                getUser.LastLoginDate = util.ConvertToMexicanDate(DateTime.Now); ;
                getUser.IsLoged_in = true;
                db.Entry(getUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            switch (result)
            {
                case SignInStatus.Success:
                    Session.Timeout = 20;
                    Session["band"] = "u";
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión fallido");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ResendEmail()
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendEmail(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.Email == model.Email);

                if (user != null)
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirmación de Cuenta");
                    ViewBag.Class = "text-success";
                    ModelState.AddModelError("", "Por favor, revise su correo");
                }
                else
                {
                    ModelState.AddModelError("", "El Email no existe");
                }
            }
            return View();
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código inválido");
                    return View(model);
            }
        }

        // check if Email Already exists
        [AllowAnonymous]
        public async Task<JsonResult> UserEmailAlreadyExistsAsync(string email)
        {
            var result = await UserManager.FindByEmailAsync(email);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }

        // check if username Already exists
        [AllowAnonymous]
        public async Task<JsonResult> UserUsernameAlreadyExistsAsync(string username)
        {
            var result = await UserManager.FindByNameAsync(username);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/Register
        [HasPermission("General_Edicion")]
        public ActionResult Register()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            string ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault().ToString() : "";
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(o => o.Position), "Id", "Position");
            RegisterViewModel model = new RegisterViewModel();
            model.rolesList = util.ObtenerRadioButtonRolesList(model.role, User.Identity.GetUserId());
            model.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(ID_Afiliado, ID_Afiliados, User.Identity.GetUserId());
            model.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(ID_Afiliado, ID_Afiliados, User.Identity.GetUserId());
            return View(model);
        }

        private string GetFileName(RegisterViewModel model, HttpPostedFileBase PicturePath)
        {
            string filename = "";
            bool edit = !String.IsNullOrEmpty(model.Id);
            filename = util.SaveOrMoveFile(model.PicturePath, PicturePath, edit);
            return filename;
        }

        private void GetTempFileName(RegisterViewModel model, HttpPostedFileBase PicturePath)
        {
            bool edit = !String.IsNullOrEmpty(model.Id);
            string filename = model.PicturePath;
            int sizefile = !String.IsNullOrEmpty(filename) && edit ? 1000 : model.Peso_PicturePath;
            ModelState.Remove("PicturePath");
            ModelState.Remove("Peso_PicturePath");
            model.PicturePath = util.SaveNewTempFile(filename, PicturePath, edit);
            model.Peso_PicturePath = PicturePath != null ? PicturePath.ContentLength : (sizefile);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase File_PicturePath)
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(o => o.Position), "Id", "Position", model.PositionID);
            ClearErrors(model, File_PicturePath != null);
            bool isRolPic = util.RolNeedsPicture(model.role);

            if (isRolPic)
                GetTempFileName(model, File_PicturePath);
            valid.ValidarUser(ModelState, model, isRolPic);
            if (ModelState.IsValid)
            {
                AspNetUsers checkUserEmail = db.AspNetUsers.Where(e => e.Email == model.Email).SingleOrDefault();

                if (checkUserEmail != null)
                {
                    ModelState.AddModelError("Email", "El Email ya existe");
                    model.rolesList = util.ObtenerRadioButtonRolesList(model.role, User.Identity.GetUserId());
                    model.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
                    model.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
                    return View(model);
                }

                AspNetUsers checkUserUsername = db.AspNetUsers.Where(e => e.UserName == model.UserName).SingleOrDefault();

                if (checkUserUsername != null)
                {
                    ModelState.AddModelError("Username", "El Nombre de Usuario ya existe");
                    model.rolesList = util.ObtenerRadioButtonRolesList(model.role, User.Identity.GetUserId());
                    model.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
                    model.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
                    return View(model);
                }

                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                user.PhoneNumber = model.PhoneNumber;
                user.PositionID = model.PositionID;
                user.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
                user.IsLoged_in = false;
                
                user.PicturePath = isRolPic ? GetFileName(model, File_PicturePath) : "";
                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                   try
                    {
                        // send email
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Account confirmation");

                    }
                    catch (Exception)
                    {
                        ViewBag.Success = false;
                    }

                    AspNetUsers getUser = db.AspNetUsers.Where(e => e.Email == model.Email).SingleOrDefault();

                    if (getUser != null)
                    {
                        await AgregarRoles(model.role, model.afiliados, getUser);
                        ViewBag.Success = true;
                    }
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            model.rolesList = util.ObtenerRadioButtonRolesList(model.role, User.Identity.GetUserId());
            model.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
            model.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(model.afiliados, ID_Afiliados, User.Identity.GetUserId());
            return View(model);
        }

        public JsonResult ShowPictureAndAfiliados(string role, bool mostrar)
        {
            var showPic = util.RolNeedsPicture(role);
            var showAfi = !mostrar ? false :
                            (String.IsNullOrEmpty(role) ? false : db.AspNetRoles.Find(role).Afiliado);
            var radioButtons = showAfi ? (db.AspNetRoles.Find(role).Name.ToUpper() == "CENTRAL") : false;
            return Json(new { showPic, showAfi, radioButtons }, JsonRequestBehavior.AllowGet);
        }

        public async Task DeleteRolesAsync(List<string> deleteList, string userId)
        {
            foreach (var roleName in deleteList)
            {
                IdentityResult deletionResult = await UserManager.RemoveFromRoleAsync(userId, roleName);
            }
        }

        private async Task AgregarRoles(string roleID, string afiliados, AspNetUsers getUser)
        {
            AspNetRoles role = db.AspNetRoles.Find(roleID);

            if (!UserManager.IsInRole(getUser.Id, role.Name))
            {
                AspNetUserRoles userRole = getUser.AspNetUserRoles.FirstOrDefault();
                string roleDelete = userRole == null ? "" : userRole.AspNetRoles.Name;

                if (!String.IsNullOrEmpty(roleDelete))
                {
                    if (userRole.AspNetRoles.Afiliado)
                    {
                        List<Afiliados> afiliadosDelete = userRole.Afiliados.ToList();
                        afiliadosDelete.ForEach(c => userRole.Afiliados.Remove(c));
                        db.SaveChanges();
                    }
                    await UserManager.RemoveFromRoleAsync(getUser.Id, roleDelete);
                }
                var roleresult = UserManager.AddToRole(getUser.Id, role.Name);
            }
            if (role.Afiliado)
            {
                object[] id = new object[] { roleID, getUser.Id };
                AspNetUserRoles userRole = db.AspNetUserRoles.Find(id);
                AgregarAfiliados(userRole, afiliados);
            }
        }

        private void AgregarAfiliados(AspNetUserRoles userRole, string afiliados)
        {
            AspNetUserRoles newUserRole = new AspNetUserRoles();
            List<int> afiliadosChecked = String.IsNullOrEmpty(afiliados) ? new List<int>() : afiliados.Split(',').Select(Int32.Parse).ToList();

            foreach (int ID_Afiliado in afiliadosChecked)
            {
                Afiliados afi = db.Afiliados.Find(ID_Afiliado);
                newUserRole.Afiliados.Add(afi);
            }
            var existingUserRole = db.AspNetUserRoles.Include("Afiliados").Where(e => e.RoleId == userRole.RoleId && e.UserId == userRole.UserId).FirstOrDefault<AspNetUserRoles>();
            existingUserRole = existingUserRole == null ? new AspNetUserRoles() : existingUserRole;
            var deletedAfiliados = existingUserRole.Afiliados.Except(newUserRole.Afiliados).ToList<Afiliados>();
            deletedAfiliados.ForEach(c => existingUserRole.Afiliados.Remove(c));
            var addedAfiliados = newUserRole.Afiliados.Except(existingUserRole.Afiliados).ToList<Afiliados>();

            foreach (Afiliados a in addedAfiliados)
            {
                if (db.Entry(a).State == EntityState.Detached)
                    db.Afiliados.Attach(a);
                existingUserRole.Afiliados.Add(a);
            }
            db.SaveChanges();
        }

        
        [HasPermission("General_Edicion")]
        [EncryptedActionParameter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUsers aspNetUser = db.AspNetUsers.Where(m => m.newId==id).FirstOrDefault();

            if (aspNetUser == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatAccount";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }

            var UserDB = (from m in db.AspNetUsers
                          where m.newId == id
                          select new RegisterViewModel
                          {
                              Email = m.Email,
                              Password = m.PasswordHash,
                              UserName = m.UserName,
                              PhoneNumber = m.PhoneNumber,
                              PositionID = m.PositionID,
                              DateOfBirth = m.DateOfBirth,
                              PicturePath = m.PicturePath,
                              Id = m.Id,
                              EmailConfirmed = m.EmailConfirmed,
                              PasswordHash = m.PasswordHash,
                              SecurityStamp = m.SecurityStamp,
                              PhoneNumberConfirmed = m.PhoneNumberConfirmed,
                              TwoFactorEnabled = m.TwoFactorEnabled,
                              LockoutEndDateUtc = m.LockoutEndDateUtc,
                              LockoutEnabled = m.LockoutEnabled,
                              AccessFailedCount = m.AccessFailedCount
                          }).FirstOrDefault();
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(o => o.Position), "Id", "Position", UserDB.PositionID);
            UserDB.role = string.Join(",", aspNetUser.AspNetUserRoles.Select(p => p.AspNetRoles).Select(a => a.Id).ToArray());
            UserDB.rolesList = util.ObtenerRadioButtonRolesList(UserDB.role, User.Identity.GetUserId());
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            UserDB.afiliados = string.Join(",", aspNetUser.AspNetUserRoles.SelectMany(p => p.Afiliados).Select(a => a.ID_Afiliado).ToArray());
            UserDB.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(UserDB.afiliados, ID_Afiliados, User.Identity.GetUserId());
            UserDB.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(UserDB.afiliados, ID_Afiliados, User.Identity.GetUserId());
            return View(UserDB);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> Edit(RegisterViewModel UsserEdit, HttpPostedFileBase File_PicturePath)
        {
            ModelState["Password"].Errors.Clear(); ModelState["ConfirmPassword"].Errors.Clear();
            ClearErrors(UsserEdit, File_PicturePath != null);

            bool isRolPic = util.RolNeedsPicture(UsserEdit.role);

            if (isRolPic)
                GetTempFileName(UsserEdit, File_PicturePath);
            valid.ValidarUser(ModelState, UsserEdit, isRolPic);
            if (ModelState.IsValid)
            {
                AspNetUsers getUser = db.AspNetUsers.Where(e => e.Email == UsserEdit.Email).SingleOrDefault();
                getUser.Id = UsserEdit.Id;
                getUser.PicturePath = isRolPic ? GetFileName(UsserEdit, File_PicturePath) : "";
                getUser.PasswordHash = UsserEdit.PasswordHash;
                getUser.SecurityStamp = UsserEdit.SecurityStamp;
                getUser.Email = UsserEdit.Email;
                getUser.UserName = UsserEdit.UserName;
                getUser.EmailConfirmed = UsserEdit.EmailConfirmed;
                getUser.PhoneNumber = UsserEdit.PhoneNumber;
                getUser.PhoneNumberConfirmed = UsserEdit.PhoneNumberConfirmed;
                getUser.TwoFactorEnabled = UsserEdit.TwoFactorEnabled;
                getUser.LockoutEndDateUtc = UsserEdit.LockoutEndDateUtc;
                getUser.LockoutEnabled = UsserEdit.LockoutEnabled;
                getUser.AccessFailedCount = UsserEdit.AccessFailedCount;
                getUser.PositionID = UsserEdit.PositionID;
                getUser.DateOfBirth = Convert.ToDateTime(UsserEdit.DateOfBirth);
                db.Entry(getUser).State = EntityState.Modified;
                db.SaveChanges();
                await AgregarRoles(UsserEdit.role, UsserEdit.afiliados, getUser);
                return RedirectToAction("Index");
            }
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(o => o.Position), "Id", "Position", UsserEdit.PositionID);
            UsserEdit.rolesList = util.ObtenerRadioButtonRolesList(UsserEdit.role, User.Identity.GetUserId());
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            UsserEdit.afiliadosListChecks = util.ObtenerCheckBoxAfiliadosList(UsserEdit.afiliados, ID_Afiliados, User.Identity.GetUserId());
            UsserEdit.afiliadosListRadios = util.ObtenerRadioButtonAfiliadosList(UsserEdit.afiliados, ID_Afiliados, User.Identity.GetUserId());
            return View(UsserEdit);
        }

        private void ClearErrors(RegisterViewModel UserEdit, bool picture)
        {
            bool isRolPic = util.RolNeedsPicture(UserEdit.role);

            if (!isRolPic || picture)
            {                
                ModelState["PicturePath"].Errors.Clear();
            }

            bool validateAfiliados = String.IsNullOrEmpty(UserEdit.role) ? false : db.AspNetRoles.Find(UserEdit.role).Afiliado;

            if (!validateAfiliados)
                ModelState["afiliados"].Errors.Clear();

            bool isRolPos = util.RolNeedsPosition(UserEdit.role);

            if (!isRolPos)
                ModelState["PositionID"].Errors.Clear();
        }

        [HasPermission("General_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUsers aspNetUser = db.AspNetUsers.Where(m=>m.newId==id).FirstOrDefault();

            if (aspNetUser == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatAccount";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.role = aspNetUser.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name;
            string[] afArr = aspNetUser.afiliados.Split(Convert.ToChar(","));
            List<string> afList = afArr.ToList();
            List<string> afList2 = new List<string>();
            int x = 1;
            foreach (var item in afList)
            {
                    if (!(item.ToString()=="" || item==null))
                    {
                        afList2.Add(db.Afiliados.Find(Convert.ToInt32(item.ToString())).Nombre);
                    }
                
                x = x++;
            }
            ViewBag.afiliados = afList2;
            return View(aspNetUser);
        }

        [HasPermission("General_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUsers aspNetUser = db.AspNetUsers.Where(m => m.newId == id).FirstOrDefault();

            if (aspNetUser == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatAccount";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.role = aspNetUser.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name;
            string[] afArr = aspNetUser.afiliados.Split(Convert.ToChar(","));
            List<string> afList = afArr.ToList();
            List<string> afList2 = new List<string>();
            int x = 1;
            foreach (var item in afList)
            {
                if (!(item.ToString() == "" || item == null))
                {
                    afList2.Add(db.Afiliados.Find(Convert.ToInt32(item.ToString())).Nombre);
                }

                x = x++;
            }
            ViewBag.afiliados = afList2;
            ViewBag.Mess = MensajeDelete;
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("General_Edicion")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string iid;
            iid= db.AspNetUsers.Where(m => m.newId == id).FirstOrDefault().Id;
            await serv.EliminarUsuario(iid);
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        //[HasPermission("Perfil")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            var IsConfirmed = ConfirmUserEmail(userId);
            AspNetUsers use = db.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();
            if (use.AspNetUserRoles.FirstOrDefault().RoleId != "949b92e6-dc35-4d15-9a81-33c0f91e1f07")
            {
                return View(result.Succeeded && IsConfirmed ? "ConfirmEmail" : "Error");
            }
            else
            {
                return View(result.Succeeded && IsConfirmed ? "ConfirmEmailDriver" : "Error2");
            }


        }

        private bool ConfirmUserEmail(string UserId)
        {
            try
            {
                var user = db.AspNetUsers.Where(x => x.Id == UserId).SingleOrDefault();
                user.EmailConfirmed = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            subject = "Confirmar Acceso a la Plataforma GoPPlus";
            MailMessage correo = new MailMessage
            {
                Subject = subject
            };
            correo.To.Add(UserManager.GetEmail(userID));                   
            await UserManager.SendEmailAsync(userID, subject, "<p style='text-align: center;'><strong>Bienvenido</strong><strong>(a)</strong>&nbsp;<img src='https://gopplustest.azurewebsites.net/images/logoemail.png' alt='' width='136' height='98' /></p><p style='text-align: center;'> &nbsp;</p><p style='text-align: center;'><strong> Es necesario confirmar su correo electr&oacute;nico para ingresar por primera vez al sistema.</strong ></p><p style='text-align: center;'><strong>Por favor confirme su cuenta dando clic <a href=\"" + callbackUrl + "\">aquí</a></strong></p><p style='text-align: justify;'> &nbsp;</p><p><strong> &nbsp;</strong></p>");
            return callbackUrl;
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            ViewBag.Logo = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "LOGO").FirstOrDefault().Valor;
            ViewBag.Color = db.Configuraciones.Where(c => c.Atributo.ToUpper() == "COLOR").FirstOrDefault().Valor;

            var userId = await SignInManager.GetVerifiedUserIdAsync();

            if (userId == null)
            {
                return View("Error");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [RedirectOnError]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOff()
        {
            LogOut();
            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        private void LogOut()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = User.Identity.GetUserId().ToString();

                if (user != null)
                {
                    AspNetUsers getUser = db.AspNetUsers.Where(e => e.Id == user).SingleOrDefault();
                    getUser.LastLogoutDate = util.ConvertToMexicanDate(DateTime.Now);
                    getUser.IsLoged_in = false;                    
                    db.Entry(getUser).State = EntityState.Modified;
                    db.SaveChanges();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //System.Web.SessionState.HttpSessionState curSession = HttpContext.Current.Session;
                    Session["cerrar"] = "yes";
                    //curSession["active"] = "off";
                    Session.Abandon();
                }
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}