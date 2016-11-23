using System;
using System.ComponentModel.DataAnnotations;
using AKK.Classes.Models.Repository;
using Newtonsoft.Json;

namespace AKK.Classes.Models
{
    public class Hold : IIdentifyable
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ImageId { get; set; }

        [JsonIgnore]
        public Image Image { get; set; }
        
        public double X { get; set; }

        public double Y { get; set; }

        public double Radius { get; set; }
    }
}