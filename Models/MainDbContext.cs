using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AKK.Models
{
    public class MainDbContext:DbContext
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

            List<Image> images = new List<Image> {
                new Image {Id = new Guid(), Width = 800, Height = 500, FileUrl = "https://placeholdit.imgix.net/~text?txtsize=28&txt=500%C3%97800&w=500&h=800"}
            };

            List<Hold> holds = new List<Hold> {
                new Hold {Id = new Guid(), ImageId = images[0].Id, X = 0.5, Y = 0.5, Radius = 0.1}
            };

            images[0].Holds.AddRange(holds);

            var _members = new List<Member>
            {
                new Member {DisplayName = "Anton"},
                new Member {DisplayName = "Jakobsen"},
                new Member {DisplayName = "Hornum"},
                new Member {DisplayName = "Jakob"},
                new Member {DisplayName = "TannerHelland"},
                new Member {DisplayName = "Grunberg"},
                new Member {DisplayName = "Ibsen"},
                new Member {DisplayName = "Geo"},
                new Member {DisplayName = "Bacci"},
                new Member {DisplayName = "Geogebra"},
                new Member {DisplayName = "Kurt"},
                new Member {DisplayName = "Benja"},
                new Member {DisplayName = "Manfred"},
                new Member {DisplayName = "Betinna"},
                new Member {DisplayName = "Kasper"},
                new Member {DisplayName = "Rasmus"}
            };

            var _sections = new List<Section>
            {
                new Section {Name = "A", Id = Guid.NewGuid()},
                new Section {Name = "B", Id = Guid.NewGuid()},
                new Section {Name = "C", Id = Guid.NewGuid()},
                new Section {Name = "D", Id = Guid.NewGuid()},
                new Section {Name = "E", Id = Guid.NewGuid()},
                new Section {Name = "G", Id = Guid.NewGuid()},
                new Section {Name = "H", Id = Guid.NewGuid()},
                new Section {Name = "I", Id = Guid.NewGuid()},
                new Section {Name = "J", Id = Guid.NewGuid()},
                new Section {Name = "K", Id = Guid.NewGuid()},
                new Section {Name = "L", Id = Guid.NewGuid()},
            };

            var _grades = new List<Grade>
            {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67, 160, 71), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33, 150, 254), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251), Id = Guid.NewGuid(), Routes = new List<Route>() }, 
                new Grade {Name = "Purple", Difficulty = 5, Color = new Color(128, 0, 128), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Pink", Difficulty = 6, Color = new Color(255, 192, 203), Id = Guid.NewGuid(), Routes = new List<Route>() }
            };

            var _routes = new List<Route>
            {
                new Route
                {
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = true},
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Grunberg", Username = "Grunberg123", Password = "123", IsAdmin = true},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "14",
                    Section = _sections[0],
                    ColorOfHolds = new Color(0, 255, 0),
                    Member = new Member {DisplayName = "Jakobsen", Username = "Jakobsen123", Password = "123", IsAdmin = false},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "43",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 255),
                    Member = new Member {DisplayName = "Hornum", Username = "Hornum123", Password = "123", IsAdmin = false},
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "21",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 255, 0),
                    Member = new Member {DisplayName = "Jakob", Username = "Jakob123", Password = "123", IsAdmin = false},
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "32",
                    Section = _sections[1],
                    ColorOfHolds = new Color(100, 100, 100),
                    Member = new Member {DisplayName = "TannerHelland", Username = "TannerHelland123", Password = "123", IsAdmin = true},
                    Grade = _grades[4],
                    CreatedDate = new DateTime(2014, 11, 24)
                },
                new Route
                {
                    Name = "99",
                    Section = _sections[1],
                    ColorOfHolds = new Color(170, 12, 54),
                    Member = new Member {DisplayName = "Grunberg", Username = "Grunberg123", Password = "123", IsAdmin = false},
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 01, 02)
                },
                new Route
                {
                    Name = "3",
                    Section = _sections[1],
                    ColorOfHolds = new Color(255, 34, 89),
                    Member = new Member {DisplayName = "Ibsen", Username = "Ibsen123", Password = "123", IsAdmin = false},
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 04, 11)
                },
                new Route
                {
                    Name = "7",
                    Section = _sections[1],
                    ColorOfHolds = new Color(232, 233, 5),
                    Member = new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = false},
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 08, 10)
                },
                new Route
                {
                    Name = "66",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Geo", Username = "Geo123", Password = "123", IsAdmin = false},
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "33",
                    Section = _sections[2],
                    ColorOfHolds = new Color(0, 22, 123),
                    Member = new Member {DisplayName = "Bacci", Username = "Bacci123", Password = "123", IsAdmin = false},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "94",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = new Member {DisplayName = "Geogebra", Username = "Geogebra123", Password = "123", IsAdmin = false},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "22",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = new Member {DisplayName = "Kurt", Username = "Kurt123", Password = "123", IsAdmin = false},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "44",
                    Section = _sections[2],
                    ColorOfHolds = new Color(123, 22, 22),
                    Member = new Member {DisplayName = "Benja", Username = "Benja123", Password = "123", IsAdmin = false},
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "20",
                    Section = _sections[3],
                    ColorOfHolds = new Color(35, 0, 22),
                    Member = new Member {DisplayName = "Manfred", Username = "Manfred123", Password = "123", IsAdmin = false},
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 03, 01),
                    ColorOfTape = new Color(123, 255, 22)
                },
                new Route
                {
                    Name = "9",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 255, 22),
                    Member = new Member {DisplayName = "Bettina", Username = "Bettina123", Password = "123", IsAdmin = false},
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 10, 27)
                },
                new Route
                {
                    Name = "76",
                    Section = _sections[3],
                    ColorOfHolds = new Color(0, 22, 68),
                    Member = new Member {DisplayName = "Kasper", Username = "Kasper123", Password = "123", IsAdmin = false},
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 09, 04)
                },
                new Route
                {
                    Name = "54",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 22, 123),
                    Member = new Member {DisplayName = "Rasmus", Username = "Rasmus123", Password = "123", IsAdmin = false},
                    Grade = _grades[4],
                    CreatedDate = new DateTime(2016, 06, 22)
                }
            };

            foreach (var section in _sections)
            {
                section.Routes.AddRange(_routes.Where(r => r.Section.Id == section.Id));
            }

            foreach (var grade in _grades)
            {
                grade.Routes.AddRange(_routes.Where(r => r.Grade.Id == grade.Id));
            }

            context.Sections.AddRange(_sections);

            /*
            List<Member> members = new List<Member> {
                new Member {DisplayName = "Morten Rask", Username = "Morten", Password = "adminadmin", IsAdmin = true}
            };

            List<Grade> grades = new List<Grade> {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67,160,71), Id = new Guid()},
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33,150,254), Id = new Guid()},
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228,83,80), Id = new Guid()},
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97,97,97), Id = new Guid()},
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251,251,251), Id = new Guid()},
                new Grade {Name = "Magic", Difficulty = 5, Color = new Color(251,251,251), Id = new Guid()},
            };

            Section sectionA = new Section { Id = new Guid(), Name = "A" };
            List<Route> routesForA = new List<Route> {
                new Route{Name = "4", Image = images[0], ColorOfHolds = new Color(255, 0, 0), Member = members[0], Author = members[0].DisplayName, Grade = grades[5], CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "14", ColorOfHolds = new Color(0, 255, 0), Member = members[0], Author = members[0].DisplayName, Grade = grades[1], CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "43", ColorOfHolds = new Color(255, 0, 255), Member = members[0], Author = members[0].DisplayName, Grade = grades[2], CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "21", ColorOfHolds = new Color(255, 255, 0), Member = members[0], Author = members[0].DisplayName, Grade = grades[3], CreatedDate = new DateTime(2016, 03, 24)} };
            sectionA.Routes.AddRange(routesForA);
            context.Sections.Add(sectionA);

            Section sectionB = new Section { Id = new Guid(), Name = "B" };
            List<Route> routesForB = new List<Route> {
                new Route{Name = "32", ColorOfHolds =  new Color(100, 100, 100), Member = members[0], Author = members[0].DisplayName, Grade = grades[4], CreatedDate = new DateTime(2014, 11, 24)},
                new Route{Name = "99", ColorOfHolds =  new Color(170, 12, 54), Member = members[0], Author = members[0].DisplayName, Grade = grades[2], CreatedDate = new DateTime(2016, 01, 02)},
                new Route{Name = "3", ColorOfHolds =  new Color(255, 34, 89), Member = members[0], Author = members[0].DisplayName, Grade = grades[3], CreatedDate = new DateTime(2016, 04, 11)},
                new Route{Name = "7", ColorOfHolds =  new Color(232, 233, 5), Member = members[0], Author = members[0].DisplayName, Grade = grades[3], CreatedDate = new DateTime(2016, 08, 10)} };
            sectionB.Routes.AddRange(routesForB);
            context.Sections.Add(sectionB);

            Section sectionC = new Section { Id = new Guid(), Name = "C" };
            List<Route> routesForC = new List<Route> {
                new Route{Name = "66", ColorOfHolds =  new Color(255, 0, 0), Member = members[0], Author = members[0].DisplayName, Grade = grades[0], CreatedDate = new DateTime(2016, 03, 24)},
                new Route{Name = "33", ColorOfHolds =  new Color(0, 22, 123), Member = members[0], Author = members[0].DisplayName, Grade = grades[1], CreatedDate = new DateTime(2016, 07, 12)},
                new Route{Name = "22", ColorOfHolds =  new Color(255, 123, 0), Member = members[0], Author = members[0].DisplayName, Grade = grades[1], CreatedDate = new DateTime(2016, 11, 11)},
                new Route{Name = "44", ColorOfHolds =  new Color(123, 22, 22), Member = members[0], Author = members[0].DisplayName, Grade = grades[2], CreatedDate = new DateTime(2016, 03, 24)} };
            sectionC.Routes.AddRange(routesForC);
            context.Sections.Add(sectionC);

            Section sectionD = new Section { Id = new Guid(), Name = "D" };
            List<Route> routesForD = new List<Route> {
                new Route{Name = "20", ColorOfHolds = new Color(35, 0, 22), Member = members[0], Author = members[0].DisplayName, Grade = grades[1], CreatedDate = new DateTime(2016, 03, 01), ColorOfTape = new Color(123,255,22)},
                new Route{Name = "9", ColorOfHolds = new Color(123, 255, 22), Member = members[0], Author = members[0].DisplayName, Grade = grades[0], CreatedDate = new DateTime(2016, 10, 27)},
                new Route{Name = "76", ColorOfHolds = new Color(0, 22, 68), Member = members[0], Author = members[0].DisplayName, Grade = grades[0], CreatedDate = new DateTime(2016, 09, 04)},
                new Route{Name = "54", ColorOfHolds = new Color(123, 22, 123), Member = members[0], Author = members[0].DisplayName, Grade = grades[4], CreatedDate = new DateTime(2016, 06, 22)} };
            sectionD.Routes.AddRange(routesForD);
            context.Sections.Add(sectionD);
            */

            //// save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}