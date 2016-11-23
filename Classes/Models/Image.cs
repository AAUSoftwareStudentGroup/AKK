using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
{
    public class Image : IIdentifyable
    {
        [Key]
        public Guid Id { get; set; }
        
        public uint Width { get; set; }

        public uint Height { get; set; }

        public string FileUrl { get; set; }

        public List<Hold> Holds { get; set; }
    }
}