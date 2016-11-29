using System.Collections.Generic;
using Newtonsoft.Json;

namespace AKK.Models
{
    public class Member : Model
    {
        public string DisplayName { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }

        [JsonIgnore]
        public List<Route> Routes { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}