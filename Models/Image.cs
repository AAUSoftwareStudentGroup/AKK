using System;
using System.Collections.Generic;

namespace AKK.Models
{
    public class Image : Model
    {
        public Image()
        {
            Holds = new List<Hold>();
        }

        public Guid RouteId { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public string FileUrl { get; set; }

        public virtual List<Hold> Holds { get; set; }
    }
}