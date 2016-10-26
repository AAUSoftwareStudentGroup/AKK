using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKK.Models {
    public class Section {
        
        [Key]
        public string Name { get; set; }

        public List<Route> Routes { get; set; } = new List<Route>();

    }
}