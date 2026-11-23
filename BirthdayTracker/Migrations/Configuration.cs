namespace BirthdayTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BirthdayTracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BirthdayTracker.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BirthdayTracker.Data.ApplicationDbContext context)
        {
            // AddOrUpdate matches on EmployeeNumber, so running this again
            // will update the existing row instead of adding a duplicate.
            context.Users.AddOrUpdate(
                u => u.EmployeeNumber,
                new User
                {
                    EmployeeNumber = "1001",
                    Password = "admin123",
                    Role = "Admin"
                },
                new User
                {
                    EmployeeNumber = "1002",
                    Password = "user123",
                    Role = "Employee"
                }
            );

            context.SaveChanges();
        }
    }
}