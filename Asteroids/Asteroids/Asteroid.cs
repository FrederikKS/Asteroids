﻿using System;
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
        private int size;
        private int type;
        private Random rnd = new Random();

        //Properties
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        // Constructor

        public Asteroid(Vector2 startPos, int type) : base(startPos)
        {
        }

        // Methods

        public override void LoadContent(ContentManager content)
        {
            //STexture = content.Load<Texture2D>("a10000.png");
            //Color[] colorData = new Color[STexture.Width * STexture.Height];
            //STexture.GetData<Color>(colorData);

            //CreateAnimation("Normal", 1, 0, 0, 120, 120, Vector2.Zero, 10, colorData, STexture.Width);
            //PlayAnimation("Normal");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            sVelocity = new Vector2((float)Math.Cos(SRotation), (float)Math.Sin(SRotation));

            //Seconds passed since last iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Applies our speed to our velocity
            sVelocity *= Speed;

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

            int randomChance = rnd.Next(0, 1);

            if (randomChance == 0)
            {
                Powerup temp = new Powerup(SPosition, rnd.Next(0, 6));
                temp.LoadContent(GameManager.Instance.Content);
                GameManager.Instance.TempList.Add(temp);
            }

            if (size == 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    AsteroidEngineer engineer = new AsteroidEngineer(new AsteroidMedium(SPosition, type));
                    engineer.BuildAsteroid();
                    GameManager.Instance.TempList.Add(engineer.GetAsteroid);
                } 
            }
            if (size == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    AsteroidEngineer engineer = new AsteroidEngineer(new AsteroidSmall(SPosition, type));
                    engineer.BuildAsteroid();
                    GameManager.Instance.TempList.Add(engineer.GetAsteroid);
                }
            }
            
            
            GameManager.Instance.RemoveWhenPossible.Add(this);
        }
    }
}
