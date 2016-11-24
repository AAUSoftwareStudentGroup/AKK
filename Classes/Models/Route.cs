using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using AKK.Classes.Models;
using Newtonsoft.Json;

namespace AKK.Classes.Models
{
    public enum SortOrder
    {
        Newest, Oldest, Grading, Author
    }

    public class Route : Model
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        public override bool Equals (object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Route input = (Route) obj;

            return Author == input.Author && Id == input.Id && Name == input.Name 
                && CreatedDate == input.CreatedDate && Grade == input.Grade && SectionId == input.SectionId;
        }
        
        public override int GetHashCode()
        {
            return int.Parse(Name);
        }

        public Guid MemberId { get; set; }

        [JsonIgnore]
        public Member Member { get; set; }

        public Guid SectionId { get; set; }

        [JsonIgnore]
        public Section Section { get; set; }

        [NotMapped]
        public string Author => Member?.DisplayName;

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
