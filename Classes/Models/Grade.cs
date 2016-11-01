using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKK.Classes.Models {
    public class Grade {
        [Key]
        public Guid GradeId  { get; set; }

        public int Difficulty { get; set; }

        public Color Color { get; set; }

        public virtual List<Route> Routes { get; set; }
    }
}