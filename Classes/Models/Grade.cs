using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using AKK.Classes.Models.Repository;
using Newtonsoft.Json;

namespace AKK.Classes.Models {
    public class Grade : IIdentifyable {
        protected uint? ColorDb { get; set; }
        [Key]
        [JsonIgnore]
        public Guid Id  { get; set; }

        public string Name { get; set; }

        public int Difficulty { get; set; }

        [NotMapped]
        public Color Color {
            get
            {
                return Color.FromUint(ColorDb);        
            }
            set
            {
                ColorDb = value.ToUint();
            }
        }

        [JsonIgnore]
        public virtual List<Route> Routes { get; set; }

        public static readonly Expression<Func<Grade, uint?>> ColorPriv = p => p.ColorDb;
    }
}