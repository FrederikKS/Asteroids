using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace Asteroids
{
    class Bullet : IBulletBuilder
    {
        private Projectile bullet;
        private GameObject origin;

        public Projectile GetBullet
        {
            get { return bullet; }
        }

        public Bullet(GameObject origin)
        {
            bullet = new Projectile(origin);
            this.origin = origin;
        }
        public void BuildSpeed()
        {
            if(origin is Player)
            this.bullet.Speed = ((Player)origin).BulletSpeed;
        }

        public void BuildSize()
        {
            if (origin is Player)
            this.bullet.Size = ((Player)origin).BulletSize;
        }

        public void BuildRotation()
        {
            this.bullet.SRotation = origin.SRotation;
        }

        public void BuildLocation()
        {
            this.bullet.SPosition = origin.SPosition;
        }

        public void BuildTexture()
        {
            if(origin is Player)
            {
                this.bullet.LoadContent(((Player)origin).BulletContent);
            }
        }
    }
}
