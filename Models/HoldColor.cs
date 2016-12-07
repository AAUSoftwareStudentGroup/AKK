using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class HoldColor : Model
    {
        [JsonIgnore]
        public uint? HexColorOfHolds { get; set; }

        //Stores the Color in the database as a uint, and returns a Color based on that value
        [NotMapped]
        public Color ColorOfHolds
        {
            get { return Color.FromUint(HexColorOfHolds); }
            set { HexColorOfHolds = value?.ToUint(); }
        }

        public string Name { get; set; }
    }
}