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
    enum Direction { Left, Right, Up, Down, None }
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
            CreateAnimation("TurnRight", 2, 0, 3, 96, 120, Vector2.Zero, 10, colorData, sTexture.Width);
            PlayAnimation("Normal");

            loopAnimation = true;
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
            loopAnimation = true;
            if (keyState.IsKeyDown(Keys.A))
            {
                PlayAnimation("TurnLeft");
                loopAnimation = false;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                PlayAnimation("TurnRight");
                loopAnimation = false;
            }
            else
                PlayAnimation("Normal");
            
            if (keyState.IsKeyDown(Keys.Space))
            {
                attacking = true;
            }

            if (attacking)
            {
                
            }
        }

        public override void OnAnimationDone(string name)
        {
        }

        public override void OnCollisionEnter(GameObject other)
        {

        }

        public override void OnCollisionExit(GameObject other)
        {

        }


    }
}
