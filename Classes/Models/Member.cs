using System;
using System.Collections.Generic;
using AKK.Classes.Models.Repository;

namespace AKK.Classes.Models
{
    public class Member : IIdentifyable
    {
        public Guid Id { get; set; }
        
        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public List<Route> Routes { get; set; }
    }
}