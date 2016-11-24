using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using AKK.Classes.Models.Repository;
using Newtonsoft.Json;

namespace AKK.Classes.Models 
{
    public class Grade : Model 
    {
        public string Name { get; set; }

        public int Difficulty { get; set; }

        public uint? HexColor { get; set; }

        [NotMapped]
        public Color Color 
        {
            get
            {
                return Color.FromUint(HexColor);        
            }
            set
            {
                HexColor = value.ToUint();
            }
        }

        [JsonIgnore]
        public List<Route> Routes { get; set; }
    }
}