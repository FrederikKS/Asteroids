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

        //Properties
        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }
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
        internal List<GameObject> RemoveWhenPossible
        {
            get { return removeWhenPossible; }
            set { removeWhenPossible = value; }
        }

        internal List<GameObject> TempList
        {
            get { return tempList; }
            set { tempList = value; }
        }

        public List<GameObject> AllObjects
        {
            get { return allObjects; }
            set { allObjects = value; }
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
