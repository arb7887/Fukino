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
        private double maxhealth;
        private Rectangle red, green;
        private Texture2D texture;
        private Vector2 textpos;

        //properties
        public double Health { get { return health; } set { health = value; } }
        public double MaxHealth { get { return maxhealth; } set { maxhealth = value; } }
        public Rectangle Red { get { return red; } set { red = value; } }
        public Rectangle Green { get { return green; } set { green = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        //constructor
        public HealthBar(Vector2 position)
        {
            red = new Rectangle((int)position.X - 64, (int)position.Y - 64, 64, 8);
            green = new Rectangle((int)position.X - 64, (int)position.Y - 64, 64, 8);
            textpos = new Vector2((red.Width / 2 - 20), (red.Height / 2 - 20));
        }
        public void Update(double h, Vector2 position)
        {
            //change health to percentage
            health = h;
            double percentage = health / maxhealth;
            red.X = (int)position.X - 32;
            red.Y = (int)position.Y - 32;
            green.Width = (int)(64.0 * percentage);
            green.X = (int)position.X - 32;
            green.Y = (int)position.Y - 32;
            textpos.Y = red.Y + (red.Height / 2 - 20);
            textpos.X = red.X + (red.Width / 2 - 20);
        }
        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.Draw(texture, red, Color.Red);
            sb.Draw(texture, green, Color.Green);
            // sb.DrawString(font, "" + health, textpos, Color.White);
        }
        public void Draw(SpriteBatch sb, SpriteFont font, Vector2 position)
        {
            red.X = (int)position.X;
            red.Y = (int)position.Y;
            green.X = (int)position.X;
            green.Y = (int)position.Y;
            sb.Draw(texture, red, Color.Red);
            sb.Draw(texture, green, Color.Green);
            // sb.DrawString(font, "" + health, textpos, Color.White);
        }
    }
}
