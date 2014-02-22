using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    interface IBulletBuilder
    {
        //Property for getting bullet
        Projectile GetBullet
        {
            get;
        }

        //Methods for building bullet
        void BuildSpeed();
        void BuildSize();
        void BuildRotation();
        void BuildLocation();
        void BuildTexture();
        
    }
}
