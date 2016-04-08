using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class GameManager
    {
        // Fields
        #region Fields
        // List of units for the players to choose from
        private FileInput allUnits;   
        // Lists of the players 1 and 2 units
        private List<Unit> player1Units;
        private List<Unit> player2Units;
        // This is a list of textures that has been loaded in
        private List<Texture2D> unitTextures;
        private Texture2D bulletTexture;

        // This is the menu handler for the main menu
        private MenuHandler menu;

        // FSM for the current game state
        public enum GameState { Menu, Game, Paused, GameOver }
        private GameState curGameState;

        // Mouse stuff
        private MouseState currentMouse;
        private MouseState previousMouse;
        #endregion


        #region Properties
        // Properties
        public GameState CurGameState { get { return this.curGameState; } set { this.curGameState = value; } }
        public FileInput AllUnits { get { return this.allUnits; } }
        public List<Unit> Player1Units {get { return this.player1Units; } set { this.player1Units = value; } }
        public List<Unit> Player2Units { get { return this.player2Units; } }
        public List<Texture2D> UnitTextures { get { return this.unitTextures; } set { this.unitTextures = value; } }
        public Texture2D BulletTexture { get { return this.bulletTexture; } set { this.bulletTexture = value;} }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }

        #endregion


        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName, MouseState curMouse, MouseState prevMouse)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            player2Units = new List<Unit>();
            unitTextures = new List<Texture2D>();
            curGameState = GameState.Menu;
            menu = new MenuHandler(MenuStates.Main);

        }

        /// <summary>
        /// Eventually htis method will be used to match the names up to the different textures and stuff
        /// </summary>
        public void Initialize()
        {
            int x = 0;
            int textCount = 0;
            // Set all of the player1 units textures to the same thing
            for(int i = 0; i < player1Units.Count; i++)
            {
                if(textCount >= 3)
                {
                    textCount = 0;
                }
                player1Units[i].Texture = unitTextures[textCount];
                player1Units[i].Position = new Vector2(x + 10, 100);
                player1Units[i].Size = 50;
                player1Units[i].Center = new Vector2(player1Units[i].Position.X + player1Units[i].Size / 2, player1Units[i].Position.Y + player1Units[i].Size / 2);
                player1Units[i].BulletTexture = bulletTexture;
                textCount++;
                x += 100;
            }
            player2Units.Add(new Unit("Bob", 100, 3, 5, 5, 2));
            player2Units[0].Position = new Vector2(500, 500);
            player2Units[0].Size = 50;
            player2Units[0].Center = new Vector2(player2Units[0].Position.X + player2Units[0].Size / 2, player2Units[0].Position.Y + player2Units[0].Size / 2);
            player2Units[0].Texture = UnitTextures[0];
        }
        
        /// <summary>
        /// This loads the content from the FileInput object
        /// </summary>
        public void LoadContent()
        {
            // Load in from the notepad document of units
            allUnits.LoadUnit();
            allUnits.LoadTextures();            
        }

        /// <summary>
        /// This method loops through all the units in the player1 list, and calls their update method
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="previousMouse"></param>
        /// <param name="currentMouse"></param>
        /// <param name="userSelectedUnits"></param>
        /// <param name="graphics"></param>
        public void Update(GameTime gameTime, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, GraphicsDevice graphics, KeyboardState kbState)
        {
            // Loop through both of the arrays of units and call the Unit's update function
            switch (curGameState)
            {
                case (GameState.Menu):
                    // if (Menu.ExitGame)
                    //    Exit();
                    menu.Update(currentMouse, graphics);
                    if (menu.StartGame)
                    {
                        curGameState = GameState.Game;
                        // Load in the list of class selected units and add them to the list of units in 
                        // This game one class's list called 'userSelectedUnits'
                        SetUnitsFromButtons();
                        Initialize();
                    }
                    break;
                case (GameState.Game):
                    // Call the updates on all of the units in the players list
                    for(int i = 0; i < player1Units.Count; i++)
                    {
                        player1Units[i].Update(gameTime, previousMouse, currentMouse, userSelectedUnits, player2Units);
                        if (player1Units[i].Health < 0)
                        {
                            player1Units.Remove(player1Units[i]);
                        }
                    }
                    for (int i = 0; i < player2Units.Count; i++)
                    {
                        if (player2Units[i].Health <= 0)
                        {
                            player2Units.Remove(player2Units[i]);
                        }
                    }
                    // Check for the button push of some key pause the game

                    // Check to see if they pushed a button to use a special ability

                    break;
                case (GameState.Paused):
                    // Check to see if the paused button is pressed again
                    // A button to give up, which will set you to game over
                    break;
                case(GameState.GameOver):
                    // Check to see if the user has pushed something to go back to the main menu
                    // Show a button to go back to the menu
                    break;
            }
        }

        // Call the unit's draw methods and the maps draw method
        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            // Loop through both of the arrays of units and call the Unit's update function
            switch (curGameState)
            {
                case (GameState.Menu):
                    // Draw the menu handlers thing
                    menu.Draw(sb);
                    break;
                case (GameState.Game):
                    // Call the updates on all of the units in the players list
                    for (int i = 0; i < player1Units.Count; i++)
                    {
                        player1Units[i].Draw(sb, font);
                        for (int j = 0; j < player1Units[i].ActiveBullets.Count; j++)
                        {
                            player1Units[i].ActiveBullets[j].Draw(sb);
                        }
                    }
                    sb.DrawString(font, Player1String(), Vector2.Zero, Color.Black);
                    if (player2Units.Count > 0)
                    {
                        player2Units[0].Draw(sb, font);
                    }
                    break;
                case (GameState.Paused):
                    // Show some text about the current score, and the current untis health and what not
                    break;
                case (GameState.GameOver):
                    // Show the teams score and the points and stuff
                    break;
            }
        }

        // Sets the listOfUnits to whatever the buttons are that the player has selected on the screen
        public void SetUnitsFromButtons()
        {
            menu.UserSelectedNames = this.Menu.GetButtonNames();
            for (int i = 0; i < menu.UserSelectedNames.Count; i++)
            {
                // Go through and find the actual unit object with the same name as the button
                for (int j = 0; j < this.AllUnits.ListCount; j++)
                {
                    if (menu.UserSelectedNames[i] == this.AllUnits.UnitList[j].Name)
                    {
                        player1Units.Add(new Unit(allUnits.UnitList[j]));
                    }
                }
            }
        }

        public string Player1String()
        {
            string thing = "";
            for(int i = 0; i < player1Units.Count; i++)
            {
                thing += " " + player1Units[i].Name;
            }
            return thing;
        }


    }
}
