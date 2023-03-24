using System;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

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
    public class MainForm : Form {
        protected Drawable Canvas;

        private enum Direction {
            None = 0b0000,
            Up = 0b0001,
            Down = 0b0010,
            Left = 0b0100,
            Right = 0b1000,
            Window = 0b10000
        }

        private Galaxy galaxy;
        private Direction movement;
        private long lastTicks = 0;
        private Point windowPos = new Point();

        public MainForm() {
            JsonReader.Load(this);

            InitUI();
            SetupEventHandlers();

            galaxy = new Galaxy();
            movement = Direction.None;

            MainLoop();
        }

        private void MainLoop() {
            Task.Run(async () => {
                float fps = 30.0f;
                float interval = (float)Stopwatch.Frequency / fps;
                Stopwatch sw = Stopwatch.StartNew();
                long startTicks = sw.ElapsedTicks;

                while(true) {
                    if(sw.ElapsedTicks >= startTicks + interval) {
                        if(movement != Direction.None) {
                            ApplyOffset(0.5f, movement);
                            if((movement & Direction.Window) == Direction.Window) movement = Direction.None;

                            Application.Instance.Invoke(() => Canvas.Invalidate());
                        }
                    }

                    await Task.Delay(1);
                }
            });
        }

        private void InitUI() {
            this.Title = "Procedurally Generated Universe";
            this.ClientSize = new Size(800, 600);
            windowPos = this.Location;
        }

        private void SetupEventHandlers() {
            Canvas.Paint += (object s, PaintEventArgs e) => {
                lock(galaxy) {
                    long curTicks = DateTime.Now.Ticks;
                    galaxy.Render(e.Graphics, Canvas.Bounds);
                    lastTicks -= (curTicks - lastTicks);
                }
            };

            Canvas.KeyDown += (object s, KeyEventArgs e) => {
                if(e.Key == Keys.Up) movement |= Direction.Up;
                if(e.Key == Keys.Down) movement |= Direction.Down;
                if(e.Key == Keys.Left) movement |= Direction.Left;
                if(e.Key == Keys.Right) movement |= Direction.Right;
            };

            Canvas.KeyUp += (object s, KeyEventArgs e) => {
                if(e.Key == Keys.Up) movement ^= Direction.Up;
                if(e.Key == Keys.Down) movement ^= Direction.Down;
                if(e.Key == Keys.Left) movement ^= Direction.Left;
                if(e.Key == Keys.Right) movement ^= Direction.Right;
            };

            this.LocationChanged += (object s, EventArgs e) => {
                float incX = (this.Location.X - windowPos.X) / galaxy.SectorSize;
                float incY = (this.Location.Y - windowPos.Y) / galaxy.SectorSize;

                ApplyOffset(incX, Direction.Right);
                ApplyOffset(incY, Direction.Down);
                movement = Direction.Window;

                windowPos = this.Location;
            };
        }

        private void ApplyOffset(float offset, Direction m) {
            if((m & Direction.Left) == Direction.Left) galaxy.Offset.X -= offset;
            if((m & Direction.Right) == Direction.Right) galaxy.Offset.X += offset;
            if((m & Direction.Up) == Direction.Up) galaxy.Offset.Y -= offset;
            if((m & Direction.Down) == Direction.Down) galaxy.Offset.Y += offset;
        }
    }
}