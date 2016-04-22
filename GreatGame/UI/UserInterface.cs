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
        private List<Unit> selected;

        public Texture2D BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }
        public Texture2D BottomRight { get { return bottomRight; } set { bottomRight = value; } }
        public Rectangle BLRect { get { return blRect; } set { blRect = value; } }
        public Rectangle BRRect { get { return brRect; } set { brRect = value; } }
        public Rectangle IconRect { get { return iconRect; } set { iconRect = value; } }

        public UserInterface(GraphicsDevice gd, List<Unit> selected)
        {
            blRect = new Rectangle(0, gd.Viewport.Height - 150, 150, 150);
            brRect = new Rectangle(gd.Viewport.Width - 375, gd.Viewport.Height - 150, 375, 150);
            iconRect = new Rectangle(0, gd.Viewport.Height - 150, 100, 100);
            textpos = new Vector2(brRect.Location.X + 60, brRect.Location.Y + 50);
            textpos2 = new Vector2(brRect.Location.X + 200, brRect.Location.Y + 50);
            this.selected = selected;
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(bottomLeft, blRect, Color.White);
            sb.Draw(bottomRight, brRect, Color.White);
            if(selected.Count != 0)
            {
                if(selected[0].Icon != null)
                {
                    sb.Draw(selected[0].Icon, iconRect, Color.White);
                }
                sb.DrawString(font,
                    selected[0].Name + 
                    ": \nHealth: " + selected[0].Health + 
                    "\nSpeed: " + selected[0].Speed + 
                    "\nRange: " + selected[0].AttackRange + 
                    "\nDamage: " + selected[0].ATTACK_STRENGTH, 
                    textpos,
                    Color.White);
                sb.DrawString(font,
                    "\nRate of Fire: " + selected[0].RateOfFire,
                    textpos2,
                    Color.White);
            }
        }
    }
}
