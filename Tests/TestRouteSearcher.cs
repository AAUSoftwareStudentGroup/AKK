using System.Linq;
using NUnit.Framework;
using AKK.Services;

namespace AKK.Tests
{
    [TestFixture]
    public class TestRouteSearcher
    {
        /*Since addition, deletion and substitution are all >1 when computing the Levenshtein distance, the length of
         * the distance should be greater than or equal to the length of the pattern.
         */
        [Test]
        public void _computeLevenshtein_DistanceBetweenPatternAndTextAlwaysGreaterOrEqualToPatternLength()
        {
            var pattern = "Tanner";
            var text = "TannerHelland";
            var distance = RouteSearchService._computeLevenshtein(pattern, text);
            Assert.GreaterOrEqual(distance, pattern.Length);

            distance = RouteSearchService._computeLevenshtein(text, pattern);
            Assert.GreaterOrEqual(distance, pattern.Length);
        }

        //Asserts that the distance between certain words is correct according.
        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGreenAndMorten_DistanceIs9()
        {
            var distance = RouteSearchService._computeLevenshtein("Green", "Morten");
            Assert.AreEqual(9, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAndGeogebra_DistanceIs5()
        { 
            var distance = RouteSearchService._computeLevenshtein("Geo", "Geogebra");
            Assert.AreEqual(5, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGebraAndGeo_DistanceIs3()
        {
            var distance = RouteSearchService._computeLevenshtein("Gebra", "Geogebra");
            Assert.AreEqual(3, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAnd94_DistanceIs12()
        { 
            var distance = RouteSearchService._computeLevenshtein("Geo", "94");
            Assert.AreEqual(12, distance);
        }

        /* The following tests will assert that searching for a specific term (difficulty, author, etc.) will only display 
         * routes that match the search term.
         */
        [Test]
        public void Search_SearchFor10Green_RoutesWithGradeGreen()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("green");

            foreach (var result in searchResult)
                Assert.AreEqual("Green", result.Grade.Name);
        }

        [Test]
        public void Search_SearchFor3Red_RoutesWithGradeRed()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 3);
            var searchResult = searcher.Search("red");

            foreach (var result in searchResult)
                Assert.AreEqual("Red", result.Grade.Name);
        }

        [Test]
        public void Search_SearchFor10Anto_RoutesWithAuthorAnton()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("anto");

            foreach (var result in searchResult)
                Assert.AreEqual("Anton", result.Author);
        }

        //Asserts that by searching for 'Geo', the route made by Geo will show before the route made by Geogebra.
        [Test]
        public void Search_SearchFor10Geo_RoutesWithAuthorGeoAndGeogebraInOrder()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("geo");

            Assert.AreEqual("Geo", searchResult.ElementAt(0).Author);
            Assert.AreEqual("Geogebra", searchResult.ElementAt(1).Author);
        }

        /*Asserts that searching for a substring of the original string will work - also in cases where the substring does not
         * include the beginning of the original string
         */
        [Test]
        public void Search_SearchFor10Gebra_RoutesWithAuthorGeogebraFirst()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("gebra");

            if (!searchResult.Any())
            {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("Geogebra", searchResult.FirstOrDefault().Author);
        }

        [Test]
        public void Search_10SearchForHelland_ExpectRouteWithTannerHellandAsAuthor()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("Helland");
            if (!searchResult.Any())
            {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("TannerHelland", searchResult.ElementAt(0).Author);
        }

        //Asserts that the calculation of Levenshtein works. The black route is meant to show up since it's threshold is 2.
        [Test]
        public void Search_SearchFor10Bl_8RoutesWithGradesBlueAndBlack()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("Blu");

            foreach (var result in searchResult)
            {
                if (result.Grade.Name == "Black" || result.Grade.Name == "Blue")
                {
                }
                else
                {
                    Assert.Fail($"  Expected: grade of Blue or Black\n  But grade was: {result.Grade.Name}.");
                }
            }
        }

        //With one route in section A with the route name 4, it should show up first when searching for A 4.
        [Test]
        public void Search_3SearchForASPACE4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 3);
            var searchResult = searcher.Search("A 4");

