using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Wall
    {
        private BoundingBox bounds;
        private Texture2D texture;
        public BoundingBox Bounds { get { return bounds; } }

        public Wall(BoundingBox bound, Texture2D tex)
        {
            bounds = bound;
            texture = tex;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)bounds.Min.X, (int)bounds.Min.Y,
                (int)(bounds.Max.X - bounds.Min.X), (int)(bounds.Max.Y - bounds.Min.Y)), Color.White);
        }

        public bool Colliding(Unit u)
        {
            return u.Bounds.Intersects(bounds);
        }
    }
}
