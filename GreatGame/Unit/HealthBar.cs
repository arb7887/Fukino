using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class HealthBar
    {
        //fields
        private double health;
        private Rectangle red, green;
        private Texture2D texture;

        //properties
        public double Health { get { return health; } set { health = value; } }
        public Rectangle Red { get { return red; } set { red = value; } }
        public Rectangle Green { get { return green; } set { green = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        //constructor
        public HealthBar(Vector2 position)
        {
            red = new Rectangle((int)position.X, (int)position.Y, 128, 64);
            green = new Rectangle((int)position.X, (int)position.Y, 128, 64);
        }
        public void Update(double h)
        {
            health = h;
        }
        public void Draw(SpriteBatch sb)
        {
            // sb.Draw(texture, red, Color.Red);
            // sb.Draw(texture, green, Color.Green);
        }
    }
}
