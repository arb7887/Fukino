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

        private FileInput<Unit> listOfUnits;    // The list of units
        private List<Unit> userSelectedUnits;   // The list of units that the user has selected

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
        List<Unit> unitList;
        Vector2 destination;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

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
            currentState = GameStates.Game;

            menu = new MenuHandler(MenuStates.Main);
            menu.initialize();
            // Load in the Units.txt file, this works now
            listOfUnits = new FileInput<Unit>("Content/Units.txt");
            userSelectedUnits = new List<Unit>();
            //listOfUnits.LoadUnit();
            unitList = new List<Unit>();
            test = new Unit("Test", 10, 10, 10, 10);
            test.size = 50;
            unitList.Add(test);
            test2 = new Unit("Test2", 15, 15, 15, 15);
            test2.size = 50;
            unitList.Add(test2);

            test.Position = new Vector2(50, 50);
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
            // Load in the list of units from the file here
            listOfUnits.LoadUnit();
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
                        currentState = GameStates.Game;
                    break;
                case GameStates.Game:                    
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                        {
                            if ((previousMouse.X >= unitList[i].Position.X) && previousMouse.X <= (unitList[i].Position.X + unitList[i].size)
                            && previousMouse.Y >= unitList[i].Position.Y && previousMouse.Y <= (unitList[i].Position.Y + unitList[i].size))
                            {
                                unitList[i].IsSelected = true;
                                unitList[i].color = Color.Cyan;
                                userSelectedUnits.Add(unitList[i]);
                            }
                            else
                            {
                                unitList[i].IsSelected = false;
                                unitList[i].color = Color.White;
                                userSelectedUnits.Remove(unitList[i]);
                            }
                        }                            
                        if (unitList[i].IsSelected && (previousMouse.RightButton == ButtonState.Pressed && currentMouse.RightButton == ButtonState.Released))
                        {
                            destination = new Vector2(previousMouse.X, previousMouse.Y);
                            unitList[i].ProcessInput(destination);
                            unitList[i].IsMoving = true;
                        }
                        else if (unitList[i].IsMoving)
                        {
                            unitList[i].ProcessInput(destination);
                        }
                    }
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
                    if (listOfUnits.UnitList.Count > 0)
                    {
                        spriteBatch.DrawString(font, listOfUnits.UnitList[0].Name, Vector2.Zero, Color.Black);
                    }
                    menu.Draw(spriteBatch);
                    break;
                case GameStates.Game:
                    //GraphicsDevice.Clear(Color.Green);
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        spriteBatch.Draw(unitList[i].Texture, new Rectangle((int)unitList[i].position.X, (int)unitList[i].position.Y, unitList[i].size, unitList[i].size), unitList[i].UnitColor);
                    }
                    break;
                case GameStates.GameOver:
                    // Print out some info about the score and stuff
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>

}
