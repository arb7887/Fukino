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
        public UserInterface()
        {
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.DrawString(font, "HELLOOO UI", Vector2.Zero, Color.Red);
        }
    }
}
