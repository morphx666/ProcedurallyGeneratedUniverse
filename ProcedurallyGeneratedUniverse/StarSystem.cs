using System;
using Eto.Drawing;

namespace ProcedurallyGeneratedUniverse {
    public class StarSystem : IDisposable {
        private Color[] colors = {Colors.White, Colors.Blue, Colors.Green, Colors.Red,
                                  Colors.Violet, Colors.Yellow, Colors.Cyan, Colors.DeepSkyBlue,
                                  Colors.Chocolate, Colors.DarkSlateBlue, Colors.LightGreen};
        private UInt32 procGen = 0;

        public bool Exists { get; private set; } = false;
        public float Diameter { get; private set; } = 0;
        public Brush ForeColor { get; private set; } = Brushes.White;

        public StarSystem(UInt32 x, UInt32 y) {
            // Set seed based on location of star system
            procGen = (x & 0xFFFF) << 16 | (y & 0xFFFF);

            // Not all locations contain a system
            Exists = (RndInt(0, 45) == 1);
            if(!Exists) return;

            // Generate Star
            Diameter = RndFloat(10.0f, 40.0f);
            ForeColor = new SolidBrush(colors[RndInt(0, colors.Length)]);
        }

        private float RndFloat(float min, float max) {
            return ((float)Rnd() / 0x7FFFFFFF) * (max - min) + min;
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

        public void Dispose() {
            ForeColor.Dispose();
        }
    }
}