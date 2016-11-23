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
        public void Search_SearchFor10Gebra_RoutesWithAuthorGeogebraFirst() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);
            var searchResult = searcher.Search("gebra");

            if (!searchResult.Any()) {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("Geogebra", searchResult.FirstOrDefault().Author);
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
        public void _computeLevenshtein_CalculateDistanceBetweeenGebraAndGeo_DistanceIs3() {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearcher(testRepo.Routes, 10);

            var distance = searcher._computeLevenshtein("Gebra", "Geogebra");
            Assert.AreEqual(3, distance);
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
            if (!searchResult.Any()) {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("TannerHelland", searchResult.ElementAt(0).Author);
        }

    }
}
