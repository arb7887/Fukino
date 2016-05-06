using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Grid
    {
        #region Fields
        private PriorityQueue _OPEN;    // A min - priority queue
        private List<Vertex> _CLOSED, _ALL_VERTECIES;
        private Vertex _currentVertex;  // This is the currently selected vertex
        private Texture2D _pixel;       // This is the texture that I will use to draw the grid
        private int gridWidth, gridHeight;
        #endregion


        #region Properties
        public PriorityQueue OPEN { get { return this._OPEN; } set { this._OPEN = value; } }
        public List<Vertex> CLOSED { get { return this._CLOSED; } set { this._CLOSED = value; } }
        public Vertex CurrentVertex { get { return this._currentVertex; } set { this._currentVertex = value; } }
        public List<Vertex> ALL_VERTECIES { get { return this._ALL_VERTECIES; } }

        #endregion


        #region Constructor
        public Grid(Map map, Point location, int blockSize, Texture2D pixelTexture)
        {
            //_pixel = pixelTexture;
            _pixel = null;
            // Create and fill a simple 1x1 texture
            _OPEN = new PriorityQueue();
            _CLOSED = new List<Vertex>();
            _ALL_VERTECIES = new List<Vertex>();

            // Get the size of the map 
            GetMapSize(map);

            // Initialized this grid
            Initialize(location, GetMapSize(map).X, GetMapSize(map).Y, blockSize);
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method will be used to make all the new vertex objects
        /// that we need for drawing the graph
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Initialize(Point startLocation, int gridWidth, int gridHeight, int blockSize)
        {
            // Loop throgh and make the number of rows
            for (int x = startLocation.X; x < gridWidth + startLocation.X; x += blockSize)
            {
                // For each row, make a column
                for (int y = startLocation.Y; y < gridHeight + startLocation.Y; y += blockSize)
                {
                    // Make a new Vertex and add it to OPEN
                    // Use blocksize - 1 in order to draw a little bit of space in between the blocks
                    Vertex newVert = new Vertex(new Point(x, y), new Point(blockSize - 1));
                    _ALL_VERTECIES.Add(newVert);
                }
            }

            // Pick a current vertex
            if (_ALL_VERTECIES.Capacity > 0)
                _currentVertex = _ALL_VERTECIES[0];

            // Change th color of the starting vertex
            _currentVertex.VertColor = Color.Green;
            // The distance is 0
            _currentVertex.Distance = 0;

            // Add the current vertex to the OPEN priority queue
            OPEN.Enqueue(_currentVertex);
        }

        public Point GetMapSize(Map m)
        {
            Point mapSize = new Point(0, 0);

            foreach(Wall w in m.Walls)
            {
                if(w.Bounds.Max.X >= mapSize.X)
                {
                    mapSize.X = (int)w.Bounds.Max.X;
                }
                if(w.Bounds.Max.Y >= mapSize.Y)
                {
                    mapSize.Y = (int)w.Bounds.Max.Y;
                }

            }


            return mapSize;
        }

        /// <summary>
        /// This is where A* will be implemented
        /// </summary>
        /// <param name="GOAL">This is the vertex that we start at</param>
        public void ShortestPath(Vertex GOAL)
        {
            // Set this so taht we can traverse later
            Vertex startingPoint = _currentVertex;

            GOAL.VertColor = Color.Red;
            // While the peek of open is not the goal
            while (_OPEN.Peek() != GOAL)
            {
                // Take the current node out of open
                // Current should be the thing on top of the priority queue
                _currentVertex = _OPEN.DequeueTwo();
                _CLOSED.Add(_currentVertex);

                // Find all of the neighbors of the current vertex
                List<Vertex> currentNeighbors = GetNieghbors(_currentVertex);

                foreach (Vertex neighbor in currentNeighbors)
                {
                    // Get the cost, which is G(current)
                    int cost = _currentVertex.Distance + GetCost(_currentVertex, neighbor);

                    // If neighbor is in OPEN and cost less then neighbor.distance
                    // Neighbor.Distance is G(neighbor)
                    if (_OPEN.heap.Contains(neighbor) && cost < neighbor.Distance)
                    {
                        // Remove neighbor from OPEN, because the new path is better
                        _OPEN.heap.Remove(neighbor);
                        _currentVertex = OPEN.DequeueTwo();
                    }
                    // If neighbor is in CLOSED and cost is less then g(neighbor)
                    if (_CLOSED.Contains(neighbor) && cost < neighbor.Distance)
                    {
                        // Remove neighbor from CLOSED
                        _CLOSED.Remove(neighbor);
                    }
                    // If neighbor is not in open and neighbor is not in CLOSED:
                    if (!_OPEN.heap.Contains(neighbor) && !_CLOSED.Contains(neighbor))
                    {
                        neighbor.Distance = cost;
                        _OPEN.Enqueue(neighbor);

                        neighbor.NeighboringVertex = _currentVertex;
                    }
                }
            }

            // Reconstruct the reverse path from goal to start
            ColorPath(startingPoint, GOAL);
        }

        /// <summary>
        /// This method will be called if you want to do this as the 
        /// game plays, not all at once
        /// </summary>
        /// <param name="GOAL"></param>
        public void ShortestPathSlow(Vertex GOAL)
        {

            if (_currentVertex == GOAL)
            {
                return;
            }

            // Set this so taht we can traverse later
            Vertex startingPoint = _currentVertex;

            GOAL.VertColor = Color.Red;

            // Take the current node out of open
            // Current should be the thing on top of the priority queue
            _currentVertex = _OPEN.DequeueTwo();
            _CLOSED.Add(_currentVertex);

            // Find all of the neighbors of the current vertex
            List<Vertex> currentNeighbors = GetNieghbors(_currentVertex);

            foreach (Vertex neighbor in currentNeighbors)
            {
                // Get the cost, which is G(current)
                int cost = _currentVertex.Distance + GetCost(_currentVertex, neighbor);

                // If neighbor is in OPEN and cost less then neighbor.distance
                // Neighbor.Distance is G(neighbor)
                if (_OPEN.heap.Contains(neighbor) && cost < neighbor.Distance)
                {
                    // Remove neighbor from OPEN, because the new path is better
                    _OPEN.heap.Remove(neighbor);
                    _currentVertex = OPEN.DequeueTwo();
                }
                // If neighbor is in CLOSED and cost is less then g(neighbor)
                if (_CLOSED.Contains(neighbor) && cost < neighbor.Distance)
                {
                    // Remove neighbor from CLOSED
                    _CLOSED.Remove(neighbor);
                }
                // If neighbor is not in open and neighbor is not in CLOSED:
                if (!_OPEN.heap.Contains(neighbor) && !_CLOSED.Contains(neighbor))
                {
                    neighbor.Distance = cost;
                    _OPEN.Enqueue(neighbor);

                    neighbor.NeighboringVertex = _currentVertex;
                }
            }
        }

        /// <summary>
        /// This method checks if any vertex to the left, right, top or bottom
        /// of the given vertex, v, intersect with it. If they do, then add them to the list 
        /// and return that list when it has traversed all vertecies in the graph
        /// </summary>
        /// <param name="v">The vertex of which I am getting a list of all the neighbors</param>
        /// <returns>A list of vertecies that intersect with the given vertex(v)</returns>
        public List<Vertex> GetNieghbors(Vertex v)
        {
            List<Vertex> neighboringVertecies = new List<Vertex>();
            // Use collision detection to see which rectangles are neighbors
            // Make 4 different rectangles, checking one in each direction
            Rectangle leftCheck = v.RECTANGLE;
            leftCheck.X -= 3;
            Rectangle rightCheck = v.RECTANGLE;
            rightCheck.X += 3;
            Rectangle topCheck = v.RECTANGLE;
            topCheck.Y -= 3;
            Rectangle botCheck = v.RECTANGLE;
            botCheck.Y += 3;

            foreach (Vertex vertex in _ALL_VERTECIES)
            {
                if (vertex != v)
                {
                    if (leftCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (rightCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (topCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (botCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                }

            }

            return neighboringVertecies;
        }

        public void ColorOne(Vertex v)
        {
            if (_CLOSED.Contains(v) && v.VertColor != Color.Green && v.VertColor != Color.Red)
                v.VertColor = Color.Gray;

            if (_OPEN.heap.Contains(v) && v.VertColor != Color.Green && v.VertColor != Color.Red)
                v.VertColor = Color.HotPink;
        }

        /// <summary>
        /// This method will go backwards through the list and
        /// change each color of the tile to something along the 
        /// path
        /// </summary>
        public void ColorPath(Vertex start, Vertex goal)
        {
            Vertex currentPathVertex = goal;

            while (currentPathVertex != start)
            {
                if (currentPathVertex.NeighboringVertex != null && currentPathVertex.NeighboringVertex.VertColor != Color.Green)
                {
                    currentPathVertex = currentPathVertex.NeighboringVertex;
                    currentPathVertex.VertColor = Color.Aqua;
                }
                else
                {
                    return;
                }
            }

        }

        /// <summary>
        /// This is a helper method that I will use to compute the cost
        /// The cost is the absoulte value of the (cur X - next X) + (cur Y - next Y)
        /// </summary>
        /// <returns>Absolute value of the difference between the distances</returns>
        public int GetCost(Vertex current, Vertex next)
        {
            return Math.Abs(current.RECTANGLE.X - next.RECTANGLE.X)
                + Math.Abs(current.RECTANGLE.Y - next.RECTANGLE.Y);
        }

        /// <summary>
        /// This method loops though and calls the draw method
        /// each vertex object in OPEN and CLOSED
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (_CLOSED.Count != 0)
            {
                foreach (Vertex v in _CLOSED)
                {
                    if (v.VertColor == Color.White)
                        v.VertColor = Color.Gray;
                }
            }

            if (!_OPEN.IsEmpty())
            {
                foreach (Vertex v in _OPEN.heap)
                {
                    if (v.VertColor == Color.White)
                        v.VertColor = Color.HotPink;
                }
            }
            // In the draw method I need to go through the list of all the vertecies and 
            // Draw them with their tint color
            foreach (Vertex v in _ALL_VERTECIES)
            {
                v.Draw(sb, _pixel, v.VertColor);
            }
        }



        public void DrawSlow(SpriteBatch sb)
        {
            if (_CLOSED.Count != 0)
            {
                foreach (Vertex v in _CLOSED)
                {
                    if (v.VertColor == Color.White)
                        v.VertColor = Color.Gray;
                }
            }

            if (!_OPEN.IsEmpty())
            {
                foreach (Vertex v in _OPEN.heap)
                {
                    if (v.VertColor == Color.White)
                        v.VertColor = Color.HotPink;
                }
            }
            foreach (Vertex v in _ALL_VERTECIES)
            {
                v.Draw(sb, _pixel, v.VertColor);
            }

            ColorOne(_currentVertex);
            if (_currentVertex.NeighboringVertex != null)
            {
                _currentVertex.NeighboringVertex.VertColor = Color.Aqua;
                ColorOne(_currentVertex.NeighboringVertex);

            }
            //ColorOne(_currentVertex.NeighboringVertex);

            _currentVertex.Draw(sb, _pixel, _currentVertex.VertColor);
        }
        #endregion

    }
}
