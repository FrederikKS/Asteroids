using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    public enum GameState { mainMenu, enterName, inGame }
 
    class MainMenu
    {
        //Fields
        public GameState gameState;
        List<GUIElement> main = new List<GUIElement>();
        List<GUIElement> enterName = new List<GUIElement>();

        private Keys[] lastPressedKeys = new Keys[5];

        private string myName = string.Empty;

        private SpriteFont sf;

        //Properties
        public string MyName
        {
            get { return myName; }
            set { myName = value; }
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
        }

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

            enterName.Find(x => x.AssetName == "GUI/NameOK.png").MoveElement(0, 50);

        }

        public void Update()
        {
            switch (gameState)
            {
                case GameState.mainMenu:
                    foreach (GUIElement ele in main)
                    {
                        ele.Update();
                    }
                    break;
                case GameState.enterName:
                    foreach (GUIElement name in enterName)
                    {
                        name.Update();
                    }
                    GetKeys();
                    break;
                case GameState.inGame:
                    break;
                default:
                    break;
            }




        }

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
                    foreach (GUIElement name in enterName)
                    {
                        name.Draw(spriteBatch);
                    }
                    spriteBatch.DrawString(sf, myName, new Vector2((1200 / 2) - 90, (720 / 2) + 25), Color.White);
                    break;

                case GameState.inGame:
                    break;
                default:
                    break;
            }
        }

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
                //Quit game
            }
        }

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
    }
}
