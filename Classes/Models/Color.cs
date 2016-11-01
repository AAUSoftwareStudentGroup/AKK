using System;
namespace AKK.Classes.Models {
    public class Color {

        public Guid ColorId { get; set; }

        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte a { get; set; }

        public Color (byte r, byte g, byte b)
        {
          this.r = r;
          this.g = g;
          this.b = b;
          this.a = 255;
        }
        
        public Color()
        {}

    }
}