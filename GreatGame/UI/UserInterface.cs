using System;
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
        private Texture2D bottomLeft;
        private Texture2D bottomRight;
        private Texture2D icon;
        private Texture2D timer;
        private Vector2 textpos;
        private Vector2 textpos2;
        private Vector2 timertext1;
        private Vector2 timertext2;
        private Rectangle blRect;
        private Rectangle brRect;
        private Rectangle redRect;
        private Rectangle blueRect;
        private Rectangle iconRect;
        private Unit selected;
        private Map map;

        public Texture2D BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }
        public Texture2D BottomRight { get { return bottomRight; } set { bottomRight = value; } }
        public Texture2D Timer { get { return timer; } set { timer = value; } }
        public Rectangle BLRect { get { return blRect; } set { blRect = value; } }
        public Rectangle BRRect { get { return brRect; } set { brRect = value; } }
        public Rectangle IconRect { get { return iconRect; } set { iconRect = value; } }
        public Map Map { get { return map; } set { map = value; } }
        public Unit Selected { get { return selected; } set { selected = value; } }

        public UserInterface(GraphicsDevice gd)
        {
            blRect = new Rectangle(0, gd.Viewport.Height - 150, 150, 150);
            brRect = new Rectangle(gd.Viewport.Width - 375, gd.Viewport.Height - 150, 375, 150);
            iconRect = new Rectangle(0, gd.Viewport.Height - 150, 100, 100);
            textpos = new Vector2(brRect.Location.X + 60, brRect.Location.Y + 50);
            textpos2 = new Vector2(brRect.Location.X + 200, brRect.Location.Y + 50);
            blueRect = new Rectangle((gd.Viewport.Width / 2) - 128, 0, 128, 64);
            redRect = new Rectangle(gd.Viewport.Width / 2, 0, 128, 64);
            timertext1 = new Vector2(blueRect.Location.X + 50, blueRect.Location.Y + 12);
            timertext2 = new Vector2(redRect.Location.X + 50, redRect.Location.Y + 12);
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(bottomLeft, blRect, Color.White);
            sb.Draw(bottomRight, brRect, Color.White);
            sb.Draw(timer, blueRect, Color.CornflowerBlue);
            sb.Draw(timer, redRect, new Color(255, 147, 147));
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
