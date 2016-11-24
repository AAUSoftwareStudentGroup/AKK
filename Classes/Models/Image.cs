using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKK.Classes.Models.Repository;
using Newtonsoft.Json;

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

        public List<Hold> Holds { get; set; }
    }
}