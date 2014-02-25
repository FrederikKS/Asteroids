using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Asteroids
{
    enum State { Turning, Idle }
    class Player : GameObject
    {
        // Fields

        private int bulletSpeedBonus = 0;
        private int shieldBonus = 0;
        private int attackSpeedBonus = 0;
        private int mines = 0;

        private int attackSpeed = 600;
        private float bulletSpeed = 300;
        private int bulletSize = 1;
        private BulletEngineer bulletEngineer;
        private ContentManager bulletContent;
        private Stopwatch stopwatch;
        private int turningSpeed = 4;
        private bool attacking = false;
        private Projectile bullet;
        private SoundEffect laserEffect;
        private SoundEffect damageEffect;
        private float volume = 1.0f;
        

        //Properties
        public int AttackSpeedBonus
        {
            get { return attackSpeedBonus; }
            set { attackSpeedBonus = value; }
        }
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
        public int BulletSpeedBonus
        {
            get { return bulletSpeedBonus; }
            set { bulletSpeedBonus = value; }
        }
        public int ShieldBonus
        {
            get { return shieldBonus; }
            set { shieldBonus = value; }
        }
        public int Mines
        {
            get { return mines; }
            set { mines = value; }
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
            // Loads texture
            STexture = content.Load<Texture2D>("spaceship.png");
            Color[] colorData = new Color[STexture.Width * STexture.Height];
            STexture.GetData<Color>(colorData);

            // Loads sound effects
            laserEffect = content.Load<SoundEffect>("laser3");
            damageEffect = content.Load<SoundEffect>("explosion-1");


            // Creates animation for spaceship
            CreateAnimation("Normal", 1, 0, 2, 48, 60, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnLeft", 2, 0, 0, 48, 60, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnLeftFull", 1, 0, 1, 48, 60, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnRight", 2, 0, 3, 48, 60, Vector2.Zero, 10, colorData, STexture.Width);
            CreateAnimation("TurnRightFull", 1, 0, 4, 48, 60, Vector2.Zero, 10, colorData, STexture.Width);
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
                if (stopwatch.ElapsedMilliseconds > attackSpeed - (attackSpeedBonus*100))
                {
                    attacking = true;

                    //Sound effect
                    SoundEffectInstance laserInstance = laserEffect.CreateInstance();
                    laserInstance.IsLooped = false;
                    laserInstance.Volume = volume;
                    laserInstance.Play();

                    stopwatch.Restart();                   
                }
            }

            if (attacking)
            {
                //Shoot projectile
                bulletSpeed = 300 + bulletSpeedBonus * 100;
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

            if (other is Powerup)
            {
                AddBonus(((Powerup)other).Type);
                GameManager.Instance.RemoveWhenPossible.Add(other);
            }
            if (other is Asteroid)
            {
                GameManager.Instance.Lives--;

                attackSpeedBonus = 0;
                bulletSize = 1;
                shieldBonus = 0;
                bulletSpeedBonus = 0;
                mines = 0;

                SPosition = new Vector2(1200 / 2, 720 / 2);
                

                //Sound effect
                SoundEffectInstance damageInstance = damageEffect.CreateInstance();
                damageInstance.IsLooped = false;
                damageInstance.Volume = volume;
                damageInstance.Play();
            }
        }

        public override void OnCollisionExit(GameObject other)
        {
            this.sColor = Color.White;
        }

        public void AddBonus(int type)
        {
            if (type == 0 && shieldBonus < 1)
                shieldBonus++;
            else if (type == 1 && mines < 3)
                mines++;
            else if (type == 2 && attackSpeedBonus < 4)
                attackSpeedBonus++;
            else if (type == 3 && bulletSpeedBonus < 3)
                bulletSpeedBonus++;
            else if (type == 4)
                GameManager.Instance.Score += 50;
            else if (type == 5 && bulletSize < 5)
                bulletSize++;
        }


    }
}
