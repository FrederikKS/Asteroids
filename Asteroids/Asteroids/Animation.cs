using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    class Animation
    {
        /// <summary>
        /// Animation offset
        /// </summary>
        private Vector2 offset;

        /// <summary>
        /// Animation fps
        /// </summary>
        private float fps;

        /// <summary>
        /// Animation rectangles
        /// </summary>
        private Rectangle[] rectangles;

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Rectangle[] Rectangles
        {
            get { return rectangles; }
            set { rectangles = value; }
        }

        public float Fps
        {
            get { return fps; }
            set { fps = value; }
        }

        public Color[][] colors;

        /// <summary>
        /// The animation constructor
        /// </summary>
        /// <param name="frames">Number of frames in the animation</param>
        /// <param name="yPos">Y position on the sprite sheet in pixels</param>
        /// <param name="xStartFrame">X position on the sprite sheet in frames</param>
        /// <param name="width">The width of each frame</param>
        /// <param name="height">The height of each frame</param>
        /// <param name="offset">Animation offset(can be used to align animations)</param>
        /// <param name="fps">Animation fps</param>
        public Animation(int frames, int yPos, int xStartFrame, int width, int height, Vector2 offset, float fps, Color[] colorData, int textureWidth)
        {
            colors = new Color[frames][];

            rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);

                colors[i] = new Color[rectangles[i].Width * rectangles[i].Height];

                for (int x = 0; x < rectangles[i].Width; x++)
                {
                    for (int y = 0; y < rectangles[i].Height; y++)
                    {
                        int partIndex = x + y * rectangles[i].Width;
                        colors[i][partIndex] = colorData[(x + rectangles[i].X) + (y + rectangles[i].Y) * textureWidth];
                    }
                }
            }

            this.fps = fps;
            this.offset = offset;
        }
    }
}
