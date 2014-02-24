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
        private Texture2D GUITexture;

        private Rectangle GUIRect;

        private string assetName;

        public delegate void ElementClicked(string element);

        public event ElementClicked clickEvent;

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

        public GUIElement(string assetName)
        {
            this.assetName = assetName;
        }

        public void LoadContent(ContentManager content)
        {
            GUITexture = content.Load<Texture2D>(assetName);
            GUIRect = new Rectangle(0, 0, GUITexture.Width, GUITexture.Height);
        }
        public void Update()
        {
            if (GUIRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed) 
            {
                clickEvent(assetName);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
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

    }
}
