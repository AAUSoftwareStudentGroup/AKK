using System;
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
    }
}
