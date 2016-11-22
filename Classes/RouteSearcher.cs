using System;
using AKK.Classes.Models;

namespace AKK.Classes
{
    internal static class RouteSearcher
    {
        internal static int computeDistance(Route route, string searchStr)
        {
            int[] distances = new int[4];

            distances[0] = computeLevenshtein(route.Author, searchStr);
            distances[1] = computeLevenshtein(route.Name, searchStr);
            distances[2] = computeLevenshtein(route.Grade.Name, searchStr);
            distances[3] = computeLevenshtein(route.Section.Name, searchStr);
            
            int smallestDist = int.MaxValue;

            for (int i = 0; i < distances.Length; i++) 
            {
                if (distances[i] < smallestDist) 
                {
                    smallestDist = distances[i];
                }
            }
            return smallestDist;
        }

        //Levenshtein algorithm copied from https://www.dotnetperls.com/levenshtein
        private static int computeLevenshtein(string s, string t) {
            s = s.ToLower();
            t = t.ToLower();

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) {}
            for (int j = 0; j <= m; d[0, j] = j++) {}

            for (int i = 1; i <= n; i++) {
                for (int j = 1; j <= m; j++) {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), 
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
