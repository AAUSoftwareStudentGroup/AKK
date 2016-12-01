using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

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
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
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
            return Name.GetHashCode();
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

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        public Grade Grade { get; set; }

        public Guid GradeId { get; set; }

        //This should be removed from DB
        [JsonIgnore]
        public bool PendingDeletion { get; set; }

        [JsonIgnore]
        public Image Image { get; set; }

        [NotMapped]
        public bool HasImage => Image != null; 

        public List<Comment> Comments { get; set; }

        [NotMapped]
        public bool HasBeta => Comments.Any(c => c.Video != null);

        [JsonIgnore]
        public List<Rating> Ratings { get; set; }

        [NotMapped]
        public float? AverageRating
        {
            get
            {
                //Checks if empty
                if (Ratings == null || !Ratings.Any())
                {
                    return null;
                }

                //If not empty, return average of ratings. Typecasts to float to avoid integer division
                return (float)Ratings.Sum(x => x.RatingValue) / Ratings.Count();
            }
        }

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
