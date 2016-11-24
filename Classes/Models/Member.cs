using System;
using System.Collections.Generic;
using AKK.Classes.Models.Repository;
using Newtonsoft.Json;

namespace AKK.Classes.Models
{
    public class Member : Model
    {
        public string DisplayName { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }

        public List<Route> Routes { get; set; }
    }
}