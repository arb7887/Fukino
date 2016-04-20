using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace MapMaker
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Rectangle> walls;
        Texture2D wallTexture;
        bool drawing;

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
            // TODO: Add your initialization logic here
            walls = new List<Rectangle>();

            drawing = false;
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

            wallTexture = Content.Load<Texture2D>("wallTexture.jpg");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter("walls.txt");
                writer.WriteLine(walls.Count);
                foreach (Rectangle r in walls)
                {
                    writer.WriteLine("----");
                    writer.WriteLine(r.X);
                    writer.WriteLine(r.Y);
                    writer.WriteLine(r.X + r.Width);
                    writer.WriteLine(r.Y + r.Height);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouse = currMouse;
            currMouse = Mouse.GetState();
            
            if(currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && !drawing)
            {
                drawing = true;
                int mouseX = currMouse.Position.X+5;
                int mouseY = currMouse.Position.Y+5;
                mouseX = 10 * (mouseX / 10);
                mouseY = 10 * (mouseY / 10);
                startPoint = new Point(mouseX, mouseY);
            }else if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && drawing)
            {
                drawing = false;
                int mouseX = currMouse.Position.X+5;
                int mouseY = currMouse.Position.Y+5;
                mouseX = 10 * (mouseX / 10);
                mouseY = 10 * (mouseY / 10);
                endPoint = new Point(mouseX, mouseY);
                walls.Add(new Rectangle(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
            }else if (currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                if (drawing)
                    drawing = false;
                else
                {
                    foreach(Rectangle r in walls)
                    {
                        if (r.Intersects(new Rectangle(currMouse.X, currMouse.Y, 1, 1)))
                        {
                            walls.Remove(r);
                            break;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }
        MouseState currMouse;
        MouseState prevMouse;
        Point startPoint;
        Point endPoint;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if (drawing)
            {
                spriteBatch.Draw(wallTexture, new Rectangle(startPoint.X, startPoint.Y, 10 * ((currMouse.X - startPoint.X+5)/10), 
                    10 * ((currMouse.Y - startPoint.Y+5)/10)), Color.White);
            }

            foreach(Rectangle r in walls)
            {
                spriteBatch.Draw(wallTexture, r, Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
