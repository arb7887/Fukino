using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatGame
{
    class Map
    {
        private List<Wall> walls;
        public List<Wall> Walls { get { return walls; } }

        public Map()
        {
            walls = new List<Wall>();
        }
    }
}
