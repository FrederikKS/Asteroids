using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    interface IAsteroidBuilder
    {
        //Property for getting bullet
        Asteroid GetAsteroid
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
