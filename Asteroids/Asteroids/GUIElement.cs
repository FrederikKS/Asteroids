using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class GUIElement
    {
        //Fields
        private Texture2D GUITexture;
        private Rectangle GUIRect;
        private string assetName;
        private float rotation = 0;
        private SpriteEffects sEffect = new SpriteEffects();
        public delegate void ElementClicked(string element);
        public event ElementClicked clickEvent;

        /// <summary>
        /// Properties
        /// </summary>
        public Texture2D GetGUITexture
        {
            get { return GUITexture; }
            set { GUITexture = value; }
        }
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        //Constructor
        public GUIElement(string assetName)
        {
            this.assetName = assetName;
        }
        //Load content for this GUI element
        public void LoadContent(ContentManager content)
        {
            GUITexture = content.Load<Texture2D>(assetName);
            GUIRect = new Rectangle(0, 0, GUITexture.Width, GUITexture.Height);
        }

        //Perform this every frame
        public void Update()
        {
            if (GUIRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed) 
            {
                clickEvent(assetName);

            }
        }

        /// <summary>
        /// Draw function, used for drawing GUI elements
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(sTexture, sPosition + sOffset, sRectangles[currentIndex], sColor, sRotation, sOrigin, scale, sEffect, sLayer);
            if (this.assetName == "GUI/background.png")
                spriteBatch.Draw(GUITexture, new Vector2(GUIRect.Location.X, GUIRect.Location.Y) + new Vector2(GUIRect.Width/2, GUIRect.Height/2), new Rectangle(0, 0, GUITexture.Width, GUITexture.Height), Color.White, rotation, new Vector2(GUITexture.Width/2, GUITexture.Height/2), 1, sEffect, 1);
            else
                spriteBatch.Draw(GUITexture, GUIRect, Color.White);
                
        }

        public void CenterElement(int width, int height)
        {
            GUIRect = new Rectangle((width / 2) - GUITexture.Width / 2, (height / 2) - GUITexture.Height / 2, GUITexture.Width, GUITexture.Height);
        }

        public void MoveElement(int x, int y)
        {
            GUIRect = new Rectangle(GUIRect.X += x, GUIRect.Y += y, GUIRect.Width, GUIRect.Height);
        }

        public void RotateElement(float rotation)
        {
            if (this.rotation + rotation > MathHelper.ToRadians(360))
                this.rotation = 0;
            else
            this.rotation = this.rotation + rotation;
        }

    }
}
