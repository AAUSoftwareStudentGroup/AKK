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

        public string Name { get; set; }    
        
        public List<Route> Routes { get; set; }
    }
}