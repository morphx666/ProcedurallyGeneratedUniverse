using System;
using System.Diagnostics;
using System.Drawing;

namespace ProcedurallyGeneratedUniverse {
    public class Galaxy {
        private int sectorSize = 16;
        private int sectorSize2;
        private int sectorSize4;
        public PointF Offset = new PointF(0.0f, 0.0f);

        public Galaxy() {
            sectorSize2 = sectorSize / 2;
            sectorSize4 = sectorSize / 4;
        }

        public void Render(Graphics g, Rectangle r) {
            g.Clear(Color.Black);

            float w = r.Width / sectorSize;
            float h = r.Height / sectorSize;

            Point screenSector = new Point(0, 0);

            for(screenSector.X = -(int)sectorSize2; screenSector.X < w; screenSector.X++) {
                for(screenSector.Y = -(int)sectorSize2; screenSector.Y < h; screenSector.Y++) {
                    UInt32 seed1 = (UInt32)(Offset.X + screenSector.X);
                    UInt32 seed2 = (UInt32)(Offset.Y + screenSector.Y);

                    StarSystem star = new StarSystem(seed1, seed2);
                    if(star.Exists) {
                        g.FillEllipse(star.ForeColor,
                                        screenSector.X * sectorSize + sectorSize2,
                                        screenSector.Y * sectorSize + sectorSize2,
                                        (float)(star.Diameter / sectorSize4),
                                        (float)(star.Diameter / sectorSize4));
                    }
                }
            }
        }
    }
}