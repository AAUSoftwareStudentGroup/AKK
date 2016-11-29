using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Comments : Model
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
