using System;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Rating : Model
    {
        public int RatingValue { get; set; }

        [JsonIgnore]
        public Route Route { get; set; }
        public Guid RouteId { get; set; }
        public Member Member { get; set; }
    }
}
