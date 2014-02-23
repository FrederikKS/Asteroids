using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Asteroids
{
    enum State { Turning, Idle }
    class Player : GameObject
    {
        // Fields

        private int bulletSpeedBonus = 300;
        private int shieldBonus = 0;
        private int attackSpeedBonus = 0;
        private int mines = 0;

        private float bulletSpeed = 300;
        private int bulletSize = 1;
        private BulletEngineer bulletEngineer;
        private ContentManager bulletContent;
        private Stopwatch stopwatch;
        private int turningSpeed = 4;
        private bool attacking = false;
        private Projectile bullet;
        

        //Properties
        public int BulletSize
        {
            get { return bulletSize; }
            set { bulletSize = value; }
        }
        public float BulletSpeed
        {
            get { return bulletSpeed; }
            set { bulletSpeed = value; }
        }
        public ContentManager BulletContent
        {
            get { return bulletContent; }
            set { bulletContent = value; }
        }

        // Consctrutor

        public Player(Vector2 sPosition)
            : base(sPosition)
        {
            Speed = 200;
            
        }

        // Methods

        public override void LoadContent(ContentManager content)
        {
            STexture = content.Load<Texture2D>("spaceship.png");
            Color[] colorData = new Color[STexture.Width * STexture.Height];
            STexture.GetData<Color>(colorData);

            CreateAnimation("Normal", 1, 0, 2, 96, 120, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnLeft", 2, 0, 0, 96, 120, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnLeftFull", 1, 0, 1, 96, 120, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnRight", 2, 0, 3, 96, 120, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnRightFull", 1, 0, 4, 96, 120, Vector2.Zero, 10, colorData, STexture.Width);
            PlayAnimation("Normal");

            bulletContent = content;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            
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
            sVelocity *= Speed;

            //Multiplies our movement framerate independent by multiplying with deltaTime
            SPosition += (sVelocity * deltaTime);

            if (SPosition.X >= 1200)
            {
                SPosition = new Vector2(1, SPosition.Y);
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
                SRotation = SRotation - (float)(turningSpeed * Math.PI / 180);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                if(!currentAnimation.Contains("TurnRightFull"))
                PlayAnimation("TurnRight");
                state = State.Turning;
                SRotation = SRotation + (float)(turningSpeed * Math.PI / 180);
            }
            if (state == State.Idle)
            {
                PlayAnimation("Normal");
            }
            
            if (keyState.IsKeyDown(Keys.Space))
            {
                if (stopwatch.ElapsedMilliseconds > 600 - (attackSpeedBonus*100))
                {
                    attacking = true;
                    stopwatch.Restart();
                }
            }

            if (attacking)
            {
                //Shoot projectile
                bulletEngineer = new BulletEngineer(new Bullet(this));
                bulletEngineer.BuildBullet();
                bullet = bulletEngineer.GetBullet;
                GameManager.Instance.TempList.Add(bullet);
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
            this.sColor = Color.Red;

        }

        public override void OnCollisionExit(GameObject other)
        {
            this.sColor = Color.White;
        }

        public void AddBonus(int type)
        {
            if (type == 0 && shieldBonus < 1)
                shieldBonus++;
            if (type == 1 && mines < 3)
                mines++;
            if (type == 2 && attackSpeedBonus < 4)
                attackSpeedBonus++;
            if (type == 3 && bulletSpeedBonus < 3)
                bulletSpeedBonus++;
            if (type == 5 && bulletSize < 5)
                bulletSize++;
        }


    }
}
