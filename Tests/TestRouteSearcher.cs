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
        public void Search_SearchFor10Red_RoutesWithGradeRed() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
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
        public void Search_SearchFor10Gebra_RoutesWithAuthorGeogebraFirst() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("gebra");

            Assert.AreEqual("Geogebra", searchResult?.FirstOrDefault().Author);
        }

        [Test]
        public void Search_SearchFor5G_5RoutesWithG() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 5);
            var searchResult = searcher.Search("g");

            Assert.AreEqual(10, searchResult.Count());    
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
    }
}
