using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKK.Models {
    public enum Grades {Green, Blue, Red, Black, White};
    public class Route {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        
        public string Name { get; set; }
        
        public string Author { get; set; }
        
        public DateTime Date { get; set; }
        
        public uint ColorOfHolds { get; set; }
        
        public Grades Grade { get; set; }
        
        public Section Section { get; set; }

        public string SectionID { get; set; }
        }
}