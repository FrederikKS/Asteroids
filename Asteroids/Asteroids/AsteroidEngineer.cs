using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class AsteroidEngineer
    {
        //Engineer in charge of building specific asteroids
        private readonly IAsteroidBuilder asteroidBuilder;

        public Asteroid GetAsteroid
        {
            get { return asteroidBuilder.GetAsteroid; }
        }

        public AsteroidEngineer(IAsteroidBuilder asteroidBuilder)
        {
            this.asteroidBuilder = asteroidBuilder;
        }

        public void BuildAsteroid()
        {
            asteroidBuilder.BuildLocation();
            asteroidBuilder.BuildRotation();
            asteroidBuilder.BuildSize();
            asteroidBuilder.BuildSpeed();
            asteroidBuilder.BuildTexture();
        }
    }
}
