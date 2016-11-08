using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKK.Classes.Models {
    public abstract class RouteInformation {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RouteId { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public Color ColorOfHolds { get; set; }

        public Color ColorOfTape { get; set; }

        public virtual Grade Grade { get; set; }

        public Guid SectionId { get; set; }

        [JsonIgnore]
        public Guid GradeId { get; set; }

        public bool PendingDeletion { get; set; }
    
    }
}