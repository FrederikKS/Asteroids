using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class GameManager
    {
        private static GameManager instance;

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
        private List<GameObject> allObjects;
        private List<GameObject> tempList;

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

        private GameManager()
        {
            allObjects = new List<GameObject>();
            tempList = new List<GameObject>();
        }
    }
}
