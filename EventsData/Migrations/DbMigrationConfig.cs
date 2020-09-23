namespace EventsData.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class DbMigrationConfig : DbMigrationsConfiguration<EventsData.ApplicationDbContext>
    {
        public DbMigrationConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EventsData.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFullName = "System Administrator";
                var adminPassword = adminEmail;
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateSeveralEvents(context);
            }
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            //create admin
            var adminUser = new ApplicationUser()
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
            //create administrator role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
            //add admin user to administrator role
            var adminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!adminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", adminRoleResult.Errors));
            }
        }

        private void CreateSeveralEvents(ApplicationDbContext context)
        {
            context.Events.Add(new Event()
            {
                Title = "Finally some party :)",
                StartDateTime = DateTime.Now.AddDays(4).AddHours(2).AddMinutes(59),
                Author = context.Users.First(),
            });

            context.Events.Add(new Event()
            {
                Title = "Passed event anonymous",
                StartDateTime = DateTime.Now.AddDays(-3).AddHours(2).AddMinutes(59),
                Duration = TimeSpan.FromHours(2),
                Comments = new HashSet<Comment>()
                {
                    new Comment(){Text = "New Comment anonymous one"},
                    new Comment(){Text="User comment", Author=context.Users.First()}
                }
            });

            context.Events.Add(new Event()
            {
                Title = "New Event 1",
                StartDateTime = DateTime.Now.AddDays(30).AddHours(2).AddMinutes(59),
                Author = context.Users.First(),
                Comments = new HashSet<Comment>() {
                    new Comment() { Text ="Comment 1 in event 1"},
                    new Comment() { Text ="Comment 2 in event 1"},
                    new Comment() { Text ="Comment 3 in event 1"},
                    new Comment() { Text ="Comment 4 in event 1"},}
            });
        }
    }
}
