using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    class GameManager
    {
        //Fields
        private static GameManager instance;
        private ContentManager content;
        private List<GameObject> allObjects;
        private List<GameObject> tempList;
        private List<GameObject> removeWhenPossible;
        private int score;
        private int lives = 3;

        //Properties
        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// The static instance making this class a singleton
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
            set { instance = value; }
        }
        /// <summary>
        /// List used for removing objects
        /// </summary>
        internal List<GameObject> RemoveWhenPossible
        {
            get { return removeWhenPossible; }
            set { removeWhenPossible = value; }
        }
        /// <summary>
        /// List used for temporary storage before adding to AllObjects
        /// </summary>
        internal List<GameObject> TempList
        {
            get { return tempList; }
            set { tempList = value; }
        }
        /// <summary>
        /// All objects shown in the game
        /// </summary>
        public List<GameObject> AllObjects
        {
            get { return allObjects; }
            set { allObjects = value; }
        }

        /// <summary>
        /// Player's score
        /// </summary>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        /// <summary>
        /// Player's Lives
        /// </summary>
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        //Constructor
        private GameManager()
        {
            removeWhenPossible = new List<GameObject>();
            allObjects = new List<GameObject>();
            tempList = new List<GameObject>();
        }
    }
}
