using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKK.Classes.Models.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AKK.Classes.Models {
    public class Section : IIdentifyable{

        public Section()
        {
            Routes = new List<Route>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }    
            
        public virtual List<Route> Routes { get; set; }

    }
}