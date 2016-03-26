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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Make the Game Manager
        private GameManager manager;

        private SpriteFont font;

        private List<Unit> userSelectedUnits;   // The list of units that the user has selected

        Texture2D buttonTexture;
        SpriteFont buttonFont;

        // Mouse stuff
        MouseState currentMouse;
        MouseState previousMouse;

        Map gameMap;

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
            // Make a new Game Manager
            manager = new GameManager("Content/Units.txt", "Content/Textures.txt", currentMouse, previousMouse);

            // Instantiates the list of units
            manager.Menu.initialize();

            // Load in the Units.txt file, this works now
            userSelectedUnits = new List<Unit>();

            //gameMap = new Map();

            this.IsMouseVisible = true;
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

            KeyboardState kbState = Keyboard.GetState();
            

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            // Call the managers update method
            manager.Update(gameTime, previousMouse, currentMouse, userSelectedUnits, GraphicsDevice, kbState);

            MouseState mouse = Mouse.GetState();

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

            spriteBatch.Begin();

           // gameMap.Draw(spriteBatch);

            // Call the managers Draw method
            manager.Draw(spriteBatch, font);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
