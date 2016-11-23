using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKK.Classes;
using AKK.Classes.Models;
using NUnit.Framework;

namespace AKK.Tests
{
    [TestFixture]
    public class TestRouteSearcher
    {
        [Test]
        public void Search_SearchFor10Green_RoutesWithGradeGreen()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("green");

            foreach (var result in searchResult)
                Assert.AreEqual("Green", result.Grade.Name);
        }

        [Test]
        public void Search_SearchFor3Red_RoutesWithGradeRed() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 3);
            var searchResult = searcher.Search("red");

            foreach (var result in searchResult)
                Assert.AreEqual("Red", result.Grade.Name);
        }

        [Test]
        public void Search_SearchFor10Anto_RoutesWithAuthorAnton() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("anto");

            foreach (var result in searchResult)
                Assert.AreEqual("Anton", result.Author);
        }

        [Test]
        public void Search_SearchFor10Geo_RoutesWithAuthorGeoAndGeogebraInOrder() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("geo");

            Assert.AreEqual("Geo", searchResult.ElementAt(0).Author);
            Assert.AreEqual("Geogebra", searchResult.ElementAt(1).Author);
        }


        [Test]
        public void Search_SearchFor5G_5RoutesWithG() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 5);
            var searchResult = searcher.Search("g");

            Assert.AreEqual(5, searchResult.Count());    
        }

        [Test]
        public void _computeLevenshtein_DistanceBetweenPatternAndTextAlwaysGreaterOrEqualThanPatternLength()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var pattern = "YYY";
            var distance = searcher._computeLevenshtein(pattern, "A");
            Assert.GreaterOrEqual(distance, pattern.Length);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGreenAndMorten_DistanceIs9() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var distance = searcher._computeLevenshtein("Green", "Morten");
            Assert.AreEqual(9, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAndGeogebra_DistanceIs5() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var distance = searcher._computeLevenshtein("Geo", "Geogebra");
            Assert.AreEqual(5, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAndC_DistanceIs12() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var distance = searcher._computeLevenshtein("Geo", "C");
            Assert.AreEqual(12, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAnd94_DistanceIs12() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var distance = searcher._computeLevenshtein("Geo", "94");
            Assert.AreEqual(12, distance);
        }

        [Test]
        public void _computeLevenshtein_SearchFor10Bl_8RoutesWithGradesBlueAndBlack() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("Blu");

            foreach (var result in searchResult) {
                if(result.Grade.Name == "Black" || result.Grade.Name == "Blue") {
                } else {
                    Assert.Fail($"  Expected: grade of Blue or Black\n  But grade was: {result.Grade.Name}.");
                }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForHelland_ExpectRouteWithTannerHellandAsAuthor() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("Helland");
            if (searchResult.ToArray().Length == 0) {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("TannerHelland", searchResult.ElementAt(0).Author);
        }

        [Test]
        public void _computeLevenshtein_3SearchForASPACE4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 3);
            var searchResult = searcher.Search("A 4");

            foreach (var result in searchResult)
            {
                Assert.AreEqual("A", result.Section.Name);
            }
        }

        [Test]
        public void _computeLevenshtein_3SearchForA4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 3);
            var searchResult = searcher.Search("A4");

            foreach (var result in searchResult)
            {
                Assert.AreEqual("A", result.Section.Name);
            }
        }

        [Test]
        public void _computeLevenshtein_3SearchForA4_ExpectedResultTheSameAsSearchForASPACE4() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResultWithSpace = searcher.Search("A 4");
            var searchResultWithoutSpace = searcher.Search("A4");

            int length = searchResultWithSpace.ToArray().Length;

            if(searchResultWithoutSpace.ToArray().Length != length) {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: WithSpace: {length}  noSpace: {searchResultWithoutSpace.ToArray().Length}");
            }

            for (int i = 0; i < length; i++)
            {
               if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true) {
                   Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
               }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForASPACE4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("A 4 Gr");

            foreach (var result in searchResult)
            {
                if(result.Section.Name == "A" || result.Name.Contains("4") || result.Grade.Name == "Green") {

                } else {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Grade Green\n  Was: {result.Section.Name}  {result.Name}  {result.Grade.Name}");
                }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForA4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("A4 Gr");

            foreach (var result in searchResult)
            {
                if(result.Section.Name == "A" || result.Name.Contains("4") || result.Grade.Name == "Green") {

                } else {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Grade Green\n  Was: {result.Section.Name}  {result.Name}  {result.Grade.Name}");
                }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForA4SPACEGr_ExpectedResultTheSameAsWithSpaceBetweenAAnd4() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResultWithoutSpace = searcher.Search("A4 Gr");
            var searchResultWithSpace = searcher.Search("A 4 Gr");

            int length = searchResultWithoutSpace.ToArray().Length;

            if(searchResultWithSpace.ToArray().Length != length) {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: WithSpace: {searchResultWithSpace.ToArray().Length}  noSpace: {length}");
            }

            for (int i = 0; i < length; i++)
            {
               if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true) {
                   Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
               }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForA4SPACEGru_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("A4 Gru");

            foreach (var result in searchResult)
            {
                if(result.Section.Name == "A" || result.Name.Contains("4") || result.Author.Contains("Gru")) {

                } else {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Author containing Gru\n  Was: {result.Section.Name}  {result.Name}  {result.Author}");
                }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForASPACE4SPACEGru_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("A 4 Gru");

            foreach (var result in searchResult)
            {
                if(result.Section.Name == "A" || result.Name.Contains("4") || result.Author.Contains("Gru")) {

                } else {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Author containing Gru\n  Was: {result.Section.Name}  {result.Name}  {result.Author}");
                }
            }
        }

        [Test]
        public void _computeLevenshtein_10SearchForA4SPACEGru_ExpectedResultTheSameAsWithSpaceBetweenAAnd4() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResultWithoutSpace = searcher.Search("A4 Gru");
            var searchResultWithSpace = searcher.Search("A 4 Gru");

            int length = searchResultWithoutSpace.ToArray().Length;

            if(searchResultWithSpace.ToArray().Length != length) {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: WithSpace: {searchResultWithSpace.ToArray().Length}  noSpace: {length}");
            }

            for (int i = 0; i < length; i++)
            {
               if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true) {
                   Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
               }
            }
        }
    }
}
