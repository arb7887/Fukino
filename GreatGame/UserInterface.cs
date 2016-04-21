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
        private Rectangle blRect;
        private Rectangle brRect;

        public Texture2D BottomLeft { get { return bottomLeft; } set { bottomLeft = value; } }
        public Texture2D BottomRight { get { return bottomRight; } set { bottomRight = value; } }
        public Rectangle BLRect { get { return blRect; } set { blRect = value; } }
        public Rectangle BRRect { get { return brRect; } set { brRect = value; } }

        public UserInterface(GraphicsDevice gd)
        {
            blRect = new Rectangle(0, gd.Viewport.Height - 150, 150, 150);
            brRect = new Rectangle(gd.Viewport.Width - 375, gd.Viewport.Height - 150, 375, 150);
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(bottomLeft, blRect, Color.White);
            sb.Draw(bottomRight, brRect, Color.White);
        }
    }
}
