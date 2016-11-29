using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Media : Model
    {
        [JsonIgnore]
        public Route Route { get; set; }

        public Guid RouteId { get; set; }

        public string FileUrl { get; set; }
    }
}
