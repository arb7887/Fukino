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

        private enum GameStates
        {
            Menu,
            Select,
            Game,
            GameOver
        }

        private GameStates currentState;
        MouseState currentMouse;
        MouseState previousMouse;
        Unit test;
        Point destination;

        Texture2D buttonTexture;
        MenuButton exit;
        MenuButton options;
        MenuButton play;
        MouseState ms;
        Texture2D pointerTexture;
        SpriteFont buttonFont;

        List<ClassSelectButton> classSelectors;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
           // graphics.ToggleFullScreen();

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
            
            // Load in the Units.txt file, this works now
            listOfUnits = new FileInput<Unit>("Content/Units.txt");
            listOfUnits.LoadUnit();
            currentState = GameStates.Menu;

            exit = new MenuButton(new Rectangle(10, 10, 100, 50), null, "Exit", Color.White, null);
            options = new MenuButton(new Rectangle(10, 60, 100, 50), null, "Options", Color.White, null);
            play = new MenuButton(new Rectangle(10, 110, 100, 50),null, "Play", Color.White, null);
            classSelectors = new List<ClassSelectButton>();

            test = new Unit("Test", 10, 10, 10, 10);

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
            pointerTexture = Content.Load<Texture2D>("Mouse_pointer_small.png");
            buttonFont = Content.Load<SpriteFont>("buttonFont");

            for(int i = 0; i < 6; i++)
            {
                classSelectors.Add(new ClassSelectButton(new Rectangle(50 + (100* i), GraphicsDevice.Viewport.Height - 60, 100, 50), buttonTexture, "Select", Color.White, buttonFont));
            }

            exit.Texture = buttonTexture;
            play.Texture = buttonTexture;
            options.Texture = buttonTexture;
            exit.Font = buttonFont;
            play.Font = buttonFont;
            options.Font = buttonFont;
            

            // Load in the list of units from the file here
            //listOfUnits.LoadUnit();
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

            ms = Mouse.GetState();
            
            switch (currentState)
            {
                case GameStates.Menu:
                    if (exit.CheckClicked(ms))
                        Exit();
                    if (options.CheckClicked(ms))
                        options.Shade = Color.Blue;
                    if (play.CheckClicked(ms))
                    {
                        currentState = GameStates.Select;
                        play.X = 650;
                        play.Y = GraphicsDevice.Viewport.Height - 60;
                    }
                        
                    break;
                case GameStates.Game:
                    previousMouse = currentMouse;
                    currentMouse = Mouse.GetState();
                    /*if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                    {
                        if ((previousMouse.X >= test.Size.X) && previousMouse.X <= (test.Size.X + 10) && previousMouse.Y >= test.Size.Y && previousMouse.Y <= (previousMouse.Y + 10))
                        {
                            test.IsSelected = true;
                        }
                        else
                        {
                            test.IsSelected = false;
                        }
                    }*/
                    if (previousMouse.RightButton == ButtonState.Pressed && currentMouse.RightButton == ButtonState.Released)
                    {
                        destination = new Point(previousMouse.X, previousMouse.Y);
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
                case GameStates.Select:
                    for (int i = 0; i < classSelectors.Count; i++)
                        classSelectors[i].CheckClicked(ms);
                    bool selected = true; ;
                    for (int i = 0; i < classSelectors.Count; i++)
                        if (classSelectors[i].Name == "Select")
                            selected = false;
                    if (selected)
                        play.Enabled = true;
                    else
                        play.Enabled = false; 
                    if (play.CheckClicked(ms))
                        currentState = GameStates.Game;
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
                    exit.Draw(spriteBatch);
                    play.Draw(spriteBatch);
                    options.Draw(spriteBatch);
                    break;
                case GameStates.Game:
                    spriteBatch.Draw(test.Texture, test.Position, Color.White);
                    break;
                case GameStates.GameOver:
                    break;
                case GameStates.Select:
                    for (int i = 0; i < classSelectors.Count; i++)
                        classSelectors[i].Draw(spriteBatch);
                    play.Draw(spriteBatch);
                    break;
            }


            spriteBatch.Draw(pointerTexture, new Rectangle(ms.X, ms.Y, pointerTexture.Width, pointerTexture.Height), Color.White);

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
