using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame.UI
{
    class PauseMenu
    {
        private MenuButton resume, options, exit;

        #region Properties
        public MenuButton Resume
        {
            get { return resume; }
            set { resume = value; }
        }
        public MenuButton Options
        {
            get { return options; }
            set { options = value; }
        }
        public MenuButton Exit
        {
            get { return exit; }
            set { exit = value; }
        }
        #endregion
        public PauseMenu()
        {
            resume = new MenuButton(new Rectangle(590, 285, 100, 50), null, "Resume", Color.White, null);
            options = new MenuButton(new Rectangle(590, 335, 100, 50), null, "Options", Color.White, null);
            exit = new MenuButton(new Rectangle(590, 385, 100, 50), null, "Exit", Color.White, null);
        }

        public void Draw(SpriteBatch sb)
        {
            resume.Draw(sb);
            options.Draw(sb);
            exit.Draw(sb);
        }
        public void Update(MouseState ms)
        {
            exit.CheckHover(ms);
            options.CheckHover(ms);
            resume.CheckHover(ms);
            if (options.CheckClicked(ms))
                options.Shade = Color.Blue;
        }
    }
}
