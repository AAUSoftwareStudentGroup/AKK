using System;
using System.Collections.Generic;
using System.Linq;
using AKK.Models;
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
        private TestDataFactory _dataFactory;

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlite("Filename=./testdb.sqlite");
            _mainDbContext = new MainDbContext(optionsBuilder.Options);
            _dataFactory = new TestDataFactory();   
            _mainDbContext.Database.EnsureDeleted();
            _mainDbContext.Database.EnsureCreated();

            _members = _dataFactory.Members;
            _sections = _dataFactory.Sections;
            _grades = _dataFactory.Grades;
            _routes = _dataFactory.Routes;

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
            _members = null;
            _sections = null;
            _grades = null;
            _routes = null;
            _dataFactory = null;
        }

        [Test]
        public void Sections_FirstOrderedByName_NameEqualsARoutesCountEqualsFour()
        {
            Section section = _mainDbContext.Sections.Include(s => s.Routes).OrderBy(s => s.Name).First();
            Assert.AreEqual(section.Name, "A");
            Assert.AreEqual(4, section.Routes.Count);
        }

        [Test]
        public void Sections_SectionA_LastOrderedByRouteNameEqualsJakobsen()
        {
            Section section = _mainDbContext.Sections.Include(s => s.Routes).ThenInclude(r => r.Member).Single(s => s.Name == "A");
            Assert.AreEqual("Jakobsen", section.Routes.OrderBy(r => r.Author).Last().Author);
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
