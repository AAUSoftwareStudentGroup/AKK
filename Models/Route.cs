using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.DotNet.Cli.Utils;
using Newtonsoft.Json;

namespace AKK.Models
{
    public enum Grades { Green, Blue, Red, Black, White };
    public enum SortOrder { Newest };

    public class Route
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RouteId { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public uint ColorOfHolds { get; set; }

        public Grades Grade { get; set; }

        [JsonIgnore]
        public Section Section { get; set; }

        public Guid SectionId { get; set; }
    }
}