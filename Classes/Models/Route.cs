using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace AKK.Classes.Models
{
    public enum Grades { Green, Blue, Red, Black, White };
    public enum SortOrder { Newest, Oldest, Grading, Author };

    public class Route : RouteInformation
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        [JsonIgnore]
        public virtual Section Section { get; set; }

        public static readonly Expression<Func<Route, uint?>> ColorOfHoldsPriv = p => p.ColorOfHoldsDb;
        public static readonly Expression<Func<Route, uint?>> ColorOfTapePriv = p => p.ColorOfTapeDb;

    }
}
