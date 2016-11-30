using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class HoldColor : Model
    {
        [JsonIgnore]
        public uint? HexColorOfHolds { get; set; }

        [NotMapped]
        public Color ColorOfHolds
        {
            get { return Color.FromUint(HexColorOfHolds); }
            set { HexColorOfHolds = value?.ToUint(); }
        }

        public string Name { get; set; }
    }
}