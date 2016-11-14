using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace AKK.Classes.Models {
    public class Grade {
        protected uint? ColorDB { get; set; }

        [Key]
        [JsonIgnore]
        public Guid GradeId  { get; set; }

        public string Name { get; set; }

        public int Difficulty { get; set; }

        [NotMapped]
        public Color Color { 
            get {
                return Color.FromUint(ColorDB);
            } 
            set {
                ColorDB = value?.ToUint();
            } 
        }

        [JsonIgnore]
        public virtual List<Route> Routes { get; set; }
        // https://weblogs.asp.net/ricardoperes/mapping-non-public-members-with-entity-framework-code-first
        public static readonly Expression<Func<Grade, uint?>> ColorPriv = p => p.ColorDB;  
    }
}