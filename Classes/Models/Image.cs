using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
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