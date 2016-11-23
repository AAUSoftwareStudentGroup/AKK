using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace AKK.Classes.Models
    {
    public enum SortOrder { Newest, Oldest, Grading, Author };

    public class Route : RouteInformation
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        public override bool Equals (object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Route input = (Route) obj;

            if(Author == input.Author &&
               Id == input.Id &&
               Name == input.Name &&
               CreatedDate == input.CreatedDate &&
               Grade == input.Grade &&
               SectionId == input.SectionId) {
                return true;
            } else {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return int.Parse(Name);
        }
        [JsonIgnore]
        public virtual Section Section { get; set; }

        public static readonly Expression<Func<Route, uint?>> ColorOfHoldsPriv = p => p.ColorOfHoldsDb;
        public static readonly Expression<Func<Route, uint?>> ColorOfTapePriv = p => p.ColorOfTapeDb;
    }
}
