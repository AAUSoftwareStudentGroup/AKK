using System;
using Newtonsoft.Json;

namespace AKK.Classes.Models
    {
    public enum SortOrder { Newest };

    public class Route : RouteInformation
    {
        public Route()
        {
            CreatedDate = DateTime.Now.Date;
        }

        [JsonIgnore]
        public virtual Section Section { get; set; }        
    }
}