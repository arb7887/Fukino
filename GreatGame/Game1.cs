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
<<<<<<< HEAD
        Unit test;
        Unit test2;
        List<Unit> unitList;
        Vector2 destination;
=======
>>>>>>> origin/master

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
<<<<<<< HEAD
=======

            // Make the screen bigger
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            // Make full screen when we get to the point of that

>>>>>>> origin/master
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
<<<<<<< HEAD
            currentState = GameStates.Game;
=======
            manager.Menu.initialize();
>>>>>>> origin/master

            // Load in the Units.txt file, this works now
            userSelectedUnits = new List<Unit>();
<<<<<<< HEAD
            //listOfUnits.LoadUnit();
            unitList = new List<Unit>();
            test = new Unit("Test", 10, 10, 100, 10);
            test.size = 50;
            unitList.Add(test);
            test2 = new Unit("Test2", 15, 15, 50, 15);
            test2.size = 50;
            unitList.Add(test2);

            test.Position = new Vector2(50, 50);
            test2.Position = new Vector2(200, 200);
=======
>>>>>>> origin/master

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

<<<<<<< HEAD
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
                        for (int j = 0; j < unitList.Count; j++)
                        {
                            if (i != j)
                            {
                                unitList[i].AttackUnit(unitList[j]);
                            }
                        }
                    }
                    break;
                case GameStates.GameOver:
                    // Check for if the user has hit enter to return to title screen
                    break;
            }
            
=======
            // Call the managers update method
            manager.Update(gameTime, previousMouse, currentMouse, userSelectedUnits, GraphicsDevice);

>>>>>>> origin/master
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

<<<<<<< HEAD
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
=======
            // Call the managers Draw method
            manager.Draw(spriteBatch, font);
            
>>>>>>> origin/master
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
<<<<<<< HEAD

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>

=======
>>>>>>> origin/master
}
