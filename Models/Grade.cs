using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AKK.Models 
{
    public class Grade : Model 
    {
        public string Name { get; set; }

        public int? Difficulty { get; set; }

        [JsonIgnore]
        public uint? HexColor { get; set; }

        //Stores the Color in the database as a uint, and returns a Color based on that value
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