using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Enemy : Unit
    {
        private Map map;
        private float accuracy;


        public Enemy(Unit u, int i, Map map)
            :base(u, i)
        {
            this.map = map;
        }
        

        public void Update(GameTime gameTime)
        {
            // Call the pathfinder method
        }

        public void Draw()
        {

        }

        /// <summary>
        /// Use this method in order to find out where the unit needs to move, and then calculate the distance
        /// </summary>
        /// <returns></returns>
        public Vector2 Pathfinder()
        {
            return Vector2.Zero;
        }
    }
}
