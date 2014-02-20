using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    enum State { Turning, Idle }
    class Player : GameObject
    {
        // Fields

        private bool attacking = false;

        // Consctrutor

        public Player(Vector2 sPosition)
            : base(sPosition)
        {
            speed = 100;
            
        }

        // Methods

        public override void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("spaceship.png");
            Color[] colorData = new Color[sTexture.Width * sTexture.Height];
            sTexture.GetData<Color>(colorData);

            CreateAnimation("Normal", 1, 0, 2, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("TurnLeft", 2, 0, 0, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("TurnLeftFull", 1, 0, 1, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("TurnRight", 2, 0, 3, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            CreateAnimation("TurnRightFull", 1, 0, 4, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            PlayAnimation("Normal");

            base.LoadContent(content);

        }

        public override void Update(GameTime gameTime)
        {
            //Resets velocity
            sVelocity = Vector2.Zero;

            //Handles user input
            HandleInput(Keyboard.GetState());

            //Seconds passed since last iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Applies our speed to our velocity
            sVelocity *= speed;

            //Multiplies our movement framerate independent by multiplying with deltaTime
            SPosition += (sVelocity * deltaTime);

            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (SRotation > 6.28325f || SRotation < -6.28325f)
                SRotation = 0;

            if (keyState.IsKeyDown(Keys.W))
            {
                sVelocity = new Vector2((float)Math.Cos(SRotation - 1.57079f), (float)Math.Sin(SRotation - 1.57079f));
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                if (!currentAnimation.Contains("TurnLeftFull"))
                PlayAnimation("TurnLeft");
                state = State.Turning;
                SRotation = SRotation - (float)(3 * Math.PI / 180);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                if(!currentAnimation.Contains("TurnRightFull"))
                PlayAnimation("TurnRight");
                state = State.Turning;
                SRotation = SRotation + (float)(3 * Math.PI / 180);
            }
            if (state == State.Idle)
            {
                PlayAnimation("Normal");
            }
            
            if (keyState.IsKeyDown(Keys.Space))
            {
                attacking = true;
            }

            if (attacking)
            {
                //Shoot projectile
                attacking = false;
            }

            state = State.Idle;
        }

        public override void OnAnimationDone(string name)
        {
            if(name.Contains("TurnLeft"))
                PlayAnimation("TurnLeftFull");
            if (name.Contains("TurnRight"))
                PlayAnimation("TurnRightFull");
        }

        public override void OnCollisionEnter(GameObject other)
        {

        }

        public override void OnCollisionExit(GameObject other)
        {

        }


    }
}
