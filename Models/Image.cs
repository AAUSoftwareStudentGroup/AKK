using System;
using System.Collections.Generic;

namespace AKK.Models
{
    public class Image : Media
    {
        public Image()
        {
            Holds = new List<Hold>();
        }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public virtual List<Hold> Holds { get; set; }
    }
}