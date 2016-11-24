using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKK.Classes.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AKK.Tests.Models
{
    [TestFixture]
    public class MainDbContextTests
    {
        private MainDbContext _mainDbContext;
        private List<Member> _members;
        private List<Section> _sections;
        private List<Grade> _grades;
        private List<Route> _routes;

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlite("Filename=./testdb.sqlite");
            _mainDbContext = new MainDbContext(optionsBuilder.Options);
            _mainDbContext.Database.EnsureDeleted();
            _mainDbContext.Database.EnsureCreated();

            _members = new List<Member>
            {
                new Member {DisplayName = "Morten", Username = "Morten123", Password = "adminadmin", IsAdmin = true}
            };

            _sections = new List<Section>
            {
                new Section {Name = "A"},
                new Section {Name = "B"},
                new Section {Name = "C"},
                new Section {Name = "D"},
            };

            _grades = new List<Grade>
            {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67, 160, 71)},
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33, 150, 254)},
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80)},
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97)},
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251)},
            };

            _routes = new List<Route>
            {
                new Route
                {
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = false},
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "14",
                    Section = _sections[0],
                    ColorOfHolds = new Color(0, 255, 0),
                    Member = new Member {DisplayName = "Wagner", Username = "Jakobsen123", Password = "123", IsAdmin = false},
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
                    Member = new Member {DisplayName = "TannerHelland", Username = "TannerHelland123", Password = "123", IsAdmin = false},
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
            _mainDbContext.Sections.AddRange(_sections);
            _mainDbContext.Grades.AddRange(_grades);
            _mainDbContext.Routes.AddRange(_routes);
            _mainDbContext.SaveChanges();
            _mainDbContext.Dispose();
            _mainDbContext = new MainDbContext(optionsBuilder.Options);
        }

        [TearDown]
        public void TearDown()
        {
            _mainDbContext.Dispose();
        }

        [Test]
        public void Sections_FirstOrderedByName_NameEqualsARoutesCountEqualsFour()
        {
            Section section = _mainDbContext.Sections.Include(s => s.Routes).OrderBy(s => s.Name).First();
            Assert.AreEqual(section.Name, "A");
            Assert.AreEqual(4, section.Routes.Count);
        }

        [Test]
        public void Sections_SectionA_LastOrderedByRouteNameEqualsManfred()
        {
            Section section = _mainDbContext.Sections.Include(s => s.Routes).ThenInclude(r => r.Member).Single(s => s.Name == "A");
            Assert.AreEqual("Wagner", section.Routes.OrderBy(r => r.Author).Last().Author);
        }

        [Test]
        public void Sections_All_ABCDListContainsAllNames()
        {
            IEnumerable<Section> sections = _mainDbContext.Sections;

            Assert.AreEqual(4, sections.Count());

            List<string> names = new List<string> { "A", "B", "C", "D" };

            foreach(Section section in sections)
            {
                Assert.IsTrue(names.Contains(section.Name));
                int oldCount = names.Count;
                names.Remove(section.Name);
                Assert.Greater(oldCount, names.Count);
            }

            Assert.AreEqual(0, names.Count);
        }

        [Test] 
        public void Sections_All_IdsAreNotEqual()
        {
            List<Section> sections = _mainDbContext.Sections.ToList();

            for(int n = 0; n < sections.Count; n++)
            {
                for(int m = 0; m < sections.Count; m++)
                {
                    if(n != m)
                    {
                        Assert.AreNotEqual(sections[n].Id, sections[m].Id);
                    }
                }
            }
        }
    }
}
