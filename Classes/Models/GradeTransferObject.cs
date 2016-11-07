using System;
namespace AKK.Classes.Models {
    public class GradeTransferObject {
        public Guid GradeId  { get; set; }

        public int Difficulty { get; set; }

        public Color Color { get; set; }
    }
}