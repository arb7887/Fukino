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


        // Map stuff
        private Map gameMap;

        //For control point logic
        private float timer;
        #endregion


        #region Properties

        // Properties
        public GameState CurGameState { get { return this.curGameState; } set { this.curGameState = value; } }
        public FileInput AllUnits { get { return this.allUnits; } }
        public List<Unit> Player1Units {get { return this.player1Units; } set { this.player1Units = value; } }
        public List<Unit> Player2Units { get { return this.player2Units; } }
        public List<Texture2D> UnitTextures { get { return this.unitTextures; } set { this.unitTextures = value; } }
        public Texture2D BulletTexture { get { return this.bulletTexture; } set { this.bulletTexture = value; } }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }
        public Map GameMap { get { return this.gameMap; } set { this.gameMap = value; } }

        #endregion


        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName, MouseState curMouse, MouseState prevMosue)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            player2Units = new List<Unit>();
            unitTextures = new List<Texture2D>();
            curGameState = GameState.Menu;
            menu = new MenuHandler(MenuStates.Main);
            gameMap = new Map();
            timer = 0;
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
                player1Units[i].Position = new Vector2(x + 150, 200);
                player1Units[i].Size = 50;
                player1Units[i].Center = new Vector2(player1Units[i].Position.X + (player1Units[i].Size / 2), player1Units[i].Position.Y + (player1Units[i].Size / 2));
                player1Units[i].Bounds = new BoundingSphere(new Vector3(player1Units[i].Position.X, player1Units[i].Position.Y, 0), 25);
                textCount++;
                x += 75;
                player1Units[i].Team = Teams.Player;
                player1Units[i].BulletTexture = bulletTexture;
            }
            /*
            player2Units.Add(new Unit("Bob", 100, 3, 150, 5, 1, 1));
            player2Units[0].Position = new Vector2(500, 500);
            player2Units[0].Size = 50;
            player2Units[0].Center = new Vector2(player2Units[0].Position.X + player2Units[0].Size / 2, player2Units[0].Position.Y + player2Units[0].Size / 2);
            player2Units[0].Bounds = new BoundingSphere(new Vector3(player2Units[0].Position, 0), 25);
            player2Units[0].Texture = UnitTextures[0];
            player2Units[0].BulletTexture = bulletTexture;*/
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
        public void Update(GameTime gameTime, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, 
            GraphicsDevice graphics, KeyboardState kbState, KeyboardState kbPState, Game1 game, Camera cam)
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
                    else if (menu.ExitGame)
                    {
                        // Quit the game                  
                        game.Quit();
                    }
                    break;
                case (GameState.Game):
                    // Call the updates on all of the units in the players list                    
                    for (int i = 0; i < player1Units.Count; i++)
                    {
                        // Check to see if the mouse is even inside of the window, if not, then don't bother calling the update method
                        // THE GAME SHOULD PLAY IN THE BACKGROUND
                        player1Units[i].Update(gameTime, previousMouse, currentMouse, userSelectedUnits, player2Units, cam, gameMap);
                        

                    }
                    for (int i = 0; i < player2Units.Count; i++)
                    {
                        player2Units[i].UpdateEnemy(gameTime, player1Units);
                        if (player2Units[i].Health <= 0)
                        {
                            player2Units.Remove(player2Units[i]);
                        }
                    }

                    if (kbState.IsKeyDown(Keys.P) && kbPState.IsKeyUp(Keys.P)) curGameState = GameState.Paused;



                    var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timer += delta;
                    if (timer >= 1)
                    {
                        gameMap.CP.Count();
                        timer -= 1;
                    }


                    break;
                case (GameState.Paused):
                    // Check to see if the paused button is pressed again
                    if (kbState.IsKeyDown(Keys.P) && kbPState.IsKeyUp(Keys.P))
                    {
                        curGameState = GameState.Game;
                    }
                    // A button to give up, which will set you to game over
                    break;
                case(GameState.GameOver):
                    // Check to see if the user has pushed something to go back to the main menu
                    // Show a button to go back to the menu
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        CurGameState = GameState.Menu;
                    }
                    break;
            }
        }

        // Call the unit's draw methods and the maps draw method
        public void Draw(SpriteBatch sb, SpriteFont font, Camera cam)
        {
            // Loop through both of the arrays of units and call the Unit's update function
            switch (curGameState)
            {
                case (GameState.Menu):
                    // Draw the menu handlers thing
                    menu.Draw(sb);
                    break;
                case (GameState.Game):
                    // Draw the map
                    DrawMap(sb);
                    // Draw the units
                    // Call the updates on all of the units in the players list
                    for (int i = 0; i < player1Units.Count; i++)
                    {
                        if (player1Units[i].IsAlive)
                        {
                            player1Units[i].Draw(sb, font, cam);
                            for (int j = 0; j < player1Units[i].ActiveBullets.Count; j++)
                            {
                                if (!player1Units[i].ActiveBullets[j].ToDelete)
                                {
                                    player1Units[i].ActiveBullets[j].Draw(sb);
                                }
                            }
                        }
                    }


                    for (int k = 0; k < player2Units.Count; k++)
                    {
                        if (player2Units[k].IsAlive)
                        {
                            player2Units[k].Draw(sb, font, cam);
                            for (int l = 0; l < player2Units[k].ActiveBullets.Count; l++)
                            {
                                if (!player2Units[k].ActiveBullets[l].ToDelete)
                                {
                                    player2Units[k].ActiveBullets[l].Draw(sb);
                                }
                            }
                        }
                    }
                    sb.DrawString(font, Player1String(), Vector2.Zero, Color.Black);
                    break;
                case (GameState.Paused):
                    // Show some text about the current score, and the current untis health and what not
                    sb.DrawString(font, "Paused! Press Escape to Continue", new Vector2(500, 500), Color.Black);
                    
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
                        player1Units.Add(new Unit(allUnits.UnitList[j], i));
                    }
                }
            }
        }

        public string Player1String()
        {
            string thing = "";
            for(int i = 0; i < player1Units.Count; i++)
            {
                thing += " " + player1Units[i].Name + player1Units[i].Bounds.ToString();
            }
            return thing;
        }


        public void DrawMap(SpriteBatch sb)
        {
            if (curGameState == GameState.Game)
                gameMap.Draw(sb);
        }
    }
}
