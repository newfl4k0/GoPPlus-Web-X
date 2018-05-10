using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System;
using System.Collections.Generic;

namespace GoPS.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name, bool afiliado)
            : base(name)
        {
            this.Afiliado = afiliado;
        }

        public virtual bool Afiliado { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole()
            : base()
        { }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public System.DateTime DateOfBirth { get; set; }
        public string PicturePath { get; set; }
        public System.Nullable<int> PositionID { get; set; }
        public Boolean IsLoged_in { get; set; }

        //public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            ////Defining the keys and relations
            //modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            //modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            //modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            //modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        }

        //public bool RoleExists(ApplicationRoleManager roleManager, string name)
        //{
        //    return roleManager.RoleExists(name);
        //}

        //public bool CreateRole(ApplicationRoleManager _roleManager, string name, bool afiliado = false)
        //{
        //    var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, afiliado));
        //    return idResult.Succeeded;
        //}

        //public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        //{
        //    var idResult = _userManager.AddToRole(userId, roleName);
        //    return idResult.Succeeded;
        //}

        //public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        //{
        //    var user = userManager.FindById(userId);
        //    var currentRoles = new List<IdentityUserRole>();

        //    currentRoles.AddRange(user.UserRoles);
        //    foreach (ApplicationUserRole role in currentRoles)
        //    {
        //        userManager.RemoveFromRole(userId, role.Role.Name);
        //    }
        //}

        //public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        //{
        //    userManager.RemoveFromRole(userId, roleName);
        //}

        //public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        //{
        //    var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
        //    var role = context.Roles.Find(roleId);

        //    foreach (var user in roleUsers)
        //    {
        //        this.RemoveFromRole(userManager, user.Id, role.Name);
        //    }
        //    context.Roles.Remove(role);
        //    context.SaveChanges();
        //}
    }

}