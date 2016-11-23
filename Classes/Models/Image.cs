using System;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
{
    public class Image : IIdentifyable
    {
        public Guid Id { get; set; }
        
        public uint Width { get; set; }

        public uint Height { get; set; }

        public string FileUrl { get; set; }
    }
}