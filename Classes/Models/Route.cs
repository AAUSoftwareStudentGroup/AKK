using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace AKK.Classes.Models
{
    public enum Grades { Green, Blue, Red, Black, White };
    public enum SortOrder { Newest, Oldest, Grading, Author };

    public class Route : Model
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
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

        [JsonIgnore]
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
