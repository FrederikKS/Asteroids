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
        private int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
      
        // Constructor

        public Asteroid(Vector2 startPos, Vector2 endPos, int size, int speed) : base(startPos)
        {
            this.size = size;
            this.speed = speed;
        }

        // Methods

        public override void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("a10000.png");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            //Resets velocity
            sVelocity = Vector2.Zero;

            //Seconds passed since last iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Applies our speed to our velocity
            sVelocity *= speed;

            //Multiplies asteroids framerate independent by multiplying with deltaTime
            SPosition += (sVelocity * deltaTime);

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
    }
}
