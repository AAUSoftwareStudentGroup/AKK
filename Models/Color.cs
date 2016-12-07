namespace AKK.Models
{
    public class Color
    {
        public byte R { get; set; }

        public byte G { get; set; }
        
        public byte B { get; set; }

        public byte A { get; set; } = 255;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color()
        { }

        //Converts the Color object to an unsigned integer based on the Color object's R, G, B and A values
        public uint? ToUint()
        {
            uint? col = 0;
            col += R;
            col = (col << 8) + G;
            col = (col << 8) + B;
            col = (col << 8) + A;
            return col;
        }
        
        //Converts an unsigned integer to a Color object with R, G, B and A values
        public static Color FromUint(uint? col)
        {
            if (col == null) return null;
            Color c = new Color();
            c.R = (byte)((col & 0xFF000000) >> 24);
            c.G = (byte)((col & 0x00FF0000) >> 16);
            c.B = (byte)((col & 0x0000FF00) >> 8);
            c.A = (byte)(col & 0x000000FF);
            return c;
        }
    }
}
