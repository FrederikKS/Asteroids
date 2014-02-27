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

        //Properties
        public Asteroid GetAsteroid
        {
            get { return asteroidBuilder.GetAsteroid; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="asteroidBuilder"></param>
        public AsteroidEngineer(IAsteroidBuilder asteroidBuilder)
        {
            this.asteroidBuilder = asteroidBuilder;
        }

        /// <summary>
        /// Building sequence
        /// </summary>
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
