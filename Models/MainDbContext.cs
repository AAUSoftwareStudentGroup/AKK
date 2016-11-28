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
                new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = true},
                new Member {DisplayName = "Grunberg", Username = "Grunberg123", Password = "123", IsAdmin = true},
                new Member {DisplayName = "Jakobsen", Username = "Jakobsen123", Password = "123", IsAdmin = true},
                new Member {DisplayName = "Hornum", Username = "Hornum123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Jakob", Username = "Jakob123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "TannerHelland", Username = "TannerHelland123", Password = "123", IsAdmin = true},
                new Member {DisplayName = "Grunberg", Username = "Grunberg123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Ibsen", Username = "Ibsen123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Geo", Username = "Geo123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Bacci", Username = "Bacci123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Geogebra", Username = "Geogebra123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Kurt", Username = "Kurt123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Benja", Username = "Benja123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Manfred", Username = "Manfred123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Betinna", Username = "Betinna123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Kasper", Username = "Kasper123", Password = "123", IsAdmin = false},
                new Member {DisplayName = "Rasmus", Username = "Rasmus123", Password = "123", IsAdmin = false}
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
                    Note = "Hej",
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = _members[0],
                    Author = _members[0].DisplayName,
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = _members[1],
                    Author = _members[1].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "14",
                    Section = _sections[0],
                    ColorOfHolds = new Color(0, 255, 0),
                    Member = _members[2],
                    Author = _members[2].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "43",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 255),
                    Member = _members[3],
                    Author = _members[3].DisplayName,
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "21",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 255, 0),
                    Member = _members[4],
                    Author = _members[4].DisplayName,
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "32",
                    Section = _sections[1],
                    ColorOfHolds = new Color(100, 100, 100),
                    Member = _members[5],
                    Author = _members[5].DisplayName,
                    Grade = _grades[4],
                    CreatedDate = new DateTime(2014, 11, 24)
                },
                new Route
                {
                    Name = "99",
                    Section = _sections[1],
                    ColorOfHolds = new Color(170, 12, 54),
                    Member = _members[1],
                    Author = _members[1].DisplayName,
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 01, 02)
                },
                new Route
                {
                    Name = "3",
                    Section = _sections[1],
                    ColorOfHolds = new Color(255, 34, 89),
                    Member = _members[6],
                    Author = _members[6].DisplayName,
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 04, 11)
                },
                new Route
                {
                    Name = "7",
                    Section = _sections[1],
                    ColorOfHolds = new Color(232, 233, 5),
                    Member = _members[7],
                    Author = _members[7].DisplayName,
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 08, 10)
                },
                new Route
                {
                    Name = "66",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = _members[8],
                    Author = _members[8].DisplayName,
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "33",
                    Section = _sections[2],
                    ColorOfHolds = new Color(0, 22, 123),
                    Member = _members[9],
                    Author = _members[9].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "94",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = _members[10],
                    Author = _members[10].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "22",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = _members[11],
                    Author = _members[11].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "44",
                    Section = _sections[2],
                    ColorOfHolds = new Color(123, 22, 22),
                    Member = _members[12],
                    Author = _members[12].DisplayName,
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "20",
                    Section = _sections[3],
                    ColorOfHolds = new Color(35, 0, 22),
                    Member = _members[13],
                    Author = _members[13].DisplayName,
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 03, 01),
                    ColorOfTape = new Color(123, 255, 22)
                },
                new Route
                {
                    Name = "9",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 255, 22),
                    Member = _members[14],
                    Author = _members[14].DisplayName,
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 10, 27)
                },
                new Route
                {
                    Name = "76",
                    Section = _sections[3],
                    ColorOfHolds = new Color(0, 22, 68),
                    Member = _members[15],
                    Author = _members[15].DisplayName,
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 09, 04)
                },
                new Route
                {
                    Name = "54",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 22, 123),
                    Member = _members[16],
                    Author = _members[16].DisplayName,
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

            //// save changes and release resources
            context.SaveChanges();
            context.Dispose();
        }
    }
}