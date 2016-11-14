using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKK.Classes.Models {
    public abstract class RouteInformation {

        protected uint? ColorOfHoldsDB { get; set; }
        protected uint? ColorOfTapeDB { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RouteId { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public Color ColorOfHolds { 
            get {
                return Color.FromUint(ColorOfHoldsDB);
            }
            set {
                ColorOfHoldsDB = value?.ToUint();
            }
        }
        [NotMapped]
        public Color ColorOfTape {
            get {
                return Color.FromUint(ColorOfTapeDB);
            }
            set {
                ColorOfTapeDB = value?.ToUint();
            }
        }

        public virtual Grade Grade { get; set; }

        public Guid SectionId { get; set; }

        [JsonIgnore]
        public Guid GradeId { get; set; }

        public bool PendingDeletion { get; set; }      

    }
}