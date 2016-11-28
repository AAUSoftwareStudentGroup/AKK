using System;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Hold : Model
    {
        public Guid ImageId { get; set; }

        [JsonIgnore]
        public Image Image { get; set; }
        
        public double X { get; set; }

        public double Y { get; set; }

        public double Radius { get; set; }
    }
}