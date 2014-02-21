using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Projectile : GameObject
    {
        // Fields
        private int size;

        // Constructor
        public Projectile(Vector2 startPos, float rotation, int speed, int size) : base(startPos)
        {
            this.speed = speed;
            this.size = size;
            SRotation = rotation;
        }

        // Methods
        public override void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("BulletBlue.png");
            Color[] colorData = new Color[sTexture.Width * sTexture.Height];
            sTexture.GetData<Color>(colorData);

            CreateAnimation("Bullet1", 1, 0, 0, 20, 25, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("Bullet2", 1, 0, 16, 28, 38, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("Bullet3", 1, 0, 40, 36, 47, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("Bullet4", 1, 0, 72, 44, 47, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("Bullet5", 1, 0, 112, 52, 80, Vector2.Zero, 10, colorData, sTexture.Width);
            PlayAnimation("Bullet1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            //Move in vector
            sVelocity = new Vector2((float)Math.Cos(SRotation - 1.57079f), (float)Math.Sin(SRotation - 1.57079f));

            ////Makes sure we will get a unitvector, so that we get a normal movement
            sVelocity.Normalize();

            //Applies our speed to our velocity
            sVelocity *= speed;

            //Seconds passed since last iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Makes the movement framerate independent by multiplying with deltaTime
            SPosition += (sVelocity * deltaTime);


            base.Update(gameTime);
        }

        public override void OnCollisionEnter(GameObject other)
        {
            //Check if collides with object and which type the object is
        }

        public override void OnCollisionExit(GameObject other)
        {
            //Checks if exits object collision. Not sure this is usable
        }

        public override void OnAnimationDone(String name)
        {
            //Probably won't be used
        }

    }
}
