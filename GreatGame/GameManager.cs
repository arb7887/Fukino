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
        private List<Enemy> enemy_Units;
        // This is a list of textures that has been loaded in
        private List<Texture2D> unitTextures;
        private List<Texture2D> unitIcons;
        private Texture2D bulletTexture;
        private Texture2D e_bulletTexture;
        // This is the menu handler for the main menu
        private MenuHandler menu;
        private PauseMenu pausemenu;

        // FSM for the current game state
        public enum GameState { Menu, Game, Paused, GameOver }
        private GameState curGameState;

        // Mouse stuff
        private MouseState currentMouse;
        private MouseState previousMouse;

        private float timeRemaining;


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
        public List<Enemy> Enemy_Units { get { return this.enemy_Units; } }
        public List<Texture2D> UnitTextures { get { return this.unitTextures; } set { this.unitTextures = value; } }
        public List<Texture2D> UnitIcons { get { return this.unitIcons; } set { this.unitIcons = value; } }
        public Texture2D BulletTexture { get { return this.bulletTexture; } set { this.bulletTexture = value; } }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }
        public PauseMenu PMenu { get { return pausemenu; } set { pausemenu = value; } }
        public Map GameMap { get { return this.gameMap; } set { this.gameMap = value; } }
        public Texture2D E_bulletTexture { get { return this.e_bulletTexture; } set { this.e_bulletTexture = value; } }

        #endregion


        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName, MouseState curMouse, MouseState prevMosue)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            enemy_Units = new List<Enemy>();
            unitTextures = new List<Texture2D>();
            unitIcons = new List<Texture2D>();
            curGameState = GameState.Menu;
            menu = new MenuHandler(MenuStates.Main);
            gameMap = new Map();
            timer = 0;
            timeRemaining = 10000;
            pausemenu = new PauseMenu();
        }

        /// <summary>
        /// Eventually htis method will be used to match the names up to the different textures and stuff
        /// </summary>
        public void Initialize()
        {
            float radius = 25;
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
                player1Units[i].Center = new Vector2(player1Units[i].Position.X + radius, player1Units[i].Position.Y + radius);
                player1Units[i].Bounds = new BoundingSphere(new Vector3(player1Units[i].Position, 0), radius);
                textCount++;
                x += 75;
                player1Units[i].Team = Teams.Player;
                player1Units[i].BulletTexture = bulletTexture;
            }

            Random r = new Random();
            // Generate 6 random enemy units
            x = 1000;
            for(int i = 0; i < player1Units.Count; i++)
            {
                Unit unitToAdd = new Unit(allUnits.UnitList[r.Next(0, allUnits.UnitList.Count)], i);
                
                unitToAdd.Texture = UnitTextures[0];
                unitToAdd.Position = new Vector2(x + 50, 250);
                unitToAdd.Size = 50;
                unitToAdd.Bounds = new BoundingSphere(new Vector3(unitToAdd.Position, 0), radius);
                unitToAdd.Center = new Vector2(unitToAdd.Position.X + radius, unitToAdd.Position.Y + radius);
                unitToAdd.BulletTexture = E_bulletTexture;
                unitToAdd.Team = Teams.Enemy;
                Enemy enemyToAdd = new Enemy(unitToAdd, i, unitToAdd.AttackRange);

                enemy_Units.Add(enemyToAdd);
                enemy_Units[i].Position = new Vector2(500 + x, 1700);
                enemy_Units[i].Bounds = new BoundingSphere(new Vector3(enemy_Units[i].Position, 0), radius);
                x += 100;
            }
            // ================Set only one of the units closer so we can test bullets and stuff easier
            enemy_Units[0].Position = new Vector2(1000, 1700);
            enemy_Units[0].Bounds = new BoundingSphere(new Vector3(enemy_Units[0].Position, 0), radius);

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
                    menu.Update(currentMouse, graphics);
                    // If the player clicks on start game
                    if (menu.StartGame)
                    {
                        curGameState = GameState.Game;
                        // Load in the list of class selected units and add them to the list of units in 
                        // This game one class's list called 'userSelectedUnits'
                        SetUnitsFromButtons();
                        Initialize();
                    }
                    // IF the player clicks on exit
                    else if (menu.ExitGame)
                    {
                        // Quit the game                  
                        game.Quit();
                    }
                    break;
                case (GameState.Game):
                    gameMap.CP.Contested = false;
                    // Call the updates on all of the units in the players list   
                    for (int i = 0; i < player1Units.Count; i++)
                    {
                        player1Units[i].Update(gameTime, previousMouse, currentMouse, kbPState, kbState, userSelectedUnits, enemy_Units, cam, gameMap);
                    }
                    
                    // Update the enemy AI Units
                    for (int i = 0; i < enemy_Units.Count; i++)
                    {
                        enemy_Units[i].Update(gameTime, player1Units);
                    }

                    if (kbState.IsKeyDown(Keys.Escape) && kbPState.IsKeyUp(Keys.Escape)) curGameState = GameState.Paused;



                    var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timer += delta;
                    if (timer >= 1)
                    {
                        gameMap.CP.Count();
                        timer -= 1;
                    }
                    if(gameMap.CP.Winner != Teams.Neutral)
                    {
                        curGameState = GameState.GameOver;
                    }

                    break;
                case (GameState.Paused):
                    // Check to see if the paused button is pressed again
                    pausemenu.Update(currentMouse);
                    if (kbState.IsKeyDown(Keys.Escape) && kbPState.IsKeyUp(Keys.Escape))
                    {
                        curGameState = GameState.Game;
                    }
                    if (pausemenu.Resume.CheckClicked(currentMouse))
                    {
                        curGameState = GameState.Game;
                    }
                    if (pausemenu.MainMenu.CheckClicked(currentMouse))
                    {
                        game.Exit();
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


                    for (int k = 0; k < enemy_Units.Count; k++)
                    {
                        if (enemy_Units[k].IsAlive)
                        {
                            enemy_Units[k].Draw(sb, font);
                            for (int l = 0; l < enemy_Units[k].ActiveBullets.Count; l++)
                            {
                                if (!enemy_Units[k].ActiveBullets[l].ToDelete)
                                {
                                    enemy_Units[k].ActiveBullets[l].Draw(sb);
                                }
                            }
                        }
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
