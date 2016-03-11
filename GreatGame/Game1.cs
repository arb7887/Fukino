﻿using Microsoft.Xna.Framework;
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
        Vector2 destination;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

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
            listOfUnits = new FileInput<Unit>("Content/Units.txt");
            //listOfUnits.LoadUnit();

            test = new Unit("Test", 10, 10, 10, 10);

            test.Position = new Vector2(0, 0);
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
                    if (previousMouse.RightButton == ButtonState.Pressed && currentMouse.RightButton == ButtonState.Released)
                    {
                        destination = new Vector2(previousMouse.X, previousMouse.Y);
                        test.ProcessInput(destination);
                        test.IsMoving = true;
                    }
                    else if (test.IsMoving)
                    {
                        test.ProcessInput(destination);
                    }
                    break;
                case GameStates.GameOver:
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
                    spriteBatch.Draw(test.Texture, new Rectangle((int)test.position.X, (int)test.position.Y, 50, 50), Color.White);
                    break;
                case GameStates.GameOver:
                    // Print out some info about the score and stuff
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
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
