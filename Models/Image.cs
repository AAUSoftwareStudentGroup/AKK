using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Image : Media
    {
        public Image()
        {
            Holds = new List<Hold>();
            Id = Guid.NewGuid();
        }

        [JsonIgnore]
        public Route Route { get; set; }

        public Guid RouteId { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public virtual List<Hold> Holds { get; set; }
    }
}