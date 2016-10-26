using Microsoft.EntityFrameworkCore;
using AKK.Models;
using System.Collections.Generic;
using System;

namespace AKK.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {}

        public DbSet<Route> Routes { get; set; }
        public DbSet<Section> Sections { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Route>().HasOne(r => r.Section)
                                        .WithMany(s => s.Routes)
                                        .HasForeignKey(r => r.SectionID);
        }
    }
    public static class DbContextExtensions
    {
        public static void Seed(this MainDbContext context)
        {
            // Perform database delete and create
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Perform seed operations
            Section s = new Section {Name = "A"};
            List<Route> routes = new List<Route>();
            routes.Add(new Route {
                ID = Guid.NewGuid(),
                Name = "1",
                Author = "Mathias Hornumm",
                Date = DateTime.Now,
                Grade = Grades.Green,
                ColorOfHolds = 0x000000FF
            });
            routes.Add(new Route {
                ID = Guid.NewGuid(),
                Name = "2",
                Author = "Mathias Hornumm",
                Date = DateTime.Now,
                Grade = Grades.Blue,
                ColorOfHolds = 0xFF0000FF
            });
            routes.Add(new Route {
                ID = Guid.NewGuid(),
                Name = "3",
                Author = "Mathias Jakobsen",
                Date = DateTime.Now,
                Grade = Grades.White,
                ColorOfHolds = 0x00FF00FF
            });

            s.Routes.AddRange(routes);
            context.Sections.Add(s);
            
            // Save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}