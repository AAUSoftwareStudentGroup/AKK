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
        public List<HoldColor> _holdColors;
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

        public List<HoldColor> HoldColors {
            get { return _holdColors; }
            set { _holdColors = value; }
        }

        public TestDataFactory()
        {
            _members = new List<Member>
            {
                new Member {Id = Guid.NewGuid(), DisplayName = "Anton"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Jakobsen"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Hornum"},
                new Member {Id = Guid.NewGuid(), DisplayName = "Jakob"},
                new Member {Id = Guid.NewGuid(), DisplayName = "TannerHelland", IsAdmin = true},
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

            _holdColors = new List<HoldColor>();
            
            foreach (var color in colors)
            {
                HoldColor newColor = new HoldColor();
                newColor.HexColorOfHolds = color.Value.ToUint();
                newColor.Name = color.Key;
                newColor.Id = Guid.NewGuid();
                _holdColors.Add(newColor);
            }

            _routes = new List<Route>
            {
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "4",
                    SectionId = _sections[0].Id,
                    Member = _members[0],
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "14",
                    SectionId = _sections[0].Id,
                    Member = _members[1],
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "43",
                    SectionId = _sections[0].Id,
                    Member = _members[2],
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "21",
                    SectionId = _sections[0].Id,
                    Member = _members[3],
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "32",
                    SectionId = _sections[1].Id,
                    Member = _members[4],
                    GradeId = _grades[4].Id,
                    CreatedDate = new DateTime(2014, 11, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "99",
                    SectionId = _sections[1].Id,
                    Member = _members[5],
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 01, 02)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "3",
                    SectionId = _sections[1].Id,
                    Member = _members[6],
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 04, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "7",
                    SectionId = _sections[1].Id,
                    Member = _members[0],
                    GradeId = _grades[3].Id,
                    CreatedDate = new DateTime(2016, 08, 10)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "66",
                    SectionId = _sections[2].Id,
                    Member = _members[7],
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "33",
                    SectionId = _sections[2].Id,
                    Member = _members[8],
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "94",
                    SectionId = _sections[2].Id,
                    Member = _members[9],
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "22",
                    SectionId = _sections[2].Id,
                    Member = _members[10],
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "44",
                    SectionId = _sections[2].Id,
                    Member = _members[11],
                    GradeId = _grades[2].Id,
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "20",
                    SectionId = _sections[3].Id,
                    Member = _members[12],
                    GradeId = _grades[1].Id,
                    CreatedDate = new DateTime(2016, 03, 01),
                    ColorOfTape = new Color(123, 255, 22)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "9",
                    SectionId = _sections[3].Id,
                    Member = _members[13],
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 10, 27)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "76",
                    SectionId = _sections[3].Id,
                    Member = _members[14],
                    GradeId = _grades[0].Id,
                    CreatedDate = new DateTime(2016, 09, 04)
                },
                new Route
                {
                    Id = Guid.NewGuid(),
                    Name = "54",
                    SectionId = _sections[3].Id,
                    Member = _members[15],
                    GradeId = _grades[4].Id,
                    CreatedDate = new DateTime(2016, 06, 22)
                }
            };


            for(int i = 0; i< _routes.Count; i++)
            {
                _routes[i].Author = _routes[i].Member.DisplayName;
                _routes[i].ColorOfHolds = _holdColors.ElementAt(i%HoldColors.Count).ColorOfHolds;
            }

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

            for (int i = 0; i < Routes.Count; i++)
            {
                Routes[i].Ratings.Add(new Rating{RatingValue = i%5 + 1, Member = _members[5]});
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

            AKK.Services.AuthenticationService s = new AKK.Services.AuthenticationService(new AKK.Models.Repositories.TestRepository<Member>(_members));
            foreach (Member member in _members)
            {
                member.Username = member.DisplayName.ToLower();
                member.Password = s.HashPassword("123");
            }
        }
    }
}
