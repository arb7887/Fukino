﻿using System;
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
        private Vertex _START, _GOAL;
        private int gridWidth, gridHeight;
        private List<Vertex> _ONES_TO_DRAW;

        #endregion


        #region Properties
        public List<Vertex> ONES_TO_DRAW { get { return _ONES_TO_DRAW; } set { _ONES_TO_DRAW = value; } }

        public PriorityQueue OPEN { get { return this._OPEN; } set { this._OPEN = value; } }
        public List<Vertex> CLOSED { get { return this._CLOSED; } set { this._CLOSED = value; } }
        public Vertex CurrentVertex { get { return this._currentVertex; } set { this._currentVertex = value; } }
        public List<Vertex> ALL_VERTECIES { get { return this._ALL_VERTECIES; } }

        #endregion


        #region Constructor
        public Grid(Point location, int blockSize, Texture2D pixel, List<Wall> walls, Map m)
        {
            // Create and fill a simple 1x1 texture
            _pixel = pixel;

            _OPEN = new PriorityQueue();
            _CLOSED = new List<Vertex>();
            _ALL_VERTECIES = new List<Vertex>();

            gridWidth = GetMapSize(m).X;
            gridHeight = GetMapSize(m).Y;

            Initialize(location, gridWidth, gridHeight, blockSize, walls);
        }
        #endregion


        #region Methods
        /// <summary>
        /// This method will be used to make all the new vertex objects
        /// that we need for drawing the graph
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Initialize(Point startLocation, int gridWidth, int gridHeight, int blockSize, List<Wall> walls)
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


            // Set the walls
            SetWalls(walls);


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


        /// <summary>
        /// Takes in a list of walls, and check to see if the rectangles on 
        /// the grid are intersecting with the wall
        /// </summary>
        /// <param name="walls"></param>
        public void SetWalls(List<Wall> walls)
        {
            // Set the walls
            for (int i = 0; i < walls.Count; i++)
            {
                foreach(Vertex v in _ALL_VERTECIES)
                {
                    if (walls[i].Rectangle.Intersects(v.RECTANGLE))
                    {
                        // Set this vertex as a wall
                        v.Is_Wall = true;
                    }
                }
            }
        }

        /// <summary>
        /// Calls the Reset() method on every vertex in the grid
        /// </summary>
        /// <param name="currentVertex"></param>
        public void ResetAllVertecies(Vertex currentVertex)
        {
            foreach(Vertex v in _ALL_VERTECIES)
            {
                v.Reset();
            }
            OPEN.Clear();
            CLOSED.Clear();

            _currentVertex = currentVertex;
            OPEN.Enqueue(_currentVertex);
        }


        /// <summary>
        /// Takes in a map, and finds out the largest X and Y values of that map
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Point GetMapSize(Map m)
        {
            Point mapSize = new Point(0, 0);

            foreach (Wall w in m.Walls)
            {
                if (w.Bounds.Max.X >= mapSize.X)
                {
                    mapSize.X = (int)w.Bounds.Max.X;
                }
                if (w.Bounds.Max.Y >= mapSize.Y)
                {
                    mapSize.Y = (int)w.Bounds.Max.Y;
                }

            }


            return mapSize;
        }


        /// <summary>
        /// Returns a list that is the path that the unit should take. 
        /// The list is 'backwards' so to speak, as in the closest move to make
        /// is at the end of the list. 
        /// </summary>
        /// <param name="START"></param>
        /// <param name="GOAL"></param>
        /// <returns></returns>
        public List<Vertex> ShortestPathSlow(Vertex START, Vertex GOAL)
        {
            OPEN.Clear();
            CLOSED.Clear();
            _currentVertex = START;
            OPEN.Enqueue(_currentVertex);

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
                    double cost = _currentVertex.Distance + GetCost(_currentVertex, neighbor);

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

            _ONES_TO_DRAW = ColorPathSlow(startingPoint, GOAL);
            return _ONES_TO_DRAW;
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
                    if (leftCheck.Intersects(vertex.RECTANGLE) && !vertex.Is_Wall)
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (rightCheck.Intersects(vertex.RECTANGLE) && !vertex.Is_Wall)
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (topCheck.Intersects(vertex.RECTANGLE) && !vertex.Is_Wall)
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (botCheck.Intersects(vertex.RECTANGLE) && !vertex.Is_Wall)
                    {
                        neighboringVertecies.Add(vertex);
                    }
                }

            }

            return neighboringVertecies;
        }


        /// <summary>
        /// This returns a list that has the closest move to make at the 
        /// end of the lsit, so to traverse this list hten you have to 
        /// make sure that you account for that.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public List<Vertex> ColorPathSlow(Vertex start, Vertex goal)
        {
            Vertex currentPathVertex = goal;

            List<Vertex> backwardsResults = new List<Vertex>();
            backwardsResults.Add(currentPathVertex);

            while (currentPathVertex != start)
            {
                if (currentPathVertex.NeighboringVertex != null && currentPathVertex.NeighboringVertex.VertColor != Color.Green)
                {
                    currentPathVertex = currentPathVertex.NeighboringVertex;
                    currentPathVertex.VertColor = Color.Aqua;
                    backwardsResults.Add(currentPathVertex);
                }
                else
                {
                    return backwardsResults;
                    //return;
                }
            }

            return backwardsResults;

        }


        /// <summary>
        /// This is a helper method that I will use to compute the cost
        /// The cost is the absoulte value of the (cur X - next X) + (cur Y - next Y)
        /// </summary>
        /// <returns>Absolute value of the difference between the distances</returns>
        public double GetCost(Vertex current, Vertex next)
        {
            return Math.Abs(current.RECTANGLE.X - next.RECTANGLE.X)
                + Math.Abs(current.RECTANGLE.Y - next.RECTANGLE.Y);

            // The following is another heuristic that works, but it does the same thing i think.

            /* double  xDist = Math.Abs((current.RECTANGLE.X - current.RECTANGLE.Width / 2) - (next.RECTANGLE.X - next.RECTANGLE.Width / 2));
             double  yDist = Math.Abs((current.RECTANGLE.Y - current.RECTANGLE.Height / 2) - (next.RECTANGLE.Y - next.RECTANGLE.Height / 2));
             if (xDist > yDist)
             {
                 return 1.4 * yDist + (xDist - yDist);
             }
             else {
                 return 1.4 * xDist + (yDist - xDist);
             }*/
        }


        /// <summary>
        /// This will check to see which 
        /// </summary>
        /// <param name="mouseLoc"></param>
        public Vertex SelectVertex(Vector2 mouseLoc)
        {
            Vertex VHere = new Vertex(Point.Zero, Point.Zero);

            foreach(Vertex v in _ALL_VERTECIES)
            {
                if (v.RECTANGLE.Contains(mouseLoc))
                {
                    if(_GOAL != null)
                        _GOAL.VertColor = Color.White;
                    //_GOAL = v;
                    VHere = v;
                    //_GOAL.VertColor = Color.Red;
                    VHere.VertColor = Color.Red;
                }
            }
            return VHere;
        }


        /// <summary>
        /// Draws all the vertecies in the list _ALL_VERTECIES
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Vertex v in _ALL_VERTECIES)
            {
                v.Draw(sb, _pixel);
            }

        }


        #endregion

    }
}
