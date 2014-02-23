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
        private Texture2D sTexture;
        //Rectangle array for drawing sprite
        private Rectangle[] sRectangles;
        //Dictionary of animations
        protected Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        //Name of current animation
        protected string currentAnimation;
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
        private float speed;
        //Current direction of object
        protected State state = State.Idle;
        //Delegate ended Animation event
        public delegate void AnimationEnded(string name);
        //Texture used for drawing collisionBox
        private Texture2D boxTexture;
        //Float for keeping rotation value of GameObject
        private float sRotation;
        //List of objects currently colliding with this object
        private List<GameObject> collidingObjects;
        //Matrix translation of object's rotation
        Matrix objectTransform;

        //Properties
        public Texture2D STexture
        {
            get { return sTexture; }
            set { sTexture = value; }
        }
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
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
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
                // Defining the object's transform - origin, rotation and location
                objectTransform = Matrix.CreateTranslation(new Vector3(-sOrigin, 0.0f)) *
                    Matrix.CreateRotationZ(sRotation) *
                    Matrix.CreateTranslation(new Vector3(sPosition, 0.0f));

                // Calculate the edges of the new rectangle
                return CalculateBoundingRectangle(new Rectangle(0, 0, sRectangles[currentIndex].Width, sRectangles[currentIndex].Height),objectTransform);

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
             
             //Calculates the current index
             currentIndex = (int)(timeElapsed * fps);

             //Checks if we need to restart the animation
             if (currentIndex > sRectangles.Length - 1)
             {
                 OnAnimationDone(currentAnimation);
                 currentIndex = 0;
                 timeElapsed = 0;
             }

             // Calculate this object's transformation (rotation)
             SOrigin = new Vector2(sRectangles[currentIndex].Width / 2, sRectangles[currentIndex].Height / 2);

             //objectTransform = Matrix.CreateTranslation(new Vector3(-sOrigin, 0.0f)) * Matrix.CreateRotationZ(sRotation) * Matrix.CreateTranslation(new Vector3(sPosition, 0.0f));

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
         public void CreateAnimation(string name, int frames, int yPos, int xStartFrame, int width, int height, Vector2 offset, float fps, Color[] colors, int textureWidth)
         {
             animations.Add(name, new Animation(frames, yPos, xStartFrame, width, height, offset, fps, colors, textureWidth));
         }

         /// <summary>
         /// Plays an animation
         /// </summary>
         /// <param name="name">animation name</param>
         public void PlayAnimation(string name)
         {
             if (currentAnimation != name)
             {
                 currentAnimation = name;
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
                     if (IntersectPixels(obj)) //Checks if we have a pixel collision
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

#region not working pixel collision
         /// <summary>
         /// Determines if there is overlap of the non-transparent pixels between two
         /// sprites.
         /// </summary>
         /// <param name="transformA">World transform of the first sprite.</param>
         /// <param name="widthA">Width of the first sprite's texture.</param>
         /// <param name="heightA">Height of the first sprite's texture.</param>
         /// <param name="dataA">Pixel color data of the first sprite.</param>
         /// <param name="transformB">World transform of the second sprite.</param>
         /// <param name="widthB">Width of the second sprite's texture.</param>
         /// <param name="heightB">Height of the second sprite's texture.</param>
         /// <param name="dataB">Pixel color data of the second sprite.</param>
         /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
         
         //private bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA,
         //                             Matrix transformB, int widthB, int heightB, Color[] dataB)
         private bool IntersectPixels(GameObject other)
         {
             // Calculate a matrix which transforms from A's local space into
             // world space and then into B's local space
             Matrix transformAToB = objectTransform * Matrix.Invert(other.objectTransform);

             // When a point moves in A's local space, it moves in B's local space with a
             // fixed direction and distance proportional to the movement in A.
             // This algorithm steps through A one pixel at a time along A's X and Y axes
             // Calculate the analogous steps in B:
             Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
             Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

             // Calculate the top left corner of A in B's local space
             // This variable will be reused to keep track of the start of each row
             Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

             // For each row of pixels in A
             for (int yA = 0; yA < sRectangles[currentIndex].Height; yA++)
             {
                 // Start at the beginning of the row
                 Vector2 posInB = yPosInB;

                 // For each pixel in this row
                 for (int xA = 0; xA < sRectangles[currentIndex].Width; xA++)
                 {
                     // Round to the nearest pixel
                     int xB = (int)Math.Round(posInB.X);
                     int yB = (int)Math.Round(posInB.Y);

                     // If the pixel lies within the bounds of B
                     if (0 <= xB && xB < other.sRectangles[other.currentIndex].Width &&
                         0 <= yB && yB < other.sRectangles[other.currentIndex].Height)
                     {
                        // Get the colors of the overlapping pixels
                        Color colorA = animations[currentAnimation].colors[currentIndex][xA + yA * sRectangles[currentIndex].Width];
                        Color colorB = other.animations[other.currentAnimation].colors[other.currentIndex][xB + yB * other.sRectangles[other.currentIndex].Width];


                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                     }

                     // Move to the next pixel in the row
                     posInB += stepX;
                 }

                 // Move to the next row
                 yPosInB += stepY;
             }

             // No intersection found
             return false;
         }
#endregion
       

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

         /// <summary>
         /// Calculates an axis aligned rectangle which fully contains an arbitrarily
         /// transformed axis aligned rectangle.
         /// </summary>
         /// <param name="rectangle">Original bounding rectangle.</param>
         /// <param name="transform">World transform of the rectangle.</param>
         /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
         public Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                            Matrix transform)
         {
             // Get all four corners in local space
             Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
             Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
             Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
             Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

             // Transform all four corners into work space
             Vector2.Transform(ref leftTop, ref transform, out leftTop);
             Vector2.Transform(ref rightTop, ref transform, out rightTop);
             Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
             Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

             // Find the minimum and maximum extents of the rectangle in world space
             Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                       Vector2.Min(leftBottom, rightBottom));
             Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                       Vector2.Max(leftBottom, rightBottom));

             // Return that as a rectangle
             return new Rectangle((int)min.X, (int)min.Y,
                                  (int)(max.X - min.X), (int)(max.Y - min.Y));
         }
    }
}
