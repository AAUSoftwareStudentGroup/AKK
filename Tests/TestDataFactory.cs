﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKK.Classes.Models;

namespace AKK.Tests
{
    public class TestDataFactory
    {
        private List<Section> _sections;
        private List<Grade> _grades;
        private List<Route> _routes;

        public List<Section> Sections {
            get { return _sections; }
            set { _sections = value; }
        }

        public List<Grade> Grades
        {
            get { return _grades; }
            set { _grades = value; }
        }

        public List<Route> Routes {
            get { return _routes; }
            set { _routes = value; }
        }

        public TestDataFactory()
        {
            _sections = new List<Section>
            {
                new Section {Name = "A", Id = new Guid()},
                new Section {Name = "B", Id = new Guid()},
                new Section {Name = "C", Id = new Guid()},
                new Section {Name = "D", Id = new Guid()},
            };

            _grades = new List<Grade>
            {
                new Grade {Name = "Green", Difficulty = 0, Color = new Color(67, 160, 71), Id = new Guid()},
                new Grade {Name = "Blue", Difficulty = 1, Color = new Color(33, 150, 254), Id = new Guid()},
                new Grade {Name = "Red", Difficulty = 2, Color = new Color(228, 83, 80), Id = new Guid()},
                new Grade {Name = "Black", Difficulty = 3, Color = new Color(97, 97, 97), Id = new Guid()},
                new Grade {Name = "White", Difficulty = 4, Color = new Color(251, 251, 251), Id = new Guid()},
            };

            _routes = new List<Route>
            {
                new Route
                {
                    Name = "4",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 0),
                    Author = "Anton",
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "14",
                    Section = _sections[0],
                    ColorOfHolds = new Color(0, 255, 0),
                    Author = "Jakobsen",
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "43",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 0, 255),
                    Author = "Hornum",
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "21",
                    Section = _sections[0],
                    ColorOfHolds = new Color(255, 255, 0),
                    Author = "Jakob",
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "32",
                    Section = _sections[1],
                    ColorOfHolds = new Color(100, 100, 100),
                    Author = "TannerHelland",
                    Grade = _grades[4],
                    CreatedDate = new DateTime(2014, 11, 24)
                },
                new Route
                {
                    Name = "99",
                    Section = _sections[1],
                    ColorOfHolds = new Color(170, 12, 54),
                    Author = "Grunberg",
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 01, 02)
                },
                new Route
                {
                    Name = "3",
                    Section = _sections[1],
                    ColorOfHolds = new Color(255, 34, 89),
                    Author = "Ibsen",
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 04, 11)
                },
                new Route
                {
                    Name = "7",
                    Section = _sections[1],
                    ColorOfHolds = new Color(232, 233, 5),
                    Author = "Anton",
                    Grade = _grades[3],
                    CreatedDate = new DateTime(2016, 08, 10)
                },
                new Route
                {
                    Name = "66",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 0, 0),
                    Author = "Geo",
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "33",
                    Section = _sections[2],
                    ColorOfHolds = new Color(0, 22, 123),
                    Author = "Bacci",
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 07, 12)
                },
                new Route
                {
                    Name = "94",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Author = "Geogebra",
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "22",
                    Section = _sections[2],
                    ColorOfHolds = new Color(255, 123, 0),
                    Author = "Kurt",
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 11, 11)
                },
                new Route
                {
                    Name = "44",
                    Section = _sections[2],
                    ColorOfHolds = new Color(123, 22, 22),
                    Author = "Benja",
                    Grade = _grades[2],
                    CreatedDate = new DateTime(2016, 03, 24)
                },
                new Route
                {
                    Name = "20",
                    Section = _sections[3],
                    ColorOfHolds = new Color(35, 0, 22),
                    Author = "Manfred",
                    Grade = _grades[1],
                    CreatedDate = new DateTime(2016, 03, 01),
                    ColorOfTape = new Color(123, 255, 22)
                },
                new Route
                {
                    Name = "9",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 255, 22),
                    Author = "Bettina",
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 10, 27)
                },
                new Route
                {
                    Name = "76",
                    Section = _sections[3],
                    ColorOfHolds = new Color(0, 22, 68),
                    Author = "Kasper",
                    Grade = _grades[0],
                    CreatedDate = new DateTime(2016, 09, 04)
                },
                new Route
                {
                    Name = "54",
                    Section = _sections[3],
                    ColorOfHolds = new Color(123, 22, 123),
                    Author = "Rasmus",
                    Grade = _grades[4],
                    CreatedDate = new DateTime(2016, 06, 22)
                }
            };
        }
    }
}