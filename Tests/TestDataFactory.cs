﻿using System;
using System.Collections.Generic;
using System.Linq;
using AKK.Models;

namespace AKK.Tests
{
    public class TestDataFactory
    {
        private List<Member> _members;
        private List<Section> _sections;
        private List<Grade> _grades;
        private List<Route> _routes;
        public List<Image> _images;
        public List<Hold> _holds;
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

        public List<Image> Images {
            get { return _images; }
            set { _images = value; }
        }

        public List<Hold> Holds {
            get { return _holds; }
            set { _holds = value; }
        }

        public TestDataFactory()
        {
            _members = new List<Member>
            {
                new Member {Id = Guid.NewGuid(), DisplayName = "Anton"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Jakobsen"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Hornum"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Jakob"},
                new Member {Id = Guid.NewGuid(), DisplayName = "TannerHelland"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Grunberg"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Ibsen"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Geo"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Bacci"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Geogebra"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Kurt"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Benja"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Manfred"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Betinna"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Kasper"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Rasmus"}
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
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67, 160, 71), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33, 150, 254), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97), Id = Guid.NewGuid(), Routes = new List<Route>() },
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251), Id = Guid.NewGuid(), Routes = new List<Route>() }
            };

            _routes = new List<Route>
            {
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "4",
                    SectionId = _sections[0].Id,
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = false},
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "14",
                    SectionId = _sections[0].Id,
                    ColorOfHolds = new Color(0, 255, 0),
                    Member = new Member {DisplayName = "Jakobsen", Username = "Jakobsen123", Password = "123", IsAdmin = false},
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "43",
                    SectionId = _sections[0].Id,
                    ColorOfHolds = new Color(255, 0, 255),
                    Member = new Member {DisplayName = "Hornum", Username = "Hornum123", Password = "123", IsAdmin = false},
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "21",
                    SectionId = _sections[0].Id,
                    ColorOfHolds = new Color(255, 255, 0),
                    Member = new Member {DisplayName = "Jakob", Username = "Jakob123", Password = "123", IsAdmin = false},
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "32",
                    SectionId = _sections[1].Id,
                    ColorOfHolds = new Color(100, 100, 100),
                    Member = new Member {DisplayName = "TannerHelland", Username = "TannerHelland123", Password = "123", IsAdmin = false},
                    GradeId = _grades[4].Id,
                    CreatedDate = new DateTime(2014, 11, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "99",
                    SectionId = _sections[1].Id,
                    ColorOfHolds = new Color(170, 12, 54),
                    Member = new Member {DisplayName = "Grunberg", Username = "Grunberg123", Password = "123", IsAdmin = false},
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 01, 02)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "3",
                    SectionId = _sections[1].Id,
                    ColorOfHolds = new Color(255, 34, 89),
                    Member = new Member {DisplayName = "Ibsen", Username = "Ibsen123", Password = "123", IsAdmin = false},
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 04, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "7",
                    SectionId = _sections[1].Id,
                    ColorOfHolds = new Color(232, 233, 5),
                    Member = new Member {DisplayName = "Anton", Username = "Anton123", Password = "123", IsAdmin = false},
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 08, 10)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "66",
                    SectionId = _sections[2].Id,
                    ColorOfHolds = new Color(255, 0, 0),
                    Member = new Member {DisplayName = "Geo", Username = "Geo123", Password = "123", IsAdmin = false},
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "33",
                    SectionId = _sections[2].Id,
                    ColorOfHolds = new Color(0, 22, 123),
                    Member = new Member {DisplayName = "Bacci", Username = "Bacci123", Password = "123", IsAdmin = false},
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "94",
                    SectionId = _sections[2].Id,
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = new Member {DisplayName = "Geogebra", Username = "Geogebra123", Password = "123", IsAdmin = false},
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "22",
                    SectionId = _sections[2].Id,
                    ColorOfHolds = new Color(255, 123, 0),
                    Member = new Member {DisplayName = "Kurt", Username = "Kurt123", Password = "123", IsAdmin = false},
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "44",
                    SectionId = _sections[2].Id,
                    ColorOfHolds = new Color(123, 22, 22),
                    Member = new Member {DisplayName = "Benja", Username = "Benja123", Password = "123", IsAdmin = false},
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "20",
                    SectionId = _sections[3].Id,
                    ColorOfHolds = new Color(35, 0, 22),
                    Member = new Member {DisplayName = "Manfred", Username = "Manfred123", Password = "123", IsAdmin = false},
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 03, 01),
                    ColorOfTape = new Color(123, 255, 22)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "9",
                    SectionId = _sections[3].Id,
                    ColorOfHolds = new Color(123, 255, 22),
                    Member = new Member {DisplayName = "Bettina", Username = "Bettina123", Password = "123", IsAdmin = false},
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 10, 27)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "76",
                    SectionId = _sections[3].Id,
                    ColorOfHolds = new Color(0, 22, 68),
                    Member = new Member {DisplayName = "Kasper", Username = "Kasper123", Password = "123", IsAdmin = false},
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 09, 04)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "54",
                    SectionId = _sections[3].Id,
                    ColorOfHolds = new Color(123, 22, 123),
                    Member = new Member {DisplayName = "Rasmus", Username = "Rasmus123", Password = "123", IsAdmin = false},
                    GradeId = _grades[4].Id,
                    CreatedDate = new DateTime(2016, 06, 22)
                }
            };

            foreach (var section in _sections)
            {
                foreach (var route in _routes)
                {
                    if(route.SectionId == section.Id)
                    {
                        section.Routes.Add(route);
                        route.Section = section;
                    }
                }
            }

            foreach (var grade in _grades) 
            {
                foreach (var route in _routes)
                {
                    if(grade.Id == route.GradeId)
                    {
                        grade.Routes.Add(route);
                        route.Grade = grade;
                    }
                }
            }

            _images = new List<Image>();
            _images.AddRange(new List<Image> {
                new Image {Id = Guid.NewGuid(), RouteId = _routes.First().Id, Width = 800, Height = 500, FileUrl = "https://placeholdit.imgix.net/~text?txtsize=28&txt=500%C3%97800&w=500&h=800"}
            });

            _holds = new List<Hold>();
            _holds.AddRange(new List<Hold> {
                new Hold {Id = Guid.NewGuid(), ImageId = _images[0].Id, X = 0.5, Y = 0.5, Radius = 0.1}
            });

            _holds[0].Image = _images[0];
            _images[0].Holds = _holds;
        }
    }
}
