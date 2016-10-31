using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AKK.Classes.Models;
using System.Collections.Generic;
using System;

namespace AKK.Classes.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        { }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>().HasOne(r => r.Section)
                                        .WithMany(s => s.Routes)
                                        .HasForeignKey(r => r.SectionId)
                                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public static class DbContextExtensions
    {
        public static void Seed(this MainDbContext context)
        {
            if (Environment.GetEnvironmentVariable("ASPNET_DATABASE_NOSEED") != null)
            {
                return;
            }
            // Perform database delete and create
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Adds first section including routes:

            Section sectionA = new Section { SectionId = new Guid(), Name = "A" };
            List<Route> routesForA = new List<Route> {
                new Route{Name = "4", ColorOfHolds = 0x0000FFFF, Author = "Anton", Grade = Grades.Blue, CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "14", ColorOfHolds = 0xAAAAFFFF, Author = "Jakobsen", Grade = Grades.Red, CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "43", ColorOfHolds = 0x5221FFFF, Author = "Hornum", Grade = Grades.White, CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "21", ColorOfHolds = 0x8654FFFF, Author = "Jakob", Grade = Grades.Blue, CreatedDate = new DateTime(2016, 03, 24)} };
            sectionA.Routes.AddRange(routesForA);
            context.Sections.Add(sectionA);

            Section sectionB = new Section { SectionId = new Guid(), Name = "B" };
            List<Route> routesForB = new List<Route> {
                new Route{Name = "32", ColorOfHolds = 0x4444FFFF, Author = "TannerHelland", Grade = Grades.Red, CreatedDate = new DateTime(2014, 11, 24)},
                new Route{Name = "99", ColorOfHolds = 0x5432FFFF, Author = "Grunberg", Grade = Grades.Red, CreatedDate = new DateTime(2016, 01, 02)},
                new Route{Name = "3", ColorOfHolds = 0xBFAAFFFF, Author = "Ibsen", Grade = Grades.Green, CreatedDate = new DateTime(2016, 04, 11)},
                new Route{Name = "7", ColorOfHolds = 0xCCCCFFFF, Author = "Anton", Grade = Grades.Black, CreatedDate = new DateTime(2016, 08, 10)} };
            sectionB.Routes.AddRange(routesForB);
            context.Sections.Add(sectionB);

            Section sectionC = new Section { SectionId = new Guid(), Name = "C" };
            List<Route> routesForC = new List<Route> {
                new Route{Name = "66", ColorOfHolds = 0x9465FFFF, Author = "Geo", Grade = Grades.Green, CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "33", ColorOfHolds = 0x4D4DFFFF, Author = "Bacci", Grade = Grades.Green, CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "22", ColorOfHolds = 0x1A1AFFFF, Author = "Kurt", Grade = Grades.White, CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "44", ColorOfHolds = 0x7B7AFFFF, Author = "Benja", Grade = Grades.Red, CreatedDate = new DateTime(2016, 03, 24)} };
            sectionC.Routes.AddRange(routesForC);
            context.Sections.Add(sectionC);

            Section sectionD = new Section { SectionId = new Guid(), Name = "D" };
            List<Route> routesForD = new List<Route> {
                new Route{Name = "20", ColorOfHolds = 0x0000FFFF, Author = "Manfred", Grade = Grades.Red, CreatedDate = new DateTime(2016, 03, 01)},
                new Route{Name = "9", ColorOfHolds = 0xAAAAFFFF, Author = "Bettina", Grade = Grades.Green, CreatedDate = new DateTime(2016, 10, 27)},
                new Route{Name = "76", ColorOfHolds = 0x5221FFFF, Author = "Kasper", Grade = Grades.White, CreatedDate = new DateTime(2016, 09, 04)},
                new Route{Name = "54", ColorOfHolds = 0x8654FFFF, Author = "Rasmus", Grade = Grades.Blue, CreatedDate = new DateTime(2016, 06, 22)} };
            sectionD.Routes.AddRange(routesForD);
            context.Sections.Add(sectionD);

            //// save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}