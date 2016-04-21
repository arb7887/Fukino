using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class PauseMenu
    {
        private MenuButton resume, options, mainmenu;

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
        public MenuButton MainMenu
        {
            get { return mainmenu; }
            set { mainmenu = value; }
        }
        #endregion
        public PauseMenu()
        {
            
        }

        public void Initialize()
        {
            resume = new MenuButton(new Rectangle(590, 285, 100, 50), null, "Resume", Color.White, null);
            options = new MenuButton(new Rectangle(590, 335, 100, 50), null, "Options", Color.White, null);
            mainmenu = new MenuButton(new Rectangle(590, 385, 100, 50), null, "Main Menu", Color.White, null);
        }
        public void LoadContent(Texture2D bt, SpriteFont bf)
        {
            mainmenu.Texture = bt;
            resume.Texture = bt;
            options.Texture = bt;
            mainmenu.Font = bf;
            resume.Font = bf;
            options.Font = bf;
        }
        public void Draw(SpriteBatch sb)
        {
            resume.Draw(sb);
            options.Draw(sb);
            mainmenu.Draw(sb);
        }
        public void Update(MouseState ms)
        {
            mainmenu.CheckHover(ms);
            options.CheckHover(ms);
            resume.CheckHover(ms);
            if (options.CheckClicked(ms))
                options.Shade = Color.Blue;
        }
    }
}
