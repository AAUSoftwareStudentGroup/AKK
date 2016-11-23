using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models {
    public abstract class RouteInformation : IIdentifyable{

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Grade Grade { get; set; }

        public Guid SectionId { get; set; }

        [JsonIgnore]
        public Guid GradeId { get; set; }

        public bool PendingDeletion { get; set; }


        protected uint? ColorOfHoldsDb { get; set; }

        protected uint? ColorOfTapeDb { get; set; }

        [NotMapped]
        public virtual string Author { get; set; }

        [NotMapped]
        public Color ColorOfHolds
        {
            get
            {
                return Color.FromUint(ColorOfHoldsDb);
            }
            set
            {
                ColorOfHoldsDb = value?.ToUint();
            }
        }
        [NotMapped]
        public Color ColorOfTape
        {
            get
            {
                return Color.FromUint(ColorOfTapeDb);
            }
            set
            {
                ColorOfTapeDb = value?.ToUint();
            }
        }


    }
}