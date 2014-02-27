using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids
{
    class Powerup : GameObject
    {
        // Fields
        private int type;

        // Properties
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        // Constructor

        public Powerup(Vector2 startPos, int type) : base(startPos)
        {
            this.type = type;
        }

        /// <summary>
        /// Loads content for powerup, depending on randomized type
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            STexture = content.Load<Texture2D>("Crystals.png");
            Color[] colorData = new Color[STexture.Width * STexture.Height];
            STexture.GetData<Color>(colorData);

            if (type == 0)
            CreateAnimation("Normal", 1, 0, 0, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            if (type == 1)
            CreateAnimation("Normal", 1, 0, 1, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            if (type == 2)
            CreateAnimation("Normal", 1, 0, 2, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            if (type == 3)
            CreateAnimation("Normal", 1, 0, 3, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            if (type == 4)
            CreateAnimation("Normal", 1, 0, 4, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            if (type == 5)
            CreateAnimation("Normal", 1, 0, 5, 18, 36, Vector2.Zero, 10, colorData, STexture.Width);
            
            PlayAnimation("Normal");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void OnCollisionEnter(GameObject other)
        {
        }

        public override void OnCollisionExit(GameObject other)
        {

        }

        public override void OnAnimationDone(string name)
        {
        }
    }
}
