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
        private List<Texture2D> unitTextures;

        private MenuHandler menu;

        // FSM for the current game state
        enum GameState { Menu, Game, Paused, GameOver }
        private GameState gameState;

        // Mouse stuff
        private MouseState currentMouse;
        private MouseState previousMouse;

        // Properties
        public FileInput AllUnits { get { return this.allUnits; } }
        public List<Unit> Player1Units
        {
            get { return this.player1Units; }
            set { this.player1Units = value; }
        }
        public List<Unit> Player2Units { get { return this.player2Units; } }
        public List<Texture2D> UnitTextures
        {
            get { return this.unitTextures; }
            set { this.unitTextures = value; }
        }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }


        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            player2Units = new List<Unit>();
            unitTextures = new List<Texture2D>();
            gameState = GameState.Game;
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
        public void Update(GameTime gameTime, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits)
        {
            // Loop through both of the arrays of units and call the Unit's update function
            switch (gameState)
            {
                case (GameState.Menu):
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
        public void Draw(SpriteBatch sb)
        {
            // Loop through both of the arrays of units and call the Unit's update function
            switch (gameState)
            {
                case (GameState.Menu):
                    break;
                case (GameState.Game):
                    // Call the updates on all of the units in the players list
                    for (int i = 0; i < player1Units.Count; i++)
                    {
                        player1Units[i].Draw(sb);
                    }
                    break;
                case (GameState.Paused):
                    break;
                case (GameState.GameOver):
                    break;
            }
        }

        
    }
}
