using System;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Comment : Model
    {
        public Guid RouteId { get; set; }

        [JsonIgnore]
        public Route Route { get; set; }

        public Guid MemberId { get; set; }

        [JsonIgnore]
        public Member Member { get; set; }

        public string Message { get; set; }
    }
}
