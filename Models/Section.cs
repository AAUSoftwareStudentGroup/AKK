using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AKK.Models {
    public class Section {

        public Section()
        {
            Routes = new List<Route>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SectionId { get; set; }

        public string Name { get; set; }    
            
        public List<Route> Routes { get; set; }

    }
}