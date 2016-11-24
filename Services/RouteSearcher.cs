﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AKK.Classes.Models;

namespace AKK.Classes
{
    public class RouteSearcher
    {
        private int _maxResults;
        private int _numRoutes;
        private IEnumerable<Route> _allRoutes;

        public RouteSearcher(IEnumerable<Route> allRoutes, int maxResults)
        {
            _allRoutes = allRoutes;

            _numRoutes = _allRoutes.Count();
            _maxResults = maxResults <= 0 ? _numRoutes : Math.Min(maxResults, _numRoutes);
        }

        public IEnumerable<Route> Search(string searchStr)
        {
            //var searchTerms = searchStr.Split(' ');
            var searchTerms = Regex.Split(searchStr, @"\s{1,}").ToList();
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

            //Binds all routes together with an int value representing the Levenshtein distance.
            List<Tuple<Route, float>> routesWithDist = new List<Tuple<Route, float>>();
            for (int i = 0; i < _numRoutes; i++) 
            {
                routesWithDist.Add(new Tuple<Route, float>(_allRoutes.ElementAt(i), 0));
            }

            //Calculates the Levenshtein distance for each search term and adds it to each route's specific Levenshtein distance value.
            foreach (var searchTerm in searchTerms) {
                var searchTermLength = searchTerm.Length;

                for (int i = 0; i < _numRoutes; i++) {
                    var route = routesWithDist[i].Item1;
                    float dist = routesWithDist[i].Item2 + _getSmallestDistance(route, searchTerm) / (float)searchTermLength;

                    routesWithDist[i] = new Tuple<Route, float>(route, dist);
                }
            }

            //Sorts routes by their Levenshtein distance.
            var sortedRoutes = routesWithDist.OrderBy(x => x.Item2);

            //Returns a specific amount of routes and changes it from a list of Tuples to a list of routes.
            var foundRoutes = new List<Route>();
            for (int i = 0; i < _maxResults; i++) {
                var el = sortedRoutes.ElementAt(i);
                if (el.Item2 > 2)
                    break;

                foundRoutes.Add(el.Item1);
            }

            return foundRoutes;
        }

        private int _getSmallestDistance(Route route, string searchStr)
        {
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
                    smallestDist = dist;
            }
            return smallestDist;
        }

        // Based on http://compbio.cs.uic.edu/~tanya/teaching/CompBio/scribe/TimLuciani_scribing-0119.v3.pdf and 
        // https://en.wikipedia.org/wiki/Wagner%E2%80%93Fischer_algorithm
        public int _computeLevenshtein(string pattern, string text) {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text))
                return 0;
 
            const int cost = 4;
            int m = pattern.Length;
            int n = text.Length;
            int[,] d = new int[m + 1, n + 1];

            pattern = pattern.ToLower();
            text = text.ToLower();

            //Fill first column
            for (int i = 0; i <= m; d[i, 0] = cost * i++);
            //Fill first row
            for (int j = 0; j <= n; d[0, j] = 1 * j++);

            for (int j = 1; j <= n; j++) {
                for (int i = 1; i <= m; i++)
                {
                    if (pattern[i - 1] == text[j - 1])
                        d[i, j] = d[i - 1, j - 1];
                    else
                        d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + cost,  //Deletion
                            d[i, j - 1] + 1),    //Insertion
                        d[i - 1, j - 1] + cost); //Substitution
                }
            }
            return d[m, n];
        }
    }
}