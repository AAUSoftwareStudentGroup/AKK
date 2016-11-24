using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKK.Models 
{
    public class Section : Model
    {
        public Section()
        {
            Routes = new List<Route>();
        }

        public Guid SectionId => Id;

        public string Name { get; set; }    
        
        [InverseProperty("Section")]
        public List<Route> Routes { get; set; }
    }
}