using System;
using System.Drawing;

namespace ProcedurallyGeneratedUniverse {
    public class StarSystem {
        private Color[] colors = {Color.White, Color.Blue, Color.Green, Color.Red,
                                  Color.Violet, Color.Yellow, Color.Cyan, Color.DeepSkyBlue,
                                  Color.Chocolate, Color.DarkSlateBlue, Color.LightGreen};
        private UInt32 procGen = 0;

        public bool Exists = false;
        public double Diameter = 0.0;
        public Brush ForeColor = Brushes.White;

        public StarSystem(UInt32 x, UInt32 y, bool generateFullSystem = false) {
            // Set seed based on location of star system
            procGen = (x & 0xFFFF) << 16 | (y & 0xFFFF);

            // Not all locations contain a system
            Exists = (RndInt(0, 30) == 1);
            if(!Exists) return;

            // Generate Star
            Diameter = RndDouble(10.0, 40.0);
            ForeColor = new SolidBrush(colors[RndInt(0, colors.Length)]);

            // When viewing the galaxy map, we only care about the star
            // so abort early
            if(!generateFullSystem) return;

            // If we are viewing the system map, we need to generate the
            // full system

            // TODO
        }

        private double RndDouble(double min, double max) {
            return ((double)Rnd() / 0x7FFFFFFF) * (max - min) + min;
        }

        private int RndInt(int min, int max) {
            return (int)(Rnd() % (max - min)) + min;
        }

        // https://en.wikipedia.org/wiki/Lehmer_random_number_generator
        private UInt32 Rnd() {
            procGen += 0xe120fc15;
            UInt64 tmp = (UInt64)procGen * 0x4a39b70d;
            UInt32 m1 = (UInt32)((tmp >> 32) ^ tmp);
            tmp = (UInt64)m1 * 0x12fad5c9;
            UInt32 m2 = (UInt32)((tmp >> 32) ^ tmp);
            return m2;
        }
    }
}