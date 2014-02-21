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
    class Asteroid : GameObject
    {
        // Fields 

        public int size;
        private Vector2 startPos;
        private Random rnd = new Random();
      
        // Constructor

        public Asteroid(Vector2 startPos, int size, float rotation) : base(startPos)
        {
            this.size = size;
            this.startPos = startPos;
            speed = 150;
            this.SRotation = rotation;
        }

        // Methods

        public override void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("a10000.png");
            Color[] colorData = new Color[sTexture.Width * sTexture.Height];
            sTexture.GetData<Color>(colorData);

            CreateAnimation("Normal", 1, 0, 0, 120, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            PlayAnimation("Normal");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            sVelocity = new Vector2((float)Math.Cos(SRotation), (float)Math.Sin(SRotation));

            //Seconds passed since last iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Applies our speed to our velocity
            sVelocity *= speed;

            //Multiplies our movement framerate independent by multiplying with deltaTime
            SPosition += (sVelocity * deltaTime);

            if (SPosition.X >= 1200)
            {
                SPosition = new Vector2(1 , SPosition.Y);
            }
            if (SPosition.Y >= 720)
            {
                SPosition = new Vector2(SPosition.X, 1);
            }
            if (SPosition.X <= 0)
            {
                SPosition = new Vector2(1199, SPosition.Y);
            }
            if (SPosition.Y <= 0)
            {
                SPosition = new Vector2(SPosition.X, 719);
            }

            base.Update(gameTime);
        }

        public override void OnCollisionEnter(GameObject other)
        {
            // enter collision
        }

        public override void OnCollisionExit(GameObject other)
        {
            // Exit collision
        }

        public override void OnAnimationDone(string name)
        {
           
        }
        public void Split()
        {

        }

        double NextDouble(Random rnd, double min, double max)
        {
            return min + (rnd.NextDouble() * (max - min));
        }

    }
}
