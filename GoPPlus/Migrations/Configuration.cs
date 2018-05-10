namespace GoPS.Migrations
{
    using GoPS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GoPS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GoPS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedRoles(context);

            SeedUsers(context);

        }

        private static void SeedUsers(GoPS.Models.ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            if (!context.Users.Any(u => u.UserName == "m.ale3190"))
            {
                var user = new ApplicationUser { UserName = "m.ale3190" };
                manager.Create(user, "Speqtrum2017++");
                manager.AddToRole(user.Id, "SADMINISTRADOR");
            }

            if (!context.Users.Any(u => u.UserName == "ctorres@speqtrum.com"))
            {
                var user = new ApplicationUser { UserName = "ctorres@speqtrum.com" };
                manager.Create(user, "Speqtrum2017++");
                manager.AddToRole(user.Id, "SADMINISTRADOR");
            }

            if (!context.Users.Any(u => u.UserName == "at_he_na16@hotmail.com"))
            {
                var user = new ApplicationUser { UserName = "at_he_na16@hotmail.com" };
                manager.Create(user, "Speqtrum2017++");
                manager.AddToRole(user.Id, "SADMINISTRADOR");
            }
        }

        private static void SeedRoles(GoPS.Models.ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);

            if (!context.Roles.Any(r => r.Name.ToUpper() == "SADMINISTRADOR"))
            {
                var role = new IdentityRole { Name = "SADMINISTRADOR" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name.ToUpper() == "ADMINISTRADOR"))
            {
                var role = new IdentityRole { Name = "ADMINISTRADOR" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name.ToUpper() == "CENTRAL"))
            {
                var role = new IdentityRole { Name = "CENTRAL" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name.ToUpper() == "DRIVER"))
            {
                var role = new IdentityRole { Name = "DRIVER" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name.ToUpper() == "GUEST"))
            {
                var role = new IdentityRole { Name = "GUEST" };
                manager.Create(role);
            }
        }

    }
}
