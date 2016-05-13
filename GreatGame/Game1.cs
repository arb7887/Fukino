using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GreatGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // Fields
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Make the Game Manager
        private GameManager manager;

        private SpriteFont font;

        Texture2D buttonTexture, bulletTexture;
        SpriteFont buttonFont;

        // Mouse stuff
        MouseState currentMouse;
        MouseState previousMouse;

        // Keyboard Stuff
        KeyboardState previousKeyboard;
        KeyboardState keyboardState;

        // This is the camera that shall be used for the player
        private Camera _camera;
        private UserInterface UI;

        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // Make the screen bigger
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            // Make full screen when we get to the point of that


            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            UI = new UserInterface(GraphicsDevice);
            
            // Make a new Game Manager
            manager = new GameManager("Content/Units.txt", "Content/Textures.txt", currentMouse, previousMouse, UI);
            UI.Map = manager.GameMap;
            
            // Instantiates the list of units
            manager.Menu.initialize();
            manager.PMenu.Initialize();
            UI.Player1Units = manager.Player1Units;
            // Load in the Units.txt file, this works now


            //gameMap = new Map();

            this.IsMouseVisible = true;
            _camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
             
            buttonTexture = Content.Load<Texture2D>("ExampleButtonA.png");
            buttonFont = Content.Load<SpriteFont>("buttonFont");
            manager.Menu.LoadContent(buttonTexture, buttonFont, GraphicsDevice);
            manager.Menu.Title = Content.Load<Texture2D>("Title");
            manager.PMenu.LoadContent(buttonTexture, buttonFont);

            font = Content.Load<SpriteFont>("Arial14");

            manager.GameMap.WallTexture = Content.Load<Texture2D>("Black.png");
            manager.GameMap.LoadMap("Content/Walls.txt");
            manager.GameMap.CP = new CapturePoint(2400, 1400, 600, 600, Content.Load<Texture2D>("pointTexture"));
            bulletTexture = Content.Load<Texture2D>("Bullet.png");
            manager.E_bulletTexture = Content.Load<Texture2D>("e_Bullet.png");
            manager.BulletTexture = bulletTexture;
            //UI Textures:
            UI.BottomLeft = Content.Load<Texture2D>("BottomLeftUI.png");
            UI.BottomRight = Content.Load<Texture2D>("BottomRightUI.png");
            UI.Timer = Content.Load<Texture2D>("TimerOverlay.png");

            // Load in the list of units from the files here
            manager.LoadContent();

            manager.Grid_Texture = Content.Load<Texture2D>("gridBlock.png");

            for (int i = 0; i < manager.AllUnits.TextureList.Count; i++)
            {
                Texture2D newTexture = Content.Load<Texture2D>(manager.AllUnits.TextureList[i]);
                manager.UnitTextures.Add(newTexture); 
            }
            manager.HealthbarTexture = Content.Load<Texture2D>("Healthbar");
            #region Unit Icons
            manager.UnitIcons.Add("Alien", Content.Load<Texture2D>("AlienIcon"));
            manager.UnitIcons.Add("Assassin", Content.Load<Texture2D>("AssassinIcon"));
            manager.UnitIcons.Add("Engineer", Content.Load<Texture2D>("EngineerIcon"));
            manager.UnitIcons.Add("Medic", Content.Load<Texture2D>("MedicIcon"));
            manager.UnitIcons.Add("Minigun", Content.Load<Texture2D>("MinigunIcon"));
            manager.UnitIcons.Add("Rifle", Content.Load<Texture2D>("RifleIcon"));
            manager.UnitIcons.Add("Shotgun", Content.Load<Texture2D>("ShotgunIcon"));
            manager.UnitIcons.Add("Sniper", Content.Load<Texture2D>("SniperIcon"));
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            previousKeyboard = keyboardState;
            keyboardState = Keyboard.GetState();


            #region Camera Stuff
            if (manager.CurGameState == GameManager.GameState.Game)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 possibleMove = _camera.Pos;

                // camera movement
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    possibleMove = _camera.Pos - new Vector2(0, 250) * deltaTime;
                }

                if (keyboardState.IsKeyDown(Keys.S))
                {
                    possibleMove = _camera.Pos + new Vector2(0, 250) * deltaTime;
                }

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    possibleMove = _camera.Pos - new Vector2(250, 0) * deltaTime;
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    possibleMove = _camera.Pos + new Vector2(250, 0) * deltaTime;
                }
                #region Zooming which i don't know how to fix when it comes to mouse clickings
                /*
                if (keyboardState.IsKeyDown(Keys.Up) && _camera.Zoom <= 1.5)
                {
                    // Zoom in 
                    
                    _camera.Zoom += deltaTime;
                }

                if (keyboardState.IsKeyDown(Keys.Down) && _camera.Zoom >= 0.5)
                {
                    _camera.Zoom -= deltaTime;
                }*/
                #endregion

                // If the possible move is inside the bounds of the camera
                if (possibleMove.X >= -20 && possibleMove.X <= 850
                    && possibleMove.Y >= -20 && possibleMove.Y <= 350)
                {
                    _camera.Pos = possibleMove;
                }
            }
            #endregion


            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            // Call the managers update method


            MouseState mouse = Mouse.GetState();
            manager.Update(gameTime, previousMouse, currentMouse, GraphicsDevice, keyboardState, previousKeyboard, this, _camera);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is the draw method for our Game1 class
        /// This really just calls the GameManager's draw class
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            var viewMatrix = _camera.GetViewMatrix();
            // Second
            spriteBatch.Begin(transformMatrix: viewMatrix);

            // Draw the map in here
            manager.Draw(spriteBatch, font, _camera);            
            
            spriteBatch.End();

            //============================= USER INTERFACE ==================================
            spriteBatch.Begin();
            // DRAW THE USER INTERFACE STUFF HERE
            // This will make it so that they never move with the camera, and always stay in teh same position.
            if(manager.CurGameState == GameManager.GameState.Game)
            {
                UI.Draw(spriteBatch, font);
            }
            if (manager.CurGameState == GameManager.GameState.Paused)
            {
                manager.PMenu.Draw(spriteBatch);
            }
            //spriteBatch.DrawString(font, "Time: " + gameTime.TotalGameTime, new Vector2(1000, 100), Color.Red);
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        /// <summary>
        /// This is a method that allows us to quit the game from outside of the Game1 class
        /// </summary>
        public void Quit()
        {
            this.Exit();
        }
    }
}
