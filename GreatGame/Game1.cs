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

        private SpriteFont font;

        private FileInput listOfUnits;    // The list of units
        private List<Unit> userSelectedUnits;   // The list of units that the user has selected
        private List<String> userSelectedNames; // This is the lsit of names from the buttons that the user picks on the menu screen

        Texture2D buttonTexture;
        SpriteFont buttonFont;

        MenuHandler menu;

        private enum GameStates
        {
            Menu,
            Game,
            GameOver
        }

        private GameStates currentState;

        MouseState currentMouse;
        MouseState previousMouse;
        Unit test;
        Unit test2;
        Vector2 destination;

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
            // Instantiates the list of units
            currentState = GameStates.Menu;

            menu = new MenuHandler(MenuStates.Main);
            menu.initialize();

            // Load in the Units.txt file, this works now
            listOfUnits = new FileInput("Content/Units.txt");
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
            menu.LoadContent(buttonTexture, buttonFont, GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial14");
            
            // Load in the list of units from the file here
            listOfUnits.LoadUnit();
            
            
            test.Texture = Content.Load<Texture2D>("Kamui");
            test2.Texture = Content.Load<Texture2D>("Kamui");
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
                    menu.Update(currentMouse, GraphicsDevice);
                    if (menu.ExitGame)
                        Exit();
                    if (menu.StartGame)
                    {
                        currentState = GameStates.Game;
                        // Load in the list of class selected units and add them to the list of units in 
                        // This game one class's list called 'userSelectedUnits'
                        SetUnitsFromButtons();
                    }
                        
                    break;
                case GameStates.Game:
                    test.Update(gameTime, previousMouse, currentMouse, userSelectedUnits);
                    test2.Update(gameTime, previousMouse, currentMouse, userSelectedUnits);
                    break;
                case GameStates.GameOver:
                    // Check for if the user has hit enter to return to title screen
                    break;
            }
            
            MouseState mouse = Mouse.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();    

            switch (currentState)
            {
                case GameStates.Menu:
                    menu.Draw(spriteBatch);
                    break;
                case GameStates.Game:
                    //GraphicsDevice.Clear(Color.Green);
                    spriteBatch.Draw(test.Texture, new Rectangle((int)test.position.X, (int)test.position.Y, 50, 50), test.UnitColor);
                    //Second Test Unit:
                    spriteBatch.Draw(test2.Texture, new Rectangle((int)test2.position.X, (int)test2.position.Y, 50, 50), test2.UnitColor);

                    //spriteBatch.DrawString(font, userSelectedUnits[1].health.ToString(), Vector2.Zero, Color.Black);
                    break;
                case GameStates.GameOver:
                    // Print out some info about the score and stuff
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Sets the listOfUnits to whatever the buttons are that the player has selected on the screen
        public void SetUnitsFromButtons()
        {
            userSelectedNames = menu.GetButtonNames();
            for (int i = 0; i < userSelectedNames.Count; i++)
            {
                // Go through and find the actual unit object with the same name as the button
                for (int j = 0; j < listOfUnits.ListCount; j++)
                {
                    if (userSelectedNames[i] == listOfUnits.UnitList[j].Name)
                    {
                        userSelectedUnits.Add(listOfUnits.UnitList[j]);
                    }
                }
            }
        }
    }



    /*protected bool IsMouseOver(Unit u)
    {
        if ((previousMouse.X >= u.Size.X) && previousMouse.X <= (u.Size.X + 10) &&
                previousMouse.Y >= u.Size.Y && previousMouse.Y <= (previousMouse.Y + 10))
        {
            return true;
        }
        return false;
    }*/

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>

}
