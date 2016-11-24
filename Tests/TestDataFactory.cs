using System;
using System.Collections.Generic;
using AKK.Models;

namespace AKK.Tests
{
    public class TestDataFactory
    {
        private List<Member> _members;
        private List<Section> _sections;
        private List<Grade> _grades;
        private List<Route> _routes;

        public List<Member> Members {
            get { return _members; }
            set { _members = value; }
        }

        public List<Section> Sections {
            get { return _sections; }
            set { _sections = value; }
        }

        public List<Grade> Grades {
            get { return _grades; }
            set { _grades = value; }
        }

        public List<Route> Routes {
            get { return _routes; }
            set { _routes = value; }
        }

        public TestDataFactory()
        {
            _members = new List<Member>
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

            _sections = new List<Section>
            {
                new Section {Name = "A", Id = Guid.NewGuid()},
                new Section {Name = "B", Id = Guid.NewGuid()},
                new Section {Name = "C", Id = Guid.NewGuid()},
                new Section {Name = "D", Id = Guid.NewGuid()},
            };

            _grades = new List<Grade>
            {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67, 160, 71), Id = Guid.NewGuid()},
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33, 150, 254), Id = Guid.NewGuid()},
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80), Id = Guid.NewGuid()},
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97), Id = Guid.NewGuid()},
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251), Id = Guid.NewGuid()},
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
        }
    }
}
