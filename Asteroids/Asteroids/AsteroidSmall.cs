using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class AsteroidSmall : IAsteroidBuilder
    {
        private Asteroid asteroid;
        private Random rnd = new Random();
        private Vector2 startPos;
        private int type;

        public Asteroid GetAsteroid
        {
            get { return asteroid; }
        }

        public AsteroidSmall(Vector2 startPosition, int type)
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
            this.asteroid.Size = 1;
        }

        public void BuildRotation()
        {
            this.asteroid.SRotation = (float)NextDouble(rnd, 0, 6.28325);
        }

        public void BuildLocation()
        {
            this.asteroid.SPosition = startPos;
        }

        public void BuildTexture()
        {
            if (type == 0)
                type = rnd.Next(1, 7);
            if (type == 1)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small1.png");
            if (type == 2)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small2.png");
            if (type == 3)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small3.png");
            if (type == 4)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small4.png");
            if (type == 5)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small5.png");
            if (type == 6)
                asteroid.STexture = GameManager.Instance.Content.Load<Texture2D>("Asteroids/Small6.png");


            Color[] colorData = new Color[asteroid.STexture.Width * asteroid.STexture.Height];
            asteroid.STexture.GetData<Color>(colorData);

            asteroid.CreateAnimation("Normal", 1, 0, 0, 32, 32, Vector2.Zero, 10, colorData, asteroid.STexture.Width);
            asteroid.PlayAnimation("Normal");
            asteroid.Type = type;

            asteroid.LoadContent(GameManager.Instance.Content);
        }


        public double NextDouble(Random rnd, double min, double max)
        {
            return min + (rnd.NextDouble() * (max - min));
        }
    }
}
