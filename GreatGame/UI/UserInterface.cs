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
        private Vector2 textpos;
        private Vector2 textpos2;
        private Rectangle blRect;
        private Rectangle brRect;
        private Rectangle iconRect;
        private Unit selected;

        public Texture2D BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }
        public Texture2D BottomRight { get { return bottomRight; } set { bottomRight = value; } }
        public Rectangle BLRect { get { return blRect; } set { blRect = value; } }
        public Rectangle BRRect { get { return brRect; } set { brRect = value; } }
        public Rectangle IconRect { get { return iconRect; } set { iconRect = value; } }
        public Unit Selected { get { return selected; } set { selected = value; } }

        public UserInterface(GraphicsDevice gd)
        {
            blRect = new Rectangle(0, gd.Viewport.Height - 150, 150, 150);
            brRect = new Rectangle(gd.Viewport.Width - 375, gd.Viewport.Height - 150, 375, 150);
            iconRect = new Rectangle(0, gd.Viewport.Height - 150, 100, 100);
            textpos = new Vector2(brRect.Location.X + 60, brRect.Location.Y + 50);
            textpos2 = new Vector2(brRect.Location.X + 200, brRect.Location.Y + 50);
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(bottomLeft, blRect, Color.White);
            sb.Draw(bottomRight, brRect, Color.White);
            if(selected != null)
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
