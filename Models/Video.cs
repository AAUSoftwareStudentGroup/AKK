using System;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Video : Media
    {
        [JsonIgnore]
        public Member Member { get; set; }

        public Guid MemberId { get; set; }

    }
}
