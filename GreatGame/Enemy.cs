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

        /// <summary>
        /// This is the constructor that we will use for the "AI" in the game. 
        /// This needs to be able to find the shortest path to the objective. 
        /// Enemy's should shoot on sight when a player unit comes inside of a radius to
        /// this unit
        /// </summary>
        /// <param name="u"></param>
        /// <param name="i"></param>
        public Enemy(Unit u, int i) 
            : base(u, i)
        {

        }

        /// <summary>
        /// This method will be used to find the shortest path to the objective
        /// </summary>
        public void ShortestPath()
        {

        }

    }
}
