using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class AsteroidLarge : IAsteroidBuilder
    {
        private Asteroid asteroid;
        private Random rnd = new Random();
        private Vector2 startPos;
        private int type;

        //Properties
        public Asteroid GetAsteroid
        {
            get { return asteroid; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="type"></param>
        public AsteroidLarge(Vector2 startPosition, int type)
        {
            asteroid = new Asteroid(startPosition, type);
            this.startPos = startPosition;
            this.type = type;
            
        }
        public void BuildSpeed()
        {
            this.asteroid.Speed = rnd.Next(100, 150);
        }

        public void BuildSize()
        {
            this.asteroid.Size = 3;
        }

        public void BuildRotation()
        {
            this.asteroid.SRotation = (float)NextDouble(rnd, 0, 6.28325);
        }

        public void BuildLocation()
        {
            this.asteroid.SPosition = startPos;
        }

        /// <summary>
        /// Defines which texture this asteroid should have
        /// </summary>
        public void BuildTexture()
        {
            if (type == 0)
                type = rnd.Next(1, 7);
            if (type == 1)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large1.png");
            if (type == 2)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large2.png");
            if (type == 3)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large3.png");
            if (type == 4)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large4.png");
            if (type == 5)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large5.png");
            if (type == 6)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Large6.png");


            Color[] colorData = new Color[asteroid.STexture.Width * asteroid.STexture.Height];
            asteroid.STexture.GetData<Color>(colorData);

            asteroid.CreateAnimation("Normal", 1, 0, 0, 120, 120, Vector2.Zero, 10, colorData, asteroid.STexture.Width);
            asteroid.PlayAnimation("Normal");
            asteroid.Type = type;
            
            asteroid.LoadContent(GameManager.Instance.Content);
        }

        /// <summary>
        /// Randomizer
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public double NextDouble(Random rnd, double min, double max)
        {
            return min + (rnd.NextDouble() * (max - min));
        }
    }
}
