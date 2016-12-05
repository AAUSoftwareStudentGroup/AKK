using System.Linq;
using NUnit.Framework;
using AKK.Services;

namespace AKK.Tests
{
    [TestFixture]
    public class TestRouteSearcher
    {
        private TestDataFactory _dataFactory;
        private RouteSearchService _searcher;

        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _dataFactory = new TestDataFactory();
            _searcher = new RouteSearchService(_dataFactory.Routes);
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _searcher = null;
            _dataFactory = null;
        }

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
        public void Search_SearchForGreen_RoutesWithGradeGreen()
        {
            var searchResult = _searcher.Search("grade green");

            var routes = searchResult.ToArray();
            for (int i = 0; i < 4; i++)
                Assert.AreEqual("Green", routes[i].Grade.Name);
        }

        [Test]
        public void Search_SearchFor3Red_RoutesWithGradeRed()
        {
            var searchResult = _searcher.Search("grade red").ToArray();

            for (int i = 0; i < 3; i++)
                Assert.AreEqual("Red", searchResult[i].Grade.Name);
        }

        [Test]
        public void Search_SearchFor2Anto_RoutesWithAuthorAnton()
        {
            var searchResult = _searcher.Search("anto");

            var routes = searchResult.ToArray();
            for (int i = 0; i < 2; i++)
                Assert.AreEqual("Anton", routes[i].Author);
        }

        //Asserts that by searching for 'Geo', the route made by Geo will show before the route made by Geogebra.
        [Test]
        public void Search_SearchFor10Geo_RoutesWithAuthorGeoAndGeogebraInOrder()
        {
            var searchResult = _searcher.Search("geo");

            Assert.AreEqual("Geo", searchResult.ElementAt(0).Author);
            Assert.AreEqual("Geogebra", searchResult.ElementAt(1).Author);
        }

        /*Asserts that searching for a substring of the original string will work - also in cases where the substring does not
         * include the beginning of the original string
         */
        [Test]
        public void Search_SearchFor10Gebra_RoutesWithAuthorGeogebraFirst()
        {
            var searchResult = _searcher.Search("gebra");

            if (!searchResult.Any())
            {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("Geogebra", searchResult.FirstOrDefault().Author);
        }

        [Test]
        public void Search_10SearchForHelland_ExpectRouteWithTannerHellandAsAuthor()
        {
            var searchResult = _searcher.Search("Helland");
            if (!searchResult.Any())
            {
                Assert.Fail($"  Expected occupied list\n  But was Empty");
            }
            Assert.AreEqual("TannerHelland", searchResult.ElementAt(0).Author);
        }

        //Asserts that the calculation of Levenshtein works. The black route is meant to show up since it's threshold is 2.
        [Test]
        public void Search_SearchFor8Bl_8RoutesWithGradesBlueAndBlack()
        {
            var searchResult = _searcher.Search("Blu");

            var routes = searchResult.ToArray();
            for (int i = 0; i < 8; i++)
            {
                var result = routes[i];
                if (result.Grade.Name == "Black" || result.Grade.Name == "Blue")
                {
                }
                else
                {
                    Assert.Fail($"  Expected: grade of Blue or Black\n  But grade was: {result.Grade.Name}.");
                }
            }
        }

        /*Tests if the regex split works as intended. Should identify A4 and split it to two search terms; A and 4.
         * Hereafter, it should work as the Search_3SearchForASPACE4_ExpectedRoutesFromSectionAWithRouteNumbersContaining4 test.
         */

        [Test]
        public void Search_SearchForA4_ExpectedResultTheSameAsSearchForASPACE4()
        {
            var searchResultWithSpace = _searcher.Search("A 4");
            var searchResultWithoutSpace = _searcher.Search("A4");

            int length = searchResultWithSpace.ToArray().Length;

            if (searchResultWithoutSpace.ToArray().Length != length)
            {
                Assert.Fail($"  Expected: List have the same amount of items\n  Was: with space: {length}  without space: {searchResultWithoutSpace.ToArray().Length}");
            }

            for (int i = 0; i < length; i++)
            {
                if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true)
                {
                    Assert.Fail($"  Expected Author: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
                }
            }
        }

        [Test]
        public void Search_4SearchForASPACE4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade()
        {
            var searchResult = _searcher.Search("A 4 Gr");

            var routes = searchResult.ToArray();

            for (int i = 0; i < routes.Length; i++)
            {
                var result = routes[i];
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
        public void Search_4SearchForA4SPACEGr_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndGreenGrade()
        {
            var searchResult = _searcher.Search("A4 Gr");

            var routes = searchResult.ToArray();
            for (int i = 0; i < routes.Length; i++)
            {

                if (routes[i].Section.Name == "A" || routes[i].Name.Contains("4") || routes[i].Grade.Name == "Green")
                {

                }
                else
                {
                    Assert.Fail($"Expected: Section A, Route Number containing 4 or Grade Green\n  Was: {routes[i].Section.Name}  {routes[i].Name}  {routes[i].Grade.Name}");
                }
            }
        }

        [Test]
        public void Search_SearchForA4SPACEGr_ExpectedResultTheSameAsWithSpaceBetweenAAnd4()
        {
            var searchResultWithoutSpace = _searcher.Search("A4 Gr");
            var searchResultWithSpace = _searcher.Search("A 4 Gr");

            int length = searchResultWithoutSpace.ToArray().Length;

            for (int i = 0; i < length; i++)
            {
                if (searchResultWithSpace.ElementAt(i).Equals(searchResultWithoutSpace.ElementAt(i)) != true)
                {
                    Assert.Fail($"  Expected Id: {searchResultWithSpace.ElementAt(i).Author}\n  Was: {searchResultWithoutSpace.ElementAt(i).Author}");
                }
            }
        }

        [Test]
        public void Search_4SearchForA4SPACEGru_Expected2RoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg()
        {
            var searchResult = _searcher.Search("A4 Gru");

            var routes = searchResult.ToArray();
            for (int i = 0; i < routes.Length; i++)
            {
                var result = routes[i];
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
        public void Search_4SearchForASPACE4SPACEGru_ExpectedRoutesFromSectionAWithRouteNumbersContaining4AndAuthorGrunberg()
        {
            var searchResult = _searcher.Search("A 4 Gru");

            var routes = searchResult.ToArray();
            for (int i = 0; i < routes.Length; i++)
            {
                var result = routes[i];
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
        public void Search_SearchForA4SPACEGru_ExpectedResultTheSameAsWithSpaceBetweenAAnd4()
        {
            var searchResultWithoutSpace = _searcher.Search("A4 Gru");
            var searchResultWithSpace = _searcher.Search("A 4 Gru");

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
        public void _Search_SearchForSectionSPACEC_ExpectTheFirstRoutesToBeOfSectionA()
        {
            var searchResult = _searcher.Search("section c").ToArray();

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual("C", searchResult[i].SectionName);
            }
        }
    }
}
