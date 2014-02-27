using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Asteroids
{
    public enum GameState { mainMenu, enterName, inGame, highScore }
 
    class MainMenu
    {
        //Fields
        public GameState gameState;
        List<GUIElement> main = new List<GUIElement>();
        List<GUIElement> enterName = new List<GUIElement>();
        List<GUIElement> inGame = new List<GUIElement>();

        private string[] scores;
        private int[] sortedScores;
        private string[] sortedNames;

        private Keys[] lastPressedKeys = new Keys[5];

        private string myName = "Anonymous";

        private SpriteFont sf;

        public bool quitGame = false;
        public bool scoreAdded = false;

        //Properties
        public string MyName
        {
            get { return myName; }
            set { myName = value; }
        }
        public List<GUIElement> InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        //Constructor
        public MainMenu()
        {
            main.Add(new GUIElement("GUI/background.png"));
            main.Add(new GUIElement("GUI/MenuStart.png"));
            main.Add(new GUIElement("GUI/MenuQuit.png"));
            main.Add(new GUIElement("GUI/MenuName.png"));

            enterName.Add(new GUIElement("GUI/NameField"));
            enterName.Add(new GUIElement("GUI/NameOK.png"));

            inGame.Add(new GUIElement("GUI/CryBlue.png"));
            inGame.Add(new GUIElement("GUI/CryBrown.png"));
            inGame.Add(new GUIElement("GUI/CryGreen.png"));
            inGame.Add(new GUIElement("GUI/CryPurple.png"));
            inGame.Add(new GUIElement("GUI/CryRed.png"));
            inGame.Add(new GUIElement("GUI/CryYellow.png"));

        }
        /// <summary>
        /// Loads all the GUIElement contents
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sf = content.Load<SpriteFont>("SpriteFont1");
            foreach(GUIElement ele in main)
            {
                ele.LoadContent(content);
                ele.CenterElement(1200, 800);
                ele.clickEvent += OnClick;
            }
            main.Find(x => x.AssetName == "GUI/MenuStart.png").MoveElement(0, -50);
            main.Find(x => x.AssetName == "GUI/MenuQuit.png").MoveElement(0, 50);

            foreach(GUIElement name in enterName)
            {
                name.LoadContent(content);
                name.CenterElement(1200, 800);
                name.clickEvent += OnClick;
            }

            enterName.Find(x => x.AssetName == "GUI/NameOK.png").MoveElement(0, 100);

            foreach(GUIElement game in inGame)
            {
                game.LoadContent(content);
            }

        }
        /// <summary>
        /// Update function
        /// </summary>
        public void Update()
        {
            switch (gameState)
            {
                case GameState.mainMenu:
                    foreach (GUIElement ele in main)
                    {
                        ele.Update();
                    }
                    main.Find(x => x.AssetName == "GUI/background.png").RotateElement((float)(0.5*Math.PI/180));
                    break;
                case GameState.enterName:
                    foreach (GUIElement name in enterName)
                    {
                        name.Update();
                    }
                    main.Find(x => x.AssetName == "GUI/background.png").Update();
                    main.Find(x => x.AssetName == "GUI/background.png").RotateElement((float)(0.5 * Math.PI / 180));
                    GetKeys();
                    break;
                case GameState.inGame:
                    break;
                case GameState.highScore:
                    break;
                default:
                    break;
            }




        }
        /// <summary>
        /// Draw function like all the other draw functions :)
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.mainMenu:
                    foreach (GUIElement ele in main)
                    {
                        ele.Draw(spriteBatch);
                    }
                    break;

                case GameState.enterName:
                    main.Find(x => x.AssetName == "GUI/background.png").Draw(spriteBatch);
                    foreach (GUIElement name in enterName)
                    {
                        name.Draw(spriteBatch);
                    }
                    spriteBatch.DrawString(sf, myName, new Vector2((1200 / 2) - 90, (720 / 2) + 25), Color.White);
                    break;

                case GameState.inGame:
                    break;
                case GameState.highScore:
                    if (!scoreAdded)
                    {
                        string path = @"Content/HighScore.txt";
                        scores = new string[10];
                        // Open the file to read from. 
                        if (File.Exists(path))
                        {
                            using (StreamReader sr = File.OpenText("Content/HighScore.txt"))
                            {
                                string s = "";

                                for (int i = 0; i < scores.Length; i++)
                                {
                                    if ((s = sr.ReadLine()) != null)
                                        scores[i] = s;
                                }
                            }
                        }
                        

                        AddScoreToFile(GameManager.Instance.Score);
                        scoreAdded = true;
                    }

                    
                    spriteBatch.DrawString(sf, "Current Highscores:", new Vector2(100, 100), Color.White);
                    for (int i = 0; i < scores.Length; i++)
                    {
                        //if (scoreScores[i] != 0 && sortedNames[i] != null)
                            //spriteBatch.DrawString(sf, sortedNames[i] + " " + sortedScores[i], new Vector2(100, 130 + 30 * i), Color.White);
                        if (scores[i] != null)
                        spriteBatch.DrawString(sf, scores[i] /*+ ": " + GameManager.Instance.Score*/, new Vector2(100, 130 + 30 * i), Color.White);
                    }

                    spriteBatch.DrawString(sf, "Your Score" + ": " + GameManager.Instance.Score, new Vector2(100, 130 + 330), Color.White);
                    
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Change enum state on click
        /// </summary>
        /// <param name="element"></param>
        public void OnClick(string element)
        {
            if (element == "GUI/MenuStart.png")
            {
                gameState = GameState.inGame;
            }
            if (element == "GUI/MenuName.png")
            {
                gameState = GameState.enterName;
            }
            if (element == "GUI/NameOK.png")
            {
                gameState = GameState.mainMenu;
            }
            if (element == "GUI/MenuQuit.png")
            {
                quitGame = true;
            }
        }
        /// <summary>
        /// Read which key is pressed
        /// </summary>
        public void GetKeys()
        {
            KeyboardState kbState = Keyboard.GetState();

            Keys[] pressedKeys = kbState.GetPressedKeys();

            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                {
                    //key is no longer pressed
                    OnKeyUp(key);
                }
            }
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                {
                    OnKeyDown(key);
                }
            }

            lastPressedKeys = pressedKeys;
        }

        public void OnKeyUp(Keys key)
        {

        }
        public void OnKeyDown(Keys key)
        {
            if (key == Keys.Back && myName.Length > 0)
            {
                myName = myName.Remove(myName.Length - 1);
            }
            else
            {
                if (key != Keys.LeftShift && key != Keys.LeftControl && key != Keys.LeftAlt && key != Keys.Back && key != Keys.Enter)
                myName += key.ToString();
            }
        }

        /// <summary>
        /// Unfinished sorting method. Currently only adds plays score to a .txt file containing scores
        /// </summary>
        /// <param name="score"></param>
        public void AddScoreToFile(int score)
        {
            string path = @"Content/HighScore.txt";
            
            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(myName + ": " + GameManager.Instance.Score);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(myName + ": " + GameManager.Instance.Score);
                }
            }

            //SortScore(scores, GameManager.Instance.Score);
        }
        /// <summary>
        /// Unfinished attempt at sorting scores. Perhaps I should have looked on the internet for advice.
        /// </summary>
        /// <param name="scores"></param>
        /// <param name="newScore"></param>
        private void SortScore(string[] scores, int newScore)
        {
            string temp = "";
            char[] charListScore = new char[scores.Length];
            string[] nameList = new string[10];
            int numOfScores = 0;
            int[] tempScores = new int[10];

            for (int i = 0; i < 10; i++)
            {
                if (scores[i] != null)
                {
                    {
                        numOfScores++;
                        for (int x = 6; x >= 0; x--)
                        {
                            if (scores[i][scores[i].Length - 1 - i] == '0' || scores[i][scores[i].Length - 1 - i] == '1' || scores[i][scores[i].Length - 1 - i] == '2' || scores[i][scores[i].Length - 1 - i] == '3' || scores[i][scores[i].Length - 1 - i] == '4' || scores[i][scores[i].Length - 1 - i] == '5' || scores[i][scores[i].Length - 1 - i] == '6' || scores[i][scores[i].Length - 1 - i] == '7' || scores[i][scores[i].Length - 1 - i] == '8' || scores[i][scores[i].Length - 1 - i] == '9')
                            {
                                charListScore[i] = scores[i][scores[i].Length - 1 - i];
                            }
                        }

                        temp = Convert.ToString((charListScore[i]) + temp);

                        for (int y = 0; y < nameList.Length; y++)
                        {
                            nameList[i] = nameList[i] + scores[i][y];

                        }

                        tempScores[i] = Convert.ToInt32(temp);
                        
                    }
                }
                 
            }

            SortedScore(tempScores, numOfScores, nameList);

            
        }
        /// <summary>
        /// Last part of the failed sorting attempt. Will try to fix this at a later point.
        /// </summary>
        /// <param name="tempScores"></param>
        /// <param name="numOfScores"></param>
        /// <param name="names"></param>
        private void SortedScore(int[] tempScores, int numOfScores, string[] names)
        {
            int[] sortedScore = new int[numOfScores];
            string[] sortedName = new string[numOfScores];

            for (int i = 0; i < numOfScores; i++)
            {
                int numberOfScoresOver = 0;
                int numberOfScoresBelow = 0;
                int scoreA = tempScores[i];
                string nameA = Convert.ToString(names[i]);

                for (int x = 0; x < numOfScores; x++)
                {
                    int scoreB = tempScores[x];
                    if (scoreA >= scoreB)
                    {
                        numberOfScoresBelow++;
                    }
                    else
                    {
                        numberOfScoresOver++;
                    }
                }

                sortedScore[numberOfScoresOver] = scoreA;
                sortedName[numberOfScoresOver] = nameA;

            }

            sortedScores = sortedScore;
            sortedNames = sortedName;

        }
    }
}
