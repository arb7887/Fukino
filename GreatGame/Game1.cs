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
            //listOfUnits = new FileInput<Unit>("Units.txt");
            currentState = GameStates.Menu;

            exit = new MenuButton(new Rectangle(10, 10, 100, 50), null, "Exit", Color.White, null);
            options = new MenuButton(new Rectangle(10, 60, 100, 50), null, "Options", Color.White, null);
            play = new MenuButton(new Rectangle(10, 110, 100, 50),null, "Play", Color.White, null);
            classSelectors = new List<ClassSelectButton>();

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
                classSelectors.Add(new ClassSelectButton(new Rectangle(100 + (110* i), GraphicsDevice.Viewport.Height - 60, 100, 50), buttonTexture, "Select", Color.White, buttonFont));
            }
            exit.Texture = buttonTexture;
            play.Texture = buttonTexture;
            options.Texture = buttonTexture;
            exit.Font = buttonFont;
            play.Font = buttonFont;
            options.Font = buttonFont;
            

            // Load in the list of units from the file here
            //listOfUnits.LoadUnit();
            
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
                        play.X = 760;
                        play.Y = GraphicsDevice.Viewport.Height - 60;
                    }
                        
                    break;
                case GameStates.Game:
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
}
