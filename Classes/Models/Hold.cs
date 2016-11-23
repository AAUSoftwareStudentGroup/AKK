using System;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
{
    public class Hold : IIdentifyable
    {
        public Guid Id { get; set; }
        
        public double X { get; set; }

        public double Y { get; set; }

        public double Radius { get; set; }
    }
}