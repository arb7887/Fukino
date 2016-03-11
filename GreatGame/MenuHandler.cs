using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GreatGame
{ 
    enum MenuStates { Main, Options, Help, Select }

    class MenuHandler
    {
        private MenuStates currentState;
        private int numUnits;

        Texture2D buttonTexture;
        MenuButton exit;
        MenuButton options;
        MenuButton play;
        SpriteFont buttonFont;

        private List<ClassSelectButton> classSelectors;

        private bool exitGame;
        public bool ExitGame { get { return exitGame; } }
        private bool startGame;
        public bool StartGame { get { return startGame; } }
        public int NumUnits { get { return this.numUnits; } }
        public List<ClassSelectButton> ClassSelectors { get { return this.classSelectors; } }

        public MenuHandler(MenuStates startState)
        {
            currentState = startState;
        }

        public void initialize()
        {
            exit = new MenuButton(new Rectangle(10, 10, 100, 50), null, "Exit", Color.White, null);
            options = new MenuButton(new Rectangle(10, 60, 100, 50), null, "Options", Color.White, null);
            play = new MenuButton(new Rectangle(10, 110, 100, 50), null, "Play", Color.White, null);
            classSelectors = new List<ClassSelectButton>();
            exitGame = false;
            numUnits = 6;
        }

        public void LoadContent(Texture2D bt, SpriteFont bf, GraphicsDevice gd)
        {
            buttonFont = bf;
            buttonTexture = bt;
            // Six is the number of units that we want the player to be able to control
            for (int i = 0; i < numUnits; i++)
            {
                classSelectors.Add(new ClassSelectButton(new Rectangle(50 + (100 * i), gd.Viewport.Height - 60, 100, 50), buttonTexture, "Select", Color.White, buttonFont));
            }

            exit.Texture = buttonTexture;
            play.Texture = buttonTexture;
            options.Texture = buttonTexture;
            exit.Font = buttonFont;
            play.Font = buttonFont;
            options.Font = buttonFont;
        }

        public void Update(MouseState ms, GraphicsDevice gd)
        {
            switch (currentState)
            {
                case MenuStates.Main:
                    if (exit.CheckClicked(ms))
                        exitGame = true;
                    if (options.CheckClicked(ms))
                        options.Shade = Color.Blue;
                    if (play.CheckClicked(ms))
                    {
                        currentState = MenuStates.Select;
                        play.X = 650;
                        play.Y = gd.Viewport.Height - 60;
                    }

                    break;
                case MenuStates.Options:
                    break;
                case MenuStates.Help:
                    break;
                case MenuStates.Select:
                    for (int i = 0; i < classSelectors.Count; i++)
                        classSelectors[i].CheckClicked(ms);

                    bool selected = true;

                    for (int i = 0; i < classSelectors.Count; i++)
                        if (classSelectors[i].Name == "Select")
                            selected = false;

                    if (selected)
                        play.Enabled = true;
                    else
                        play.Enabled = false;
                    if (play.CheckClicked(ms))
                        startGame = true;
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            switch (currentState)
            {
                case MenuStates.Main:
                    exit.Draw(sb);
                    play.Draw(sb);
                    options.Draw(sb);
                    break;
                case MenuStates.Options:
                    break;
                case MenuStates.Help:
                    break;
                case MenuStates.Select:

                    for (int i = 0; i < classSelectors.Count; i++)
                        classSelectors[i].Draw(sb);
                    play.Draw(sb);
                    break;
            }
        }

        public List<String> GetButtonNames()
        {
            List<String> names = new List<String>();
            for(int i = 0; i < numUnits; i++)
            {
                // Get the name of all the class selector buttons, which should be the names of the units
                // And add that to this lsit of strings
                names.Add(classSelectors[i].Name);
            }
            return names;
        }
    }
}
