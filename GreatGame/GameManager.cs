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
        // List of units for the players to choose from
        private FileInput allUnits;   
        // Lists of the players 1 and 2 units
        private List<Unit> player1Units;
        private List<Unit> player2Units;
        // This is a list of textures that has been loaded in
        private List<Texture2D> unitTextures;

        // This is the menu handler for the main menu
        private MenuHandler menu;

        // FSM for the current game state
        public enum GameState { Menu, Game, Paused, GameOver }
        private GameState curGameState;

        // Mouse stuff
        private MouseState currentMouse;
        private MouseState previousMouse;

        // Properties
        public GameState CurGameState { get { return this.curGameState; } set { this.curGameState = value; } }
        public FileInput AllUnits { get { return this.allUnits; } }
        public List<Unit> Player1Units
        {
            get { return this.player1Units; }
            set { this.player1Units = value; }
        }
        public List<Unit> Player2Units { get { return this.player2Units; } }
        public List<Texture2D> UnitTextures { get { return this.unitTextures; } set { this.unitTextures = value; } }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }


        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            player2Units = new List<Unit>();
            unitTextures = new List<Texture2D>();
            curGameState = GameState.Menu;
            menu = new MenuHandler(MenuStates.Main);

        }

        public void Initialize()
        {
            int x = 0;
            int y = 0;
            // Set all of the player1 units textures to the same thing
            for(int i = 0; i < player1Units.Count; i++)
            {
                player1Units[i].Texture = unitTextures[0];
                player1Units[i].Position = new Vector2(x, y);
                x += 100;
                y += 100;
            }
            
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

        // Call the unit's update method
        // Call the Map's update method
        public void Update(GameTime gameTime, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, GraphicsDevice graphics)
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
                        player1Units[i].Update(gameTime, previousMouse, currentMouse, userSelectedUnits);
                    }
                    break;
                case (GameState.Paused):
                    break;
                case(GameState.GameOver):
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
                    }
                    break;
                case (GameState.Paused):
                    break;
                case (GameState.GameOver):
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
                        this.Player1Units.Add(this.AllUnits.UnitList[j]);
                    }
                }
            }
        }


    }
}