            foreach (var result in searchResult)
            {
                Assert.AreEqual("A", result.Section.Name);
                Assert.AreEqual("4", searchResult.ElementAt(0).Name);
            }
        }

        /*Tests if the regex split works as intended. Should identify A4 and split it to two search terms; A and 4.
         * Hereafter, it should work as the Search_3SearchForASPACE4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4 test.
         */
        [Test]
        public void Search_3SearchForA4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 3);
            var searchResult = searcher.Search("A4");

            foreach (var result in searchResult)
            {
                Assert.AreEqual("A", result.Section.Name);
                Assert.AreEqual("4", searchResult.ElementAt(0).Name);
            }
        }

        //Again tests the regex split function.
        [Test]
        public void Search_3SearchForA4_ExpectedResultTheSameAsSearchForASPACE4()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResultWithSpace = searcher.Search("A 4");
            var searchResultWithoutSpace = searcher.Search("A4");

            int length = searchResultWithSpace.ToArray().Length;

            if (searchResultWithoutSpace.ToArray().Length != length)
            {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: with space: {length}  without space: {searchResultWithoutSpace.ToArray().Length}");
            }

            for (int i = 0; i < length; i++)
            {
                if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true)
                {
                    Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
                }
            }
        }

        [Test]
        public void Search_10SearchForASPACE4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("A 4 Gr");

            foreach (var result in searchResult)
            {
                if (result.Section.Name == "A" || result.Name.Contains("4") || result.Grade.Name == "Green")
                {

                }
                else
                {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Grade Green\n  Was: {result.Section.Name}  {result.Name}  {result.Grade.Name}");
                }
            }
        }

        [Test]
        public void Search_10SearchForA4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("A4 Gr");

            foreach (var result in searchResult)
            {
                if (result.Section.Name == "A" || result.Name.Contains("4") || result.Grade.Name == "Green")
                {

                }
                else
                {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Grade Green\n  Was: {result.Section.Name}  {result.Name}  {result.Grade.Name}");
                }
            }
        }

        [Test]
        public void Search_10SearchForA4SPACEGr_ExpectedResultTheSameAsWithSpaceBetweenAAnd4()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResultWithoutSpace = searcher.Search("A4 Gr");
            var searchResultWithSpace = searcher.Search("A 4 Gr");

            int length = searchResultWithoutSpace.ToArray().Length;

            if (searchResultWithSpace.ToArray().Length != length)
            {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: WithSpace: {searchResultWithSpace.ToArray().Length}  noSpace: {length}");
            }

            for (int i = 0; i < length; i++)
            {
                if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true)
                {
                    Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
                }
            }
        }

        [Test]
        public void Search_10SearchForA4SPACEGru_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("A4 Gru");

            foreach (var result in searchResult)
            {
                if (result.Section.Name == "A" || result.Name.Contains("4") || result.Author.Contains("Gru"))
                {

                }
                else
                {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Author containing Gru\n  Was: {result.Section.Name}  {result.Name}  {result.Author}");
                }
            }
        }

        [Test]
        public void Search_10SearchForASPACE4SPACEGru_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResult = searcher.Search("A 4 Gru");

            foreach (var result in searchResult)
            {
                if (result.Section.Name == "A" || result.Name.Contains("4") || result.Author.Contains("Gru"))
                {

                }
                else
                {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Author containing Gru\n  Was: {result.Section.Name}  {result.Name}  {result.Author}");
                }
            }
        }

        [Test]
        public void Search_10SearchForA4SPACEGru_ExpectedResultTheSameAsWithSpaceBetweenAAnd4()
        {
            var testRepo = new TestDataFactory();
            var searcher = new RouteSearchService(testRepo.Routes, 10);
            var searchResultWithoutSpace = searcher.Search("A4 Gru");
            var searchResultWithSpace = searcher.Search("A 4 Gru");

            int length = searchResultWithoutSpace.ToArray().Length;

            if (searchResultWithSpace.ToArray().Length != length)
            {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: WithSpace: {searchResultWithSpace.ToArray().Length}  noSpace: {length}");
            }

            for (int i = 0; i < length; i++)
            {
                if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true)
                {
                    Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
                }
            }
        }
    }
}
