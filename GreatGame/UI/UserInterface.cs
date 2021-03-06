﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class UserInterface
    {
        #region Fields
        // textures:
        private Texture2D bottomLeft;
        private Texture2D bottomRight;
        private Texture2D timer;
        //locations:
        private Vector2 textpos;
        private Vector2 textpos2;
        private Vector2 timertext1;
        private Vector2 timertext2;
        //Rectangles:
        private Rectangle Rect1;
        private Rectangle Rect2;
        private Rectangle Rect3;
        private Rectangle Rect4;
        private Rectangle Rect5;
        private Rectangle Rect6;
        private Rectangle icon1;
        private Rectangle icon2;
        private Rectangle icon3;
        private Rectangle icon4;
        private Rectangle icon5;
        private Rectangle icon6;
        private Rectangle brRect;
        private Rectangle redRect;
        private Rectangle blueRect;
        private Rectangle iconRect;
        //Other
        private Unit selected;
        private List<Unit> player1units;
        private Map map;
        #endregion

        #region Properties
        public Texture2D BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }
        public Texture2D BottomRight { get { return bottomRight; } set { bottomRight = value; } }
        public Texture2D Timer { get { return timer; } set { timer = value; } }
        public Rectangle BRRect { get { return brRect; } set { brRect = value; } }
        public Rectangle IconRect { get { return iconRect; } set { iconRect = value; } }
        public Map Map { get { return map; } set { map = value; } }
        public Unit Selected { get { return selected; } set { selected = value; } }
        public List<Unit> Player1Units { get { return player1units; } set { player1units = value; } }
        #endregion
        public UserInterface(GraphicsDevice gd)
        {
            Rect1 = new Rectangle(0, (gd.Viewport.Height / 2) - 300, 100, 100);
            Rect2 = new Rectangle(0, (gd.Viewport.Height / 2) - 200, 100, 100);
            Rect3 = new Rectangle(0, (gd.Viewport.Height / 2) - 100, 100, 100);
            Rect4 = new Rectangle(0, (gd.Viewport.Height / 2), 100, 100);
            Rect5 = new Rectangle(0, (gd.Viewport.Height / 2) + 100, 100, 100);
            Rect6 = new Rectangle(0, (gd.Viewport.Height / 2) + 200, 100, 100);
            icon1 = new Rectangle(18, (gd.Viewport.Height / 2) - 270, 40, 40);
            icon2 = new Rectangle(18, (gd.Viewport.Height / 2) - 170, 40, 40);
            icon3 = new Rectangle(18, (gd.Viewport.Height / 2) - 70, 40, 40);
            icon4 = new Rectangle(18, (gd.Viewport.Height / 2) + 30, 40, 40);
            icon5 = new Rectangle(18, (gd.Viewport.Height / 2) + 130, 40, 40);
            icon6 = new Rectangle(18, (gd.Viewport.Height / 2) + 230, 40, 40);
            brRect = new Rectangle(gd.Viewport.Width - 375, gd.Viewport.Height - 150, 375, 150);
            iconRect = new Rectangle(gd.Viewport.Width - 67, gd.Viewport.Height - 127, 40, 40);
            textpos = new Vector2(brRect.Location.X + 60, brRect.Location.Y + 50);
            textpos2 = new Vector2(brRect.Location.X + 200, brRect.Location.Y + 50);
            blueRect = new Rectangle((gd.Viewport.Width / 2) - 128, 0, 128, 64);
            redRect = new Rectangle(gd.Viewport.Width / 2, 0, 128, 64);
            timertext1 = new Vector2(blueRect.Location.X + 50, blueRect.Location.Y + 12);
            timertext2 = new Vector2(redRect.Location.X + 50, redRect.Location.Y + 12);
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(bottomLeft, Rect1, Color.White);
            sb.Draw(bottomLeft, Rect2, Color.White);
            sb.Draw(bottomLeft, Rect3, Color.White);
            sb.Draw(bottomLeft, Rect4, Color.White);
            sb.Draw(bottomLeft, Rect5, Color.White);
            sb.Draw(bottomLeft, Rect6, Color.White);
            player1units[0].Healthbar.Draw(sb, font, new Vector2(Rect1.X + 5, Rect1.Y + 70));
            player1units[1].Healthbar.Draw(sb, font, new Vector2(Rect2.X + 5, Rect2.Y + 70));
            player1units[2].Healthbar.Draw(sb, font, new Vector2(Rect3.X + 5, Rect3.Y + 70));
            player1units[3].Healthbar.Draw(sb, font, new Vector2(Rect4.X + 5, Rect4.Y + 70));
            player1units[4].Healthbar.Draw(sb, font, new Vector2(Rect5.X + 5, Rect5.Y + 70));
            player1units[5].Healthbar.Draw(sb, font, new Vector2(Rect6.X + 5, Rect6.Y + 70));
            sb.Draw(player1units[0].Icon, icon1, Color.White);
            sb.Draw(player1units[1].Icon, icon2, Color.White);
            sb.Draw(player1units[2].Icon, icon3, Color.White);
            sb.Draw(player1units[3].Icon, icon4, Color.White);
            sb.Draw(player1units[4].Icon, icon5, Color.White);
            sb.Draw(player1units[5].Icon, icon6, Color.White);
            sb.Draw(bottomRight, brRect, Color.White);
            sb.Draw(timer, blueRect, Color.CornflowerBlue);
            sb.Draw(timer, redRect, new Color(255, 130, 130));
            sb.DrawString(font, "" + map.CP.PlayerTimer, timertext1, Color.White);
            sb.DrawString(font, "" + map.CP.EnemyTimer, timertext2, Color.White);
            if (selected != null)
            {
                if(selected.Icon != null)
                {
                    sb.Draw(selected.Icon, iconRect, Color.White);
                }
                sb.DrawString(font,
                    selected.Name + 
                    ": \nHealth: " + selected.Health + 
                    "\nSpeed: " + selected.Speed + "0%" +
                    "\nRange: " + selected.AttackRange + "%" +
                    "\nDamage: " + selected.AttackStrength, 
                    textpos,
                    Color.White);
                sb.DrawString(font,
                    "\nRate of Fire: " + selected.RateOfFire,
                    textpos2,
                    Color.White);
            }
        }
    }
}
