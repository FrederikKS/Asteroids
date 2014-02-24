#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Rectangle bgRec;
        Player player;
        //Enemy enemy;
        Asteroid asteroid;
        private int worldSizeX;
        private int worldSizeY;
        private List<Asteroid> astWave = new List<Asteroid>();
        private int asteroidCount = 4;
        Random rnd = new Random();
        SpriteFont sf;
        private bool levelCleared = false;
        private int currentAsteroids;
        //private int astSpawn;
        
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            worldSizeY = graphics.PreferredBackBufferHeight;
            graphics.PreferredBackBufferWidth = 1200;
            worldSizeX = graphics.PreferredBackBufferWidth;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Centers the window to the center of the resolution

            Window.SetPosition(new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 6, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 6));

            player = new Player(new Vector2(100, 100));
            //enemy = new Enemy(new Vector2(100, 100));
            GameManager.Instance.AllObjects.Add(player);
            //GameManager.Instance.AllObjects.Add(enemy);
            GameManager.Instance.Content = Content;

            for (int i = 0; i < asteroidCount; i++)
            {
                AsteroidEngineer engineer = new AsteroidEngineer(new AsteroidLarge(new Vector2(rnd.Next(0, worldSizeX), rnd.Next(0, worldSizeY)), 0));
                engineer.BuildAsteroid();
                asteroid = engineer.GetAsteroid;
                astWave.Add(asteroid);
            }
            foreach (Asteroid ast in astWave)
            {
                GameManager.Instance.AllObjects.Add(ast);
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            // Adds asteroids to a list

            sf = Content.Load<SpriteFont>("SpriteFont1");
            background = Content.Load<Texture2D>(@"BackgroundRed");
            bgRec = new Rectangle(0, 0, background.Width, background.Height);
     
            foreach (GameObject obj in GameManager.Instance.AllObjects)
            {
                obj.LoadContent(Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            // Adding temp objects to object list
            foreach (GameObject temp in GameManager.Instance.TempList)
            {
                if (!GameManager.Instance.AllObjects.Contains(temp))
                    GameManager.Instance.AllObjects.Add(temp);
            }
            
            //Clearing list of temp objects
            GameManager.Instance.TempList.Clear();

            //Clearing list of objects ready to be removed
            foreach (GameObject temp in GameManager.Instance.RemoveWhenPossible)
            {
                if (GameManager.Instance.AllObjects.Contains(temp))
                    GameManager.Instance.AllObjects.Remove(temp);
            }
            GameManager.Instance.RemoveWhenPossible.Clear();


            //Update all objects currently in the game
            foreach (GameObject obj in GameManager.Instance.AllObjects)
            {
                if (obj is Asteroid)
                {
                    levelCleared = false; ;
                }
                else
                {
                    levelCleared = true;
                }
                obj.Update(gameTime);
            }

            if (levelCleared == true)
            {
                asteroidCount++;

                for (int i = 0; i < asteroidCount; i++)
                {
                    AsteroidEngineer engineer = new AsteroidEngineer(new AsteroidLarge(new Vector2(rnd.Next(0, worldSizeX), rnd.Next(0, worldSizeY)), 0));
                    engineer.BuildAsteroid();
                    asteroid = engineer.GetAsteroid;
                    astWave.Add(asteroid);
                }
                foreach (Asteroid ast in astWave)
                {
                    GameManager.Instance.AllObjects.Add(ast);
                }
            }

            levelCleared = false;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(background, bgRec, Color.White);
            foreach (GameObject obj in GameManager.Instance.AllObjects)
            {
                obj.Draw(spriteBatch);
            }

            spriteBatch.DrawString(sf, "Score:", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(sf, GameManager.Instance.Score.ToString(), new Vector2(100, 10), Color.White);
            spriteBatch.DrawString(sf, "Lifes:", new Vector2(worldSizeX - 150, 10), Color.White);
            spriteBatch.DrawString(sf, GameManager.Instance.Lifes.ToString(), new Vector2(worldSizeX - 70, 10), Color.White);

            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        double NextDouble(Random rnd, double min, double max)
        {
            return min + (rnd.NextDouble() * (max - min));
        }
    }
}
