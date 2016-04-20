using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class MenuButton
    {
        private Rectangle location;
        private Texture2D texture;
        private SpriteFont font;
        private String name;
        private Color shade;
        private bool enabled;


        public Rectangle Location { get { return location; } set { location = value; } }
        public int X { get { return location.X; } set { location.X = value; } }
        public int Y { get { return location.Y; } set { location.Y = value; } }
        public int Width { get { return location.Width; } set { location.Width = value; } }
        public int Height { get { return location.Height; } set { location.Height = value; } }

        public Texture2D Texture { get { return texture; } set { texture = value; } }

        public SpriteFont Font { get { return font; } set { font = value; } }

        public String Name { get { return name; } set { name = value; } }

        public Color Shade { get { return shade; } set { shade = value; } }

        public bool Enabled { get { return enabled; } set { enabled = value; } }

        public MenuButton(Rectangle loc, Texture2D t, String n, Color s, SpriteFont sf)
        {
            location = loc;
            texture = t;
            name = n;
            shade = s;
            font = sf;
            enabled = true;
        }

        public MenuButton(Rectangle loc, Texture2D t, String n, Color s, SpriteFont sf, bool e)
        {
            location = loc;
            texture = t;
            name = n;
            shade = s;
            font = sf;
            enabled = e;
        }

        public virtual bool CheckClicked(MouseState ms)
        {
            if (!enabled)
                return false;

            if (ms.X >= this.X && ms.Y >= this.Y && ms.X <= this.X + this.Width && ms.Y <= this.Y + this.Height && ms.LeftButton == ButtonState.Pressed)
                return true;

            return false;
        }

        public virtual void CheckHover(MouseState ms)
        {
            if (ms.X >= this.X && ms.Y >= this.Y && ms.X <= this.X + this.Width && ms.Y <= this.Y + this.Height)
                shade = Color.Cyan;
            else shade = Color.White;

        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (!enabled)
                return;

            if(texture != null)
                sb.Draw(texture, location, shade);
            
            if(font != null)
                sb.DrawString(font, name, new Vector2(this.X + 5, this.Y + 5), Color.Black);

        }
    }
}
