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
        private List<String> userSelectedNames; // This is the lsit of names from the buttons that the user picks on the menu screen

        Texture2D buttonTexture;
        SpriteFont buttonFont;

        // Mouse stuff
        MouseState currentMouse;
        MouseState previousMouse;

        private enum GameStates
        {
            Menu,
            Game,
            GameOver
        }

        private GameStates currentState;


        Unit test;
        Unit test2;

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
            manager = new GameManager("Content/Units.txt", "Content/Textures.txt");

            // Instantiates the list of units
            currentState = GameStates.Menu;

            //manager.Menu = new MenuHandler(MenuStates.Main);
            manager.Menu.initialize();


            // Load in the Units.txt file, this works now
            userSelectedUnits = new List<Unit>();
            userSelectedNames = new List<string>();

            test = new Unit("Test", 10, 10, 10, 10);
            test2 = new Unit("Test2", 15, 15, 15, 15);

            test.Position = new Vector2(0, 0);
            test2.Position = new Vector2(200, 200);

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
            
            // Load in the list of units from the files here
            manager.LoadContent();

            for (int i = 0; i < manager.AllUnits.TextureList.Count; i++)
            {
                Texture2D newTexture = Content.Load<Texture2D>(manager.AllUnits.TextureList[i]);
                manager.UnitTextures.Add(newTexture);
            }

            test.Texture = manager.UnitTextures[0];
            test2.Texture = manager.UnitTextures[1];
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

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            switch (currentState)
            {
                case GameStates.Menu:
                    manager.Update(gameTime, previousMouse, currentMouse, userSelectedUnits, GraphicsDevice);
                    //manager.Menu.Update(currentMouse, GraphicsDevice);
                    /*
                    if (manager.Menu.ExitGame)
                        Exit();
                    if (manager.Menu.StartGame)
                    {
                        currentState = GameStates.Game;
                        // Load in the list of class selected units and add them to the list of units in 
                        // This game one class's list called 'userSelectedUnits'
                        manager.SetUnitsFromButtons();
                        manager.Initialize();
                    }*/
                        
                    break;
                case GameStates.Game:
                    manager.Update(gameTime, previousMouse, currentMouse, userSelectedUnits, GraphicsDevice);
                    break;
                case GameStates.GameOver:
                    // Check for if the user has hit enter to return to title screen
                    break;
            }
            
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

            switch (currentState)
            {
                case GameStates.Menu:
                    manager.Draw(spriteBatch, font);
                    break;
                case GameStates.Game:
                    manager.Draw(spriteBatch, font);
                    break;
                case GameStates.GameOver:
                    // Print out some info about the score and stuff
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }


        /*
        // Sets the listOfUnits to whatever the buttons are that the player has selected on the screen
        public void SetUnitsFromButtons()
        {
            userSelectedNames = manager.Menu.GetButtonNames();
            for (int i = 0; i < userSelectedNames.Count; i++)
            {
                // Go through and find the actual unit object with the same name as the button
                for (int j = 0; j < manager.AllUnits.ListCount; j++)
                {
                    if (userSelectedNames[i] == manager.AllUnits.UnitList[j].Name)
                    {
                        manager.Player1Units.Add(manager.AllUnits.UnitList[j]);
                    }
                }
            }
        }*/
    }
}
