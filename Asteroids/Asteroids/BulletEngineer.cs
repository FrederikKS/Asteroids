using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class BulletEngineer
    {
        //Engineer in charge of building specific bullets
        private readonly IBulletBuilder bulletBuilder;

        public Projectile GetBullet
        {
            get { return bulletBuilder.GetBullet; }
        }

        public BulletEngineer(IBulletBuilder bulletBuilder)
        {
            this.bulletBuilder = bulletBuilder;
        }

        public void BuildBullet()
        {
            bulletBuilder.BuildLocation();
            bulletBuilder.BuildRotation();
            bulletBuilder.BuildSize();
            bulletBuilder.BuildSpeed();
            bulletBuilder.BuildTexture();
        }
    }
}
