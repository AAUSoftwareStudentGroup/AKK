using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AKK.Models;

namespace AKK.Services
{
    public class RouteSearchService : ISearchService<Route>
    {
        private readonly IEnumerable<Route> _allRoutes;
        private readonly int _numRoutes;

        public RouteSearchService(IEnumerable<Route> allRoutes)
        {
            _allRoutes = allRoutes;
            _numRoutes = allRoutes.Count();
        }

        public IEnumerable<Route> Search(string searchStr)
        {
            // Calculate levenstein distance
            var routeDistances = DistanceOfRoutes(searchStr);
            //Sorts routes by their Levenshtein distance.
            var sortedRoutes = routeDistances.OrderBy(x => x.Item2).ToList();

            //Returns a specific amount of routes and changes it from a list of Tuples to a list of routes.
            var threshold = 10;
            return GetRoutesInThreshold(sortedRoutes, threshold, _numRoutes);
        }

        private List<Tuple<Route, float>> DistanceOfRoutes(string searchStr)
        {
            //Split search string into searchterms
            var searchTerms = _splitSearchStr(searchStr);

            //Binds all routes together with an int value representing the Levenshtein distance.
            var routesWithDist = _allRoutes.Select(x => new Tuple<Route, float>(x, 0f)).ToList();

            //Calculates the Levenshtein distance for each search term and adds it to each route's specific Levenshtein distance value.
            string specialization = null;
            var specializationAge = 0;
            foreach (var searchTerm in searchTerms)
            {
                if (++specializationAge > 1)
                {
                    specialization = null;
                }
                switch (searchTerm.ToLower())
                {
                    case "author":
                    case "section":
                    case "grade":
                        specialization = searchTerm;
                        specializationAge = 0;
                        continue;
                }
                for (int i = 0; i < _numRoutes; i++)
                {
                    var route = routesWithDist[i].Item1;
                    float dist = routesWithDist[i].Item2 + _getSmallestDistance(route, searchTerm, specialization);
                    routesWithDist[i] = new Tuple<Route, float>(route, dist);
                }
            }

            // if string was splitted compare the distance of the sum of the splitted parts to the original input and take the lowest distance
            if (searchTerms.Count() != 1)
            {
                for (int i = 0; i < _numRoutes; i++)
                {
                    var route = routesWithDist[i].Item1;
                    float dist = _getSmallestDistance(route, searchStr, specialization);
                    if (dist < routesWithDist[i].Item2)
                    {
                        routesWithDist[i] = new Tuple<Route, float>(route, dist); ;
                    }
                }
            }
            return routesWithDist;
        }

        private List<Route> GetRoutesInThreshold(List<Tuple<Route, float>> routes, int threshold, int count)
        {
            var result = new List<Route>();

            for (int i = 0; i < _numRoutes; i++)
            {
                var el = routes[i];
                if (el.Item2 > threshold)
                {
                    break;
                }

                result.Add(el.Item1);
            }
            return result;
        }

        private IEnumerable<string> _splitSearchStr(string searchStr)
        {
            //Splits the input after every space to make it search for each word.
            var searchTerms = Regex.Split(searchStr.Trim(), @"\s+").ToList();
            var numSearchTerms = searchTerms.Count;

            //If a single letter is followed by a string of digits, split them into two searchterms.
            for (int i = 0; i < numSearchTerms; i++)
            {
                if (Regex.IsMatch(searchTerms[i], @"^[a-zA-Z]{1}[0-9]+"))
                {
                    searchTerms.Add(searchTerms[i].Substring(0, 1));
                    searchTerms.Add(searchTerms[i].Substring(1));
                    searchTerms.RemoveAt(i);
                }
            }
            return searchTerms;
        }

        private int _getSmallestDistance(Route route, string searchStr, string specialization)
        {
            if (!string.IsNullOrEmpty(specialization))
            {
                specialization = specialization.ToLower();
                switch (specialization)
                {
                    case "section":
                        return _computeLevenshtein(searchStr, route.SectionName);
                    case "author":
                        return _computeLevenshtein(searchStr, route.Author);
                    case "difficulty":
                    case "grade":
                        return _computeLevenshtein(searchStr, route.Grade.Name);
                }
            }

            int smallestDist = int.MaxValue;
            int[] distances =
            {
                _computeLevenshtein(searchStr, route.Name),
                _computeLevenshtein(searchStr, route.Section.Name),
                _computeLevenshtein(searchStr, route.Grade.Name),
                _computeLevenshtein(searchStr, route.Author)
            };

            foreach (int dist in distances)
            {
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                }
            }
            return smallestDist;
        }

        // Based on http://compbio.cs.uic.edu/~tanya/teaching/CompBio/scribe/TimLuciani_scribing-0119.v3.pdf and 
        // https://en.wikipedia.org/wiki/Wagner%E2%80%93Fischer_algorithm
        public static int _computeLevenshtein(string pattern, string text)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text))
            {
                return 0;
            }

            const int cost = 4;
            int m = pattern.Length;
            int n = text.Length;
            int[,] d = new int[m + 1, n + 1];

            pattern = pattern.ToLower();
            text = text.ToLower();

            //Fill first column
            for (int i = 0; i <= m; d[i, 0] = cost * i++) ;
            //Fill first row
            for (int j = 0; j <= n; d[0, j] = 1 * j++) ;

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    if (pattern[i - 1] == text[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1];
                    }
                    else
                    {
                        d[i, j] = Math.Min(
                            Math.Min(
                                d[i - 1, j] + cost,     //Deletion
                                d[i, j - 1] + 1),       //Insertion
                            d[i - 1, j - 1] + cost);    //Substitution
                    }
                }
            }
            return d[m, n];
        }
    }
}
