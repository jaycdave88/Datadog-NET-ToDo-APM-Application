namespace Datadog_MVC_ToDo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Datadog_MVC_ToDo.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Datadog_MVC_ToDo.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Datadog_MVC_ToDo.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            AddUsers(context);
        }

        void AddUsers(Datadog_MVC_ToDo.Models.ApplicationDbContext context)
        {
            var user = new ApplicationUser { UserName = "user1@email.com" };
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.Create(user, "password");
        }
    }
}
