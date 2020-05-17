namespace HelloPlayerMVC.Migrations
{
    using HelloPlayerMVC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HelloPlayerMVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HelloPlayerMVC.Models.ApplicationDbContext";
        }

        protected override void Seed(HelloPlayerMVC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            const string admin = "admin@helloplayer.com";
            const string adminPassword = "Dope2112";
            Logic.PlayerManager playerMgr = new Logic.PlayerManager();
            var statuses = playerMgr.GetPlayerStatuses();
            foreach (var status in statuses)
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = status });
            }
            if (!statuses.Contains("Administrator"))
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Administrator" });
            }
            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                    GivenName = "Admin",
                    FamilyName = "HelloPlayer",
                    HelloPlayerUserName = "Admin"
                };
                IdentityResult result = userManager.Create(user, adminPassword);
                context.SaveChanges();
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                    context.SaveChanges();
                }
            }
        }
    }
}
