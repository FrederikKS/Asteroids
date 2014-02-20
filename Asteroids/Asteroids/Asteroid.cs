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

        public int size;
        
        // Constructor

        public Asteroid(Vector2 startPos, Vector2 endPos, int size) : base(startPos)
        {
            this.size = size;
        }

        // Methods

        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("a10000.png");
        }

        public void Update(GameTime gameTime)
        {
           
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
        public void Split()
        {

        }
    }
}
