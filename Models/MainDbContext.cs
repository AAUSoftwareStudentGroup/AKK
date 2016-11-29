using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AKK.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        { }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            var colors = new Dictionary<string, Color> {
                {"Cyan", new Color(0, 200, 200)},
                {"Azure", new Color(1, 126, 255)},
                {"Blue", new Color(60, 60, 255)},
                {"Violet", new Color(27, 0, 255)},
                {"Magenta", new Color(200, 50, 200)},
                {"Rose", new Color(210, 25, 120)},
                {"Red", new Color(200, 30, 30)},
                {"Orange", new Color(255, 127, 00)},
                {"Yellow", new Color(220, 200, 30)},
                {"Light Green", new Color(110, 210, 20)},
                {"Green", new Color(20, 150, 20)},
                {"Black", new Color(0, 0, 0)},
                {"Brown", new Color(126, 54, 15)},
                {"Grey", new Color(92, 89, 89)},
                {"White", new Color(205, 205, 205)},
            };

            var grades = new Dictionary<string, Grade> {
                {"Blue", new Grade {Name = "Blue", Difficulty = 0, Color = new Color(33, 150, 254), Id = Guid.NewGuid(), Routes = new List<Route>() }},
                {"Green", new Grade {Name = "Green", Difficulty = 1, Color = new Color(67, 160, 71), Id = Guid.NewGuid(), Routes = new List<Route>() }},
                {"Red", new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80), Id = Guid.NewGuid(), Routes = new List<Route>() }},
                {"Black", new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97), Id = Guid.NewGuid(), Routes = new List<Route>() }},
                {"White", new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251), Id = Guid.NewGuid(), Routes = new List<Route>() }}
            };

            var members = new Dictionary<string, Member>
            {
                {"Anton", new Member {DisplayName = "Anton", Username = "anton123", Password = "123", IsAdmin = true}},
                {"Grunberg", new Member {DisplayName = "Grunberg", Username = "grunberg123", Password = "123", IsAdmin = true}},
                {"Jakobsen", new Member {DisplayName = "Jakobsen", Username = "jakobsen123", Password = "123", IsAdmin = true}}
            };

            var sections = new Dictionary<string, Section> {
                {"A", new Section{Name = "A"}},
                {"B", new Section{Name = "B"}},
                {"C", new Section{Name = "C"}},
                {"D", new Section{Name = "D"}},
            };

            var comments = new List<Comment>
            {
                new Comment {Message = "Dette er en kommentar", Member = members["Anton"]},
                new Comment {Message = "Dette er en anden kommentar", Member = members["Jakobsen"]},
                new Comment{Message = "Dette er en trejde kommentar", Member = members["Grunberg"]}
            };

            var routes = new List<Route>
            {
                new Route
                {
                    Grade = grades["Blue"],
                    Name = "2",
                    Section = sections["B"],
                    ColorOfHolds = colors["Brown"],
                    Member = members["Anton"],
                    Author = "Jane og Benny",
                    CreatedDate = new DateTime(2016, 04, 10),
                },
                new Route
                {
                    Grade = grades["Blue"],
                    Name = "34",
                    Section = sections["B"],
                    ColorOfHolds = colors["Blue"],
                    Member = members["Anton"],
                    Author = "Jens Christian",
                    CreatedDate = new DateTime(2016, 04, 10),
                    Note = "Børneproblem"
                },
                new Route
                {
                    Grade = grades["Blue"],
                    Name = "38",
                    Section = sections["D"],
                    ColorOfHolds = colors["Brown"],
                    Member = members["Anton"],
                    Author = "Mette og Maiken",
                    CreatedDate = new DateTime(2016, 04, 10),
                    Note = "Twist & Shout",
                },
                new Route
                {
                    Grade = grades["Blue"],
                    Name = "11",
                    Section = sections["A"],
                    ColorOfHolds = colors["Yellow"],
                    Member = members["Anton"],
                    Author = "Foss",
                    CreatedDate = new DateTime(2016, 7, 3),
                },
            };

            context.Grades.AddRange(grades.Select(x => x.Value));
            context.Routes.AddRange(routes);
            context.Sections.AddRange(sections.Select(x => x.Value));
            context.Members.AddRange(members.Select(x => x.Value));

            //// save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}