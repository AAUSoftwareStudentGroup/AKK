using System;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Comment : Model
    {
        public Comment()
        {
            Date = DateTime.Now;
        }

        public Guid RouteId { get; set; }

        [JsonIgnore]
        public Route Route { get; set; }

        public Guid MemberId { get; set; }

        public Member Member { get; set; }

        public string Message { get; set; }

        public Video Video { get; set; }

        public DateTime Date {get; set;}
    }
}
