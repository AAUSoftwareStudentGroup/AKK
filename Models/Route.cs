using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AKK.Models
{
    public enum SortOrder
    {
        Newest, Oldest, Grading, Author
    }

    public class Route : Model, ICloneable<Route>
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        public override bool Equals (object obj)
        {
            Route route = obj as Route;

            if (obj == null || route == null)
            {
                return false;
            }

            return Id == route.Id && Name == route.Name && CreatedDate == route.CreatedDate 
                && GradeId == route.GradeId && SectionId == route.SectionId && MemberId == route.MemberId;
        }
        
        public override int GetHashCode()
        {
            return int.Parse(Name);
        }

        public Route Clone()
        {
            return JsonConvert.DeserializeObject<Route>(JsonConvert.SerializeObject(this));
        }

        public Guid MemberId { get; set; }

        [JsonIgnore]
        public Member Member { get; set; }

        public Guid SectionId { get; set; }

        [JsonIgnore]
        public Section Section { get; set; }

        public string Author { get; set; }

        [NotMapped]
        public string SectionName => Section?.Name;

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public Grade Grade { get; set; }

        public Guid GradeId { get; set; }

        public bool PendingDeletion { get; set; }

        public Image Image { get; set; }

        [JsonIgnore]
        public uint? HexColorOfHolds { get; set; }

        [JsonIgnore]
        public uint? HexColorOfTape { get; set; }

        [NotMapped]
        public Color ColorOfHolds
        {
            get
            {
                return Color.FromUint(HexColorOfHolds);
            }
            set
            {
                HexColorOfHolds = value?.ToUint();
            }
        }

        [NotMapped]
        public Color ColorOfTape
        {
            get
            {
                return Color.FromUint(HexColorOfTape);
            }
            set
            {
                HexColorOfTape = value?.ToUint();
            }
        }
    }
}
