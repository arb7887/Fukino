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
        private Dictionary<string, Unit> unitDictionary;
        // Lists of the players 1 and 2 units
        private List<Unit> player1Units;
        private List<Enemy> enemy_Units;
        // This is a list of textures that has been loaded in
        private List<Texture2D> unitTextures;
        private Dictionary<string, Texture2D> unitIcons;
        private Texture2D bulletTexture;
        private Texture2D e_bulletTexture;
        // This is the menu handler for the main menu
        private MenuHandler menu;
        private PauseMenu pausemenu;

        private Unit selectedUnit;

        // FSM for the current game state
        public enum GameState { Menu, Game, Paused, GameOver }
        private GameState curGameState;

        UserInterface userInterface;
        bool changingClass;

        // Map stuff
        private Map gameMap;

        //For control point logic
        private float timer;

        private Grid _Grid;
        private Texture2D grid_texture;
        #endregion


        #region Properties

        // Properties
        public Texture2D Grid_Texture { get { return grid_texture; } set { grid_texture = value; } }
        public Grid Grid { get { return _Grid; } set { _Grid = value; } }
        public GameState CurGameState { get { return this.curGameState; } set { this.curGameState = value; } }
        public FileInput AllUnits { get { return this.allUnits; } }
        public List<Unit> Player1Units {get { return this.player1Units; } set { this.player1Units = value; } }
        public List<Enemy> Enemy_Units { get { return this.enemy_Units; } }
        public List<Texture2D> UnitTextures { get { return this.unitTextures; } set { this.unitTextures = value; } }
        public Dictionary<string, Texture2D> UnitIcons { get { return this.unitIcons; } set { this.unitIcons = value; } }
        public Texture2D BulletTexture { get { return this.bulletTexture; } set { this.bulletTexture = value; } }
        public MenuHandler Menu { get { return this.menu; } set { this.menu = value; } }
        public PauseMenu PMenu { get { return pausemenu; } set { pausemenu = value; } }
        public Map GameMap { get { return this.gameMap; } set { this.gameMap = value; } }
        public Texture2D E_bulletTexture { get { return this.e_bulletTexture; } set { this.e_bulletTexture = value; } }

        #endregion


        #region Constructor
        // Take in a string with a file name, and move all the file input to here
        public GameManager(String fileName, String texturesFileName, MouseState curMouse, MouseState prevMosue, UserInterface ui)
        {
            allUnits = new FileInput(fileName, texturesFileName);
            player1Units = new List<Unit>();
            enemy_Units = new List<Enemy>();
            unitTextures = new List<Texture2D>();
            unitIcons = new Dictionary<string, Texture2D>();
            curGameState = GameState.Menu;
            menu = new MenuHandler(MenuStates.Main);
            gameMap = new Map();
            timer = 0;
            pausemenu = new PauseMenu();
            userInterface = ui;
            unitDictionary = new Dictionary<string, Unit>();
        }

        #endregion


        #region Methods
        /// <summary>
        /// Eventually htis method will be used to match the names up to the different textures and stuff
        /// </summary>
        public void Initialize()
        {
            changingClass = false;

            float radius = 25;
            int x = 150;
            int textCount = 0;

            int index = 0;
            foreach(Unit u in player1Units)
            {
                u.Texture = unitTextures[index];
                u.UnitsDictionary = unitDictionary;
                index++;
            }
            
            // Set all of the player1 units textures to the same thing
            for(int i = 0; i < player1Units.Count; i++)
            {
                if(textCount >= 4)
                {
                    textCount = 0;
                }
                player1Units[i].Texture = unitTextures[textCount];
                player1Units[i].Icon = UnitIcons[player1Units[i].Name];
                player1Units[i].SpawnLoc = gameMap.PlayerSpawnPoints[i];
                player1Units[i].Position = player1Units[i].SpawnLoc;
                player1Units[i].Size = 50;
                player1Units[i].Destination = player1Units[i].Position;
                player1Units[i].Bounds = new BoundingSphere(new Vector3(player1Units[i].Position, 0), radius);
                textCount++;
                x += 75;
                player1Units[i].Team = Teams.Player;
                player1Units[i].BulletTexture = bulletTexture;
            }

            List<Vector2> enemyDestinations = new List<Vector2>();
            enemyDestinations.Add(new Vector2(2950, 1400));
            enemyDestinations.Add(new Vector2(2450, 1450));
            enemyDestinations.Add(new Vector2(2550, 1600));
            enemyDestinations.Add(new Vector2(2750, 1750));
            enemyDestinations.Add(new Vector2(2400, 1950));
            enemyDestinations.Add(new Vector2(2950, 1950));

            // Generate 6 random enemy units
            Random r = new Random();
            x = 4850;
            for(int i = 0; i < player1Units.Count; i++)
            {
                Unit unitToAdd = new Unit(allUnits.UnitList[r.Next(0, allUnits.UnitList.Count)]);
                
                unitToAdd.Texture = UnitTextures[0];
                unitToAdd.Position = new Vector2(x + 50, 250);
                unitToAdd.Size = 50;
                unitToAdd.Bounds = new BoundingSphere(new Vector3(unitToAdd.Position, 0), radius);
                unitToAdd.BulletTexture = E_bulletTexture;
                unitToAdd.Team = Teams.Enemy;
                
                Enemy enemyToAdd = new Enemy(unitToAdd, unitToAdd.AttackRange, enemyDestinations[i]);
                enemy_Units.Add(enemyToAdd);

                enemy_Units[i].SpawnLoc = gameMap.EnemySpawnPoints[i];
                enemy_Units[i].Position = enemy_Units[i].SpawnLoc;

                enemy_Units[i].Bounds = new BoundingSphere(new Vector3(enemy_Units[i].Position, 0), radius);
                x += 75;

            }
            // ================Set only one of the units closer so we can test bullets and stuff easier

            enemy_Units[0].Position = new Vector2(1000, 1700);
            enemy_Units[0].Bounds = new BoundingSphere(new Vector3(enemy_Units[0].Position, 0), radius);
            

            foreach (Unit u in enemy_Units)
            {
                u.UnitsDictionary = unitDictionary;
            }

            // Map the grid for pathfinding
            int[] wallArray = new int[] { 5 };

            _Grid = new Grid( Point.Zero,50, grid_texture, gameMap.Walls, gameMap);

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

        
        public void Update(GameTime gameTime, MouseState previousMouse, MouseState currentMouse, 
            GraphicsDevice graphics, KeyboardState kbState, KeyboardState kbPState, Game1 game, Camera cam)
        {
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
                        int index = 0;
                        foreach (Unit u in allUnits.UnitList)
                        {
                            u.Texture = unitTextures[index];
                            unitDictionary.Add(u.Name, u);
                            index++;
                        }
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
                    gameMap.CP.Contested = false; // first assume there is no unit contesting the point

                    //then check to see if the player right clicked
                    #region Player Movement with A*
                    if (previousMouse.RightButton == ButtonState.Pressed && currentMouse.RightButton == ButtonState.Released)
                    {   //and set the destination of the selecred unit
                        if(selectedUnit != null)
                        {
                            selectedUnit.Vertex_Im_ON = _Grid.SelectVertex(selectedUnit.Position);

                            if (selectedUnit.DONE_MOVING)
                            {
                                _Grid.ResetAllVertecies(selectedUnit.Vertex_Im_ON);
                                selectedUnit.Backwards_List.Clear();
                                selectedUnit.DONE_MOVING = false;
                                selectedUnit.Where_I_Am_In_List = -1;
                            }
                            else
                            {
                                // Sometime the priority queue class returns some error of being out of range, 
                                // So i use a try catch to keep the game moving if that happens. I'm not really sure why 
                                // It does that. This allso prevents the game from blowing up if you click on wanting to 
                                // Move into a wall. 
                                try
                                {
                                    selectedUnit.Destination = new Vector2(previousMouse.X + cam.Pos.X * cam.CamSpeed, previousMouse.Y + cam.Pos.Y * cam.CamSpeed);

                                    // Set the DESTINATION VERTEX of this unit to where evere th mouse is over
                                    selectedUnit.Destination_Vertex = _Grid.SelectVertex(selectedUnit.Destination);
                                    // Set the vertex that the current unit is on

                                    selectedUnit.Backwards_List = _Grid.ShortestPathSlow(selectedUnit.Vertex_Im_ON, selectedUnit.Destination_Vertex);
                                    selectedUnit.Where_I_Am_In_List = selectedUnit.Backwards_List.Count - 1;
                                }
                                catch(Exception e)
                                {
                                    return;
                                }

                            }


                        }
                        
                    }
                    #endregion
                    #region Other shit
                    //then check to see if the player hit space
                    //This is what you call cheating

                    /* 
                    if (kbPState.IsKeyDown(Keys.Space) && kbState.IsKeyUp(Keys.Space))
                    {   //and use the selected unit to attack if he did
                        if(selectedUnit != null)
                            selectedUnit.ShotgunSpray(new Vector2(currentMouse.X + cam.Pos.X * cam.CamSpeed, currentMouse.Y + cam.Pos.Y * cam.CamSpeed));
                    }
                    */

                    //check to see if the player hit c to change a unit class
                    if (kbPState.IsKeyDown(Keys.C) && kbState.IsKeyUp(Keys.C))
                    {
                        if (selectedUnit != null && selectedUnit.Bounds.Intersects(gameMap.PlayerSpawn))
                        {
                            changingClass = true;
                            selectedUnit.Tint = Color.Yellow;
                        }
                    }

                    //check to see if the player wants to quickSelect a class using shift, and a number
                    if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                    {
                        #region number input
                        if (kbState.IsKeyDown(Keys.D1))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[0];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        if (kbState.IsKeyDown(Keys.D2))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[1];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        if (kbState.IsKeyDown(Keys.D3))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[2];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        if (kbState.IsKeyDown(Keys.D4))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[3];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        if (kbState.IsKeyDown(Keys.D5))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[4];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        if (kbState.IsKeyDown(Keys.D6))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.Tint = Color.White;
                                selectedUnit.IsSelected = false;
                            }
                            selectedUnit = player1Units[5];
                            selectedUnit.Tint = Color.Cyan;
                            selectedUnit.IsSelected = true;
                            userInterface.Selected = selectedUnit;
                        }
                        #endregion
                    }

                    if (changingClass)
                    {
                        #region numpads input
                        if (kbPState.IsKeyDown(Keys.NumPad0))
                        {
                            selectedUnit.changeClass("Alien"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad1))
                        {
                            selectedUnit.changeClass("Assassin"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad2))
                        {
                            selectedUnit.changeClass("Engineer"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad3))
                        { 
                            selectedUnit.changeClass("Medic"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad4))
                        {
                            selectedUnit.changeClass("Minigun"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad5))
                        {
                            selectedUnit.changeClass("Rifle"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad6))
                        {
                            selectedUnit.changeClass("Shotgun"); changingClass = false;
                        }
                        if (kbPState.IsKeyDown(Keys.NumPad7))
                        {
                            selectedUnit.changeClass("Sniper"); changingClass = false;
                        }
                        #endregion
                    }

                    //now check to see if the player left clicked
                    if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                    {   //if so, see what he clicked on
                        Vector2 prevMouseVector = new Vector2(previousMouse.X, previousMouse.Y);
                        Vector2 mouseWorldPos = GetMouseWorldPos(prevMouseVector, cam.Pos * cam.CamSpeed);
                        selectedUnit = null;
                        userInterface.Selected = selectedUnit;
                        foreach (Unit u in player1Units)
                        {
                            if (u.CheckClicked(mouseWorldPos))
                            {
                                if(selectedUnit != null && selectedUnit.IsAlive)
                                {
                                    selectedUnit.Tint = Color.White;
                                    selectedUnit.IsSelected = false;
                                }

                                selectedUnit = u;
                                selectedUnit.Tint = Color.Cyan;
                                selectedUnit.IsSelected = true;
                                userInterface.Selected = selectedUnit;
                                break;
                            }
                        }
                    }

                    //finally update each unit
                    foreach (Unit u in player1Units)
                    {
                        u.Update(gameTime, enemy_Units, cam, gameMap);
                    }
                    #endregion


                    // Update the enemy AI Units
                    foreach (Enemy e in enemy_Units)
                    {
                        #region Enemy movement using A*
                        if (e.IS_FIRST_CALL)
                        {
                            try
                            {
                                e.Vertex_Im_ON = _Grid.SelectVertex(e.Position);
                                e.Destination_Vertex = _Grid.SelectVertex(e.Destination);

                                e.Backwards_List = _Grid.ShortestPathSlow(e.Vertex_Im_ON, e.Destination_Vertex);
                                e.Where_I_Am_In_List = e.Backwards_List.Count - 1;
                                e.IS_FIRST_CALL = false;
                                e.DONE_MOVING = false;
                            }
                            catch(Exception exception)
                            {

                                return;
                            }
                        }

                        if (e.DONE_MOVING)
                        {
                            _Grid.ResetAllVertecies(e.Vertex_Im_ON);
                            e.Backwards_List.Clear();
                            e.DONE_MOVING = false;
                            e.Where_I_Am_In_List = -1;
                        }
                        #endregion

                        e.Update(gameTime, player1Units);
                    }

                    // Check if we need to pause the game
                    if (kbState.IsKeyDown(Keys.Escape) && kbPState.IsKeyUp(Keys.Escape))
                        curGameState = GameState.Paused;

                    // This is the timer stuff for the actual capture point
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

                    //_Grid.Draw(sb);
                    break;
                case (GameState.Paused):
                    // Show some text about the current score, and the current untis health and what not
                    
                    break;
                case (GameState.GameOver):
                    // Show the teams score and the points and stuff
                    break;
            }
        }

        public Vector2 GetMouseWorldPos(Vector2 screenPos, Vector2 camPos)
        {
            return screenPos + camPos;
        }

        // Sets the listOfUnits to whatever the buttons are that the player has selected on the screen
        public void SetUnitsFromButtons()
        {
            menu.UserSelectedNames = this.Menu.GetButtonNames();
            for (int i = 0; i < menu.UserSelectedNames.Count; i++)
            {
                Unit tempUnit = unitDictionary[menu.UserSelectedNames[i]];
                player1Units.Add(new Unit(tempUnit));
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
        #endregion
    }
}
