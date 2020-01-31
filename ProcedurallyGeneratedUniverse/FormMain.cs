using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
	Procedural Generation: Programming The Universe
	"Here we go again! Year 4 begins now..." - javidx9

	License (OLC-3)
	~~~~~~~~~~~~~~~

	Copyright 2018-2020 OneLoneCoder.com

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions
	are met:

	1. Redistributions or derivations of source code must retain the above
	copyright notice, this list of conditions and the following disclaimer.

	2. Redistributions or derivative works in binary form must reproduce
	the above copyright notice. This list of conditions and the following
	disclaimer must be reproduced in the documentation and/or other
	materials provided with the distribution.

	3. Neither the name of the copyright holder nor the names of its
	contributors may be used to endorse or promote products derived
	from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
	"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
	LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
	A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
	DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
	THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
	OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

	Relevant Video: https://youtu.be/ZZY9YE7rZJw

	Links
	~~~~~
	YouTube:	https://www.youtube.com/javidx9
				https://www.youtube.com/javidx9extra
	Discord:	https://discord.gg/WhwHUMV
	Twitter:	https://www.twitter.com/javidx9
	Twitch:		https://www.twitch.tv/javidx9
	GitHub:		https://www.github.com/onelonecoder
	Patreon:	https://www.patreon.com/javidx9
	Homepage:	https://www.onelonecoder.com

	Author
	~~~~~~
	David Barr, aka javidx9, �OneLoneCoder 2018, 2019, 2020
*/

namespace ProcedurallyGeneratedUniverse {
    public partial class FormMain : Form {

        private enum Direction {
            None = 0b0000,
            Up = 0b0001,
            Down = 0b0010,
            Left = 0b0100,
            Right = 0b1000
        }

        private Galaxy galaxy;
        private Direction movement;
        private long lastTicks = 0;
        private Point windowPos = new Point();

        public FormMain() {
            InitializeComponent();

            InitUI();
            SetupEventHandlers();

            galaxy = new Galaxy();
            movement = Direction.None;

            // Although it is not as smooth, this is by far the best implementation...
            Task.Run(() => {
                while(true) {
                    Thread.Sleep(30);

                    if(movement != Direction.None) {
                        lock(galaxy) {
                            if((movement & Direction.Left) == Direction.Left) galaxy.Offset.X -= 1;
                            if((movement & Direction.Right) == Direction.Right) galaxy.Offset.X += 1;
                            if((movement & Direction.Up) == Direction.Up) galaxy.Offset.Y -= 1;
                            if((movement & Direction.Down) == Direction.Down) galaxy.Offset.Y += 1;
                        }

                        this.Invalidate();
                    }
                }
            });

            //Task.Run(() => {
            //    long curTicks;
            //    float inc;

            //    while(true) {
            //        curTicks = DateTime.Now.Ticks;
            //        Thread.Sleep(30);

            //        if(movement != Direction.None) {
            //            inc = 1;// (float)(30.0 * (curTicks - lastTicks) / 30000000.0);
            //            //int sx = 0;
            //            //int sy = 0;

            //            lock(galaxy) {
            //                if((movement & Direction.Left) == Direction.Left) galaxy.Offset.X -= inc;
            //                if((movement & Direction.Right) == Direction.Right) galaxy.Offset.X += inc;
            //                if((movement & Direction.Up) == Direction.Up) galaxy.Offset.Y -= inc;
            //                if((movement & Direction.Down) == Direction.Down) galaxy.Offset.Y += inc;

            //                // This "almost" fixes the freaking diagonal wobbling
            //                //  But when implemented, switching diagonal directions, the galaxy rapidly jumps to a previous
            //                //  location before stabilizing... WTF?

            //                //if((movement & Direction.Left) == Direction.Left) { galaxy.Offset.X -= inc; sx = -1; };
            //                //if((movement & Direction.Right) == Direction.Right) { galaxy.Offset.X += inc; sx = 1; };
            //                //if((movement & Direction.Up) == Direction.Up) { galaxy.Offset.Y -= inc; sy = -1; };
            //                //if((movement & Direction.Down) == Direction.Down) { galaxy.Offset.Y += inc; sy = 1; };

            //                //bool h = (movement & Direction.Left) == Direction.Left || (movement & Direction.Right) == Direction.Right;
            //                //bool v = (movement & Direction.Up) == Direction.Up || (movement & Direction.Down) == Direction.Down;

            //                //if(h & v) {
            //                //    float fpx = Math.Abs(galaxy.Offset.X) % 1.0f;
            //                //    float fpy = Math.Abs(galaxy.Offset.Y) % 1.0f;
            //                //    float fpm = Math.Max(fpx, fpy);

            //                //    galaxy.Offset.X = (int)galaxy.Offset.X + fpm * sx;
            //                //    galaxy.Offset.Y = (int)galaxy.Offset.Y + fpm * sy;
            //                //}
            //            }

            //            this.Invalidate();
            //        }
            //        lastTicks = curTicks;
            //    }
            //});
        }

        private void InitUI() {
            this.Text = "Procedurally Generated Universe";
            this.ClientSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw,
                          true);
            windowPos = this.Location;
        }

        private void SetupEventHandlers() {
            this.Paint += (object s, PaintEventArgs e) => {
                lock(galaxy) {
                    long curTicks = DateTime.Now.Ticks;
                    galaxy.Render(e.Graphics, this.DisplayRectangle);
                    lastTicks -= (curTicks - lastTicks);
                }
            };

            this.KeyDown += (object s, KeyEventArgs e) => {
                if(e.KeyCode == Keys.Up) movement |= Direction.Up;
                if(e.KeyCode == Keys.Down) movement |= Direction.Down;
                if(e.KeyCode == Keys.Left) movement |= Direction.Left;
                if(e.KeyCode == Keys.Right) movement |= Direction.Right;
            };

            this.KeyUp += (object s, KeyEventArgs e) => {
                if(e.KeyCode == Keys.Up) movement ^= Direction.Up;
                if(e.KeyCode == Keys.Down) movement ^= Direction.Down;
                if(e.KeyCode == Keys.Left) movement ^= Direction.Left;
                if(e.KeyCode == Keys.Right) movement ^= Direction.Right;
            };

            //this.Move += (object s, EventArgs e) => {
            //    int incX = this.Location.X - windowPos.X;
            //    int incY = this.Location.Y - windowPos.Y;
            //    if(incX != 0 || incY != 0) {
            //        lock(galaxy) {
            //            galaxy.Offset.X += incX / 5.0f;
            //            galaxy.Offset.Y += incY / 5.0f;
            //        }
            //        windowPos = this.Location;
            //        this.Invalidate();
            //    }
            //};
        }
    }
}