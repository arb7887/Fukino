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

        private List<Unit> userSelectedUnits;   // The list of units that the user has selected

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
        UserInterface UI;

        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // Make the screen bigger
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            // Make full screen when we get to the point of that
            //graphics.IsFullScreen = true;


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
            // Make a new Game Manager
            manager = new GameManager("Content/Units.txt", "Content/Textures.txt", currentMouse, previousMouse);

            // Instantiates the list of units
            manager.Menu.initialize();

            // Load in the Units.txt file, this works now
            userSelectedUnits = new List<Unit>();
            UI = new UserInterface();

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

            font = Content.Load<SpriteFont>("Arial14");

            manager.GameMap.WallTexture = Content.Load<Texture2D>("wallTexture.jpg");
            manager.GameMap.LoadMap("Content/Walls.txt");
            manager.GameMap.CP = new CapturePoint(2400, 1400, 600, 600, Content.Load<Texture2D>("pointTexture"));
            bulletTexture = Content.Load<Texture2D>("Bullet.png");
            manager.BulletTexture = bulletTexture;
            // Load in the list of units from the files here
            manager.LoadContent();

            for (int i = 0; i < manager.AllUnits.TextureList.Count; i++)
            {
                Texture2D newTexture = Content.Load<Texture2D>(manager.AllUnits.TextureList[i]);
                manager.UnitTextures.Add(newTexture);
            }
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
             if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
               Exit();
            previousKeyboard = keyboardState;
            keyboardState = Keyboard.GetState();


            #region Camera Stuff
            if (manager.CurGameState == GameManager.GameState.Game)
            {           
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                // camera movement
                if (keyboardState.IsKeyDown(Keys.W))
                    _camera.Pos -= new Vector2(0, 250) * deltaTime;

                if (keyboardState.IsKeyDown(Keys.S))
                    _camera.Pos += new Vector2(0, 250) * deltaTime;

                if (keyboardState.IsKeyDown(Keys.A))
                    _camera.Pos -= new Vector2(250, 0) * deltaTime;

                if (keyboardState.IsKeyDown(Keys.D))
                    _camera.Pos += new Vector2(250, 0) * deltaTime;
        }
            #endregion


            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            // Call the managers update method


            MouseState mouse = Mouse.GetState();
            manager.Update(gameTime, previousMouse, currentMouse, userSelectedUnits, GraphicsDevice, keyboardState, previousKeyboard, this, _camera);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is the draw method for our Game1 class
        /// This really just calls the GameManager's draw class
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

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
