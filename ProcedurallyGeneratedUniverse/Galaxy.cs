using System;
using Eto.Drawing;

namespace ProcedurallyGeneratedUniverse {
    public class Galaxy {
        private float sectorSize2;
        private float sectorSize4;
        private float sectorSize;
        private PointF screenSector = new PointF();

        public PointF Offset = new PointF();

        public float SectorSize {
            get => sectorSize;
            set {
                sectorSize = value;
                sectorSize2 = sectorSize / 2;
                sectorSize4 = sectorSize / 4;
            }
        }

        public Galaxy(int sectorSize = 16) {
            SectorSize = sectorSize;
        }

        public void Render(Graphics g, Rectangle r) {
            g.Clear(Colors.Black);

            float w = r.Width / sectorSize;
            float h = r.Height / sectorSize;

            screenSector = new PointF();

            for(screenSector.X = -sectorSize2; screenSector.X < w; screenSector.X++) {
                for(screenSector.Y = -sectorSize2; screenSector.Y < h; screenSector.Y++) {
                    UInt32 seed1 = (UInt32)(Offset.X + screenSector.X);
                    UInt32 seed2 = (UInt32)(Offset.Y + screenSector.Y);

                    using(StarSystem star = new StarSystem(seed1, seed2)) {
                        if(star.Exists) {
                            g.FillEllipse(star.ForeColor,
                                            screenSector.X * sectorSize + sectorSize2,
                                            screenSector.Y * sectorSize + sectorSize2,
                                            star.Diameter / sectorSize4,
                                            star.Diameter / sectorSize4);
                        }
                    }
                }
            }
        }
    }
}