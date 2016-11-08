using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AKK.Classes.Models {
    public class Grade {
        [Key]
        [JsonIgnore]
        public Guid GradeId  { get; set; }

        public int Difficulty { get; set; }

        public Color Color { get; set; }

        [JsonIgnore]
        public virtual List<Route> Routes { get; set; }
    }
}