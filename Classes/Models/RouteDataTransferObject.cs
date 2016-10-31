using System;

namespace AKK.Classes.Models
    {
    public class RouteDataTransferObject
    {
        public Guid RouteId { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public uint ColorOfHolds { get; set; }

        public Grades Grade { get; set; }

        public Guid SectionId { get; set; }

        public string SectionName { get; set; }

    }
}