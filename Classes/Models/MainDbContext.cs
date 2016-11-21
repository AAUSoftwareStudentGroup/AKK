using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>().HasOne(r => r.Section)
                                        .WithMany(s => s.Routes)
                                        .HasForeignKey(r => r.SectionId)
                                        .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Grade>().HasMany(g => g.Routes)
                                        .WithOne(r => r.Grade)
                                        .HasForeignKey(r => r.GradeId)
                                        .OnDelete(DeleteBehavior.SetNull);
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
            List<Grade> grades = new List<Grade> {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67,160,71), GradeId = new Guid()},
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33,150,254), GradeId = new Guid()},
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228,83,80), GradeId = new Guid()},
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97,97,97), GradeId = new Guid()},
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251,251,251), GradeId = new Guid()},
            };

            Section sectionA = new Section { SectionId = new Guid(), Name = "A" };
            List<Route> routesForA = new List<Route> {
                new Route{Name = "4", ColorOfHolds = new Color(255, 0, 0), Author = "Anton", Grade = grades[0], CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "14", ColorOfHolds = new Color(0, 255, 0), Author = "Jakobsen", Grade = grades[1], CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "43", ColorOfHolds = new Color(255, 0, 255), Author = "Hornum", Grade = grades[2], CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "21", ColorOfHolds = new Color(255, 255, 0), Author = "Jakob", Grade = grades[3], CreatedDate = new DateTime(2016, 03, 24)} };
            sectionA.Routes.AddRange(routesForA);
            context.Sections.Add(sectionA);

            Section sectionB = new Section { SectionId = new Guid(), Name = "B" };
            List<Route> routesForB = new List<Route> {
                new Route{Name = "32", ColorOfHolds =  new Color(100, 100, 100), Author = "TannerHelland", Grade = grades[4], CreatedDate = new DateTime(2014, 11, 24)},
                new Route{Name = "99", ColorOfHolds =  new Color(170, 12, 54), Author = "Grunberg", Grade = grades[2], CreatedDate = new DateTime(2016, 01, 02)},
                new Route{Name = "3", ColorOfHolds =  new Color(255, 34, 89), Author = "Ibsen", Grade = grades[3], CreatedDate = new DateTime(2016, 04, 11)},
                new Route{Name = "7", ColorOfHolds =  new Color(232, 233, 5), Author = "Anton", Grade = grades[3], CreatedDate = new DateTime(2016, 08, 10)} };
            sectionB.Routes.AddRange(routesForB);
            context.Sections.Add(sectionB);

            Section sectionC = new Section { SectionId = new Guid(), Name = "C" };
            List<Route> routesForC = new List<Route> {
                new Route{Name = "66", ColorOfHolds =  new Color(255, 0, 0), Author = "Geo", Grade = grades[0], CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "33", ColorOfHolds =  new Color(0, 22, 123), Author = "Bacci", Grade = grades[1], CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "22", ColorOfHolds =  new Color(255, 123, 0), Author = "Kurt", Grade = grades[1], CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "44", ColorOfHolds =  new Color(123, 22, 22), Author = "Benja", Grade = grades[2], CreatedDate = new DateTime(2016, 03, 24)} };
            sectionC.Routes.AddRange(routesForC);
            context.Sections.Add(sectionC);

            Section sectionD = new Section { SectionId = new Guid(), Name = "D" };
            List<Route> routesForD = new List<Route> {
                new Route{Name = "20", ColorOfHolds = new Color(35, 0, 22), Author = "Manfred", Grade = grades[1], CreatedDate = new DateTime(2016, 03, 01), ColorOfTape = new Color(123,255,22)},
                new Route{Name = "9", ColorOfHolds = new Color(123, 255, 22), Author = "Bettina", Grade = grades[0], CreatedDate = new DateTime(2016, 10, 27)},
                new Route{Name = "76", ColorOfHolds = new Color(0, 22, 68), Author = "Kasper", Grade = grades[0], CreatedDate = new DateTime(2016, 09, 04)},
                new Route{Name = "54", ColorOfHolds = new Color(123, 22, 123), Author = "Rasmus", Grade = grades[4], CreatedDate = new DateTime(2016, 06, 22)} };
            sectionD.Routes.AddRange(routesForD);
            context.Sections.Add(sectionD);

            //// save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}