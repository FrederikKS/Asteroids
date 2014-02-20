using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    abstract class GameObject
    {
        //Fields

        //Texture
        protected Texture2D sTexture;
        //Rectangle array for drawing sprite
        private Rectangle[] sRectangles;
        //Dictionary of animations
        protected Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        //Name of current animation
        protected string aniName;
        //Current frame of animation
        private int currentIndex;
        //Time passed since last frame change
        private float timeElapsed;
        //The animation's fps
        private float fps;
        //Position of object
        private Vector2 sPosition = Vector2.Zero;
        //Offset of current animation
        private Vector2 sOffset = Vector2.Zero;
        //Origin point of the sprite, defining which point the sprite should be drawn from
        private Vector2 sOrigin = Vector2.Zero;
        //The layer of the sprite
        private float sLayer = 0;
        //Scale of the sprite
        private float scale = 1f;
        //Color of sprite
        protected Color sColor = Color.White;
        //Adds an effect to the sprite
        private SpriteEffects sEffect = new SpriteEffects();
        //Velocity of the object
        protected Vector2 sVelocity;
        //Speed of gameobject
        protected float speed;
        //Current direction of object
        protected Direction myDir = Direction.None;
        //Delegate ended Animation event
        public delegate void AnimationEnded(string name);
        //Texture used for drawing collisionBox
        private Texture2D boxTexture;
        //Float for keeping rotation value of GameObject
        private float sRotation;
        //List of objects currently colliding with this object
        private List<GameObject> collidingObjects;

        //Properties
        public Vector2 SPosition
        {
            get { return sPosition; }
            set { sPosition = value; }
        }
        public Vector2 SOrigin
        {
            get { return sOrigin; }
            set { sOrigin = value; }
        }
        public float SRotation
        {
            get { return sRotation; }
            set { sRotation = value; }
        }
        //Position of collision rectangle
        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle
                    (
                        (int)((sPosition.X + sOffset.X) - sRectangles[currentIndex].Width / 2),
                        (int)((sPosition.Y + sOffset.Y) - sRectangles[currentIndex].Height / 2),
                        sRectangles[currentIndex].Width, sRectangles[currentIndex].Height
                    );
            }
        }
        /// <summary>
        /// Constructor of GameObject
        /// </summary>
        /// <param name="sPosition"></param>
         public GameObject(Vector2 sPosition)
        {
            this.collidingObjects = new List<GameObject>();
          
            this.sPosition = sPosition;
        }
        /// <summary>
        /// Abstract function used for resetting/stopping/changing animation
        /// </summary>
        /// <param name="name"></param>
         public abstract void OnAnimationDone(string name);

         /// <summary>
         /// Updates our sprite
         /// </summary>
         /// <param name="gameTime">time passed since last update</param>
         public virtual void Update(GameTime gameTime)
         {

             //Adds time that has passed since last update
             timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

             //Calculates the curent index
             currentIndex = (int)(timeElapsed * fps);

             //Checks if we need to restart the animation
             if (currentIndex > sRectangles.Length - 1)
             {
                 OnAnimationDone(aniName);
                 timeElapsed = 0;
                 currentIndex = 0;
             }

             SOrigin = new Vector2(sRectangles[currentIndex].Width / 2, sRectangles[currentIndex].Height / 2);

             HandleCollision();
         }

         /// <summary>
         /// Draws the sprite on the screen
         /// </summary>
         /// <param name="spriteBatch"></param>
         public void Draw(SpriteBatch spriteBatch)
         {
             spriteBatch.Draw(sTexture, sPosition + sOffset, sRectangles[currentIndex], sColor, sRotation, sOrigin, scale, sEffect, sLayer);

             Rectangle topLine = new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, 1);
             Rectangle bottomLine = new Rectangle(CollisionRect.X, CollisionRect.Y + CollisionRect.Height, CollisionRect.Width, 1);
             Rectangle rightLine = new Rectangle(CollisionRect.X + CollisionRect.Width, CollisionRect.Y, 1, CollisionRect.Height);
             Rectangle leftLine = new Rectangle(CollisionRect.X, CollisionRect.Y, 1, CollisionRect.Height);

             spriteBatch.Draw(boxTexture, topLine, Color.Red);
             spriteBatch.Draw(boxTexture, bottomLine, Color.Red);
             spriteBatch.Draw(boxTexture, rightLine, Color.Red);
             spriteBatch.Draw(boxTexture, leftLine, Color.Red);
         }

         /// <summary>
         /// Creates an animation
         /// </summary>
         /// <param name="name">Animation name</param>
         /// <param name="frames">Number of frames in the animation</param>
         /// <param name="yPos">Y position on the sprite sheet in pixels</param>
         /// <param name="xStartFrame">X position on the sprite sheet in frames</param>
         /// <param name="width">The width of each frame</param>
         /// <param name="height">The height of each frame</param>
         /// <param name="offset">Animation offset(can be used to align animations)</param>
         /// <param name="fps">Animation fps</param>
         protected void CreateAnimation(string name, int frames, int yPos, int xStartFrame, int width, int height, Vector2 offset, float fps, Color[] colors, int textureWidth)
         {
             animations.Add(name, new Animation(frames, yPos, xStartFrame, width, height, offset, fps, colors, textureWidth));
         }

         /// <summary>
         /// Plays an animation
         /// </summary>
         /// <param name="name">animation name</param>
         protected void PlayAnimation(string name)
         {
             if (aniName != name && myDir == Direction.None)
             {
                 aniName = name;
                 currentIndex = 0;
                 timeElapsed = 0;
                 this.fps = animations[name].Fps;
                 this.sOffset = animations[name].Offset;
                 sRectangles = animations[name].Rectangles;
             }
         }
         private void HandleCollision()
         {
             foreach (GameObject obj in GameManager.Instance.AllObjects)
             {   //We have a box collision
                 if (obj != this && obj.CollisionRect.Intersects(this.CollisionRect))
                 {
                     if (PixelCollision(obj)) //Checks if we have a pixel collision
                     {
                         // We have a pixelCollision
                         if (!collidingObjects.Exists(x => x == obj))
                         {
                             collidingObjects.Add(obj);
                             OnCollisionEnter(obj);
                         }
                     }//If we stopped colliding with a object
                     else if (obj != this && collidingObjects.Exists(x => x == obj))
                     {
                         collidingObjects.Remove(obj);
                         OnCollisionExit(obj);
                     }

                 }

             }
         }
         private bool PixelCollision(GameObject other)
         {
             // Find the bounds of the rectangle intersection
             int top = Math.Max(this.CollisionRect.Top, other.CollisionRect.Top);
             int bottom = Math.Min(this.CollisionRect.Bottom, other.CollisionRect.Bottom);
             int left = Math.Max(this.CollisionRect.Left, other.CollisionRect.Left);
             int right = Math.Min(this.CollisionRect.Right, other.CollisionRect.Right);

             for (int y = top; y < bottom; y++)
             {
                 for (int x = left; x < right; x++)
                 {
                     //  Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                     // Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];

                     //Get the color of both pixels at this point 
                     Color colorA = animations[aniName].colors[currentIndex]
                     [(x - CollisionRect.Left) + (y - CollisionRect.Top) * CollisionRect.Width];
                     Color colorB = other.animations[other.aniName].colors[other.currentIndex]
                     [(x - other.CollisionRect.Left) + (y - other.CollisionRect.Top) * other.CollisionRect.Width];

                     // If both pixels are not completely transparent
                     if (colorA.A != 0 && colorB.B != 0)
                     {
                         //Then an intersection has been found
                         return true;
                     }
                 }
             }
             return false;
         }

         /// <summary>
         /// Loads content
         /// </summary>
         /// <param name="content">ContentManager</param>
         public virtual void LoadContent(ContentManager content)
         {
             boxTexture = content.Load<Texture2D>("CollisionTexture.png");

         }

         public abstract void OnCollisionEnter(GameObject other);

         public abstract void OnCollisionExit(GameObject other);
    }
}
