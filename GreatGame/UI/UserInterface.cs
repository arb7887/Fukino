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
        #region Fields
        // textures:
        private Texture2D bottomLeft;
        private Texture2D bottomRight;
        private Texture2D icon;
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
            sb.Draw(bottomLeft, Rect1, Color.White);
            sb.Draw(bottomLeft, Rect2, Color.White);
            sb.Draw(bottomLeft, Rect3, Color.White);
            sb.Draw(bottomLeft, Rect4, Color.White);
            sb.Draw(bottomLeft, Rect5, Color.White);
            sb.Draw(bottomLeft, Rect6, Color.White);
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
