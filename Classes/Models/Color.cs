using System;
using Newtonsoft.Json;

namespace AKK.Classes.Models {
    public class Color {

        [JsonIgnore]
        public Guid ColorId { get; set; }

        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public float a { get; set; } = 1f;

        public Color (byte r, byte g, byte b)
        {
          this.r = r;
          this.g = g;
          this.b = b;
        }

        public Color(UInt32? color) {
            if(color != null) {
                this.r = (byte)(color>>24);
                this.g = (byte)(color>>16);
                this.b = (byte)(color>> 8);
                this.a = (byte)(color>> 0);
            }
        }
        
        public Color()
        {}

    }
}