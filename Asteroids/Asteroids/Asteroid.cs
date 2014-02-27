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
        private int size;
        private int type;
        private float volume = 0.7f;
        private Random rnd = new Random();
        private SoundEffect destroyEffect;

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
            // Loading sound effect
            destroyEffect = content.Load<SoundEffect>("explosion-2");

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
        /// <summary>
        /// Splits the asteroid in two and spawns a powerup
        /// </summary>
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
            GameManager.Instance.Score += 50;

            //Sound effect
            SoundEffectInstance destroyInstance = destroyEffect.CreateInstance();
            destroyInstance.IsLooped = false;
            destroyInstance.Volume = volume;
            destroyInstance.Play();
        }
    }
}
