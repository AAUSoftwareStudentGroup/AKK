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
            var distance = IbsenSearchService._computeLevenshtein(pattern, text);
            Assert.GreaterOrEqual(distance, pattern.Length);

            distance = IbsenSearchService._computeLevenshtein(text, pattern);
            Assert.GreaterOrEqual(distance, pattern.Length);
        }

        //Asserts that the distance between certain words is correct according.
        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGreenAndMorten_DistanceIs9()
        {
            var distance = IbsenSearchService._computeLevenshtein("Green", "Morten");
            Assert.AreEqual(9, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAndGeogebra_DistanceIs5()
        { 
            var distance = IbsenSearchService._computeLevenshtein("Geo", "Geogebra");
            Assert.AreEqual(5, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGebraAndGeo_DistanceIs3()
        {
            var distance = IbsenSearchService._computeLevenshtein("Gebra", "Geogebra");
            Assert.AreEqual(3, distance);
        }

        [Test]
        public void _computeLevenshtein_CalculateDistanceBetweeenGeoAnd94_DistanceIs12()
        { 
            var distance = IbsenSearchService._computeLevenshtein("Geo", "94");
            Assert.AreEqual(12, distance);
        }

    }
}
