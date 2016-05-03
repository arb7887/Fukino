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
        #region Fields
        private float _FIRE_RADIUS;   // This is the radius that this enemy will use to see if it is colliding with any other units
        private double _DELAY_BETWEEN_SHOOTS;
        private Vector2 _SPAWN_POINT;
        Color _TEST_COLOR;
        private float timer;
        // Hansoo wtf is wrong with your variable naming they are all the fucking same
        private double remainingTime_;

        // Pathfinding stuff
        private Graph _MY_GRAPH;
        #endregion


        #region Properties
        public float FIRE_RADIUS { get { return this._FIRE_RADIUS; } set { _FIRE_RADIUS = value; } }
        public Double DELAY_BETWEEN_SHOOTS { get { return this._DELAY_BETWEEN_SHOOTS; } set { _DELAY_BETWEEN_SHOOTS = value; } }
        public Vector2 SPAWN_POINT { get { return this._SPAWN_POINT;  } set { this._SPAWN_POINT = value; } }
        #endregion


        #region Constructor
        /// <summary>
        /// This is the constructor that we will use for the "AI" in the game. 
        /// This needs to be able to find the shortest path to the objective. 
        /// Enemy's should shoot on sight when a player unit comes inside of a radius to
        /// this unit
        /// </summary>
        /// <param name="u">Unit of which we are basing the enemy on</param>
        /// <param name="i">Index in the array for this unit</param>
        public Enemy(Unit u, int i, float fireRadius, Map map) 
            : base(u, i)
        {
            // This is used to determine if the unit is inside the range of being shot
            _FIRE_RADIUS = fireRadius;

            this._TEST_COLOR = Color.Black;
            
            // Set this enemy has an Enemy in the game
            this.Team = Teams.Enemy;
            // Set the bullet texture
            this.BulletTexture = u.BulletTexture;
            // Set the texture
            this.Texture = u.Texture;

            // Pathfinding graph
            _MY_GRAPH = new Graph(map, 0, 0, 50);   
        }
        #endregion


        #region Methods
        /// <summary>
        /// This method will be used to find the shortest path to the objective
        /// </summary>
        public void ShortestPath()
        {
            // Call the graph class, and get the next position for this unit to move.
            
        }
        
        /// <summary>
        /// All this update needs to do is check to see if there is an
        /// enemy withinthe range, and if soo then shoot in that direction
        /// </summary>
        /// <param name="units"></param>
        public void Update(GameTime gt, List<Unit> units)
        {
            #region If  the unit is alive
            // Check if the unit is still alive
            if (this.Health > 0)
            {
                // Check if there are any player units inside of the range, if so, shoot them   
                foreach (Unit u in units)
                {
                    if (CheckRange(u))
                    {
                        // Change the test color to see if collision is working,
                        // And call the shoot method with them
                        _TEST_COLOR = Color.Red;
                        Shoot(u, gt);

                        // Move away from that unit a little bit
                    }
                    else
                    {
                        _TEST_COLOR = Color.Black;
                    }
                }

                this.IsAlive = true;
            }
            #endregion

            #region If the unit is dead
            else
            {
                // Kill the unit
                // Call the resetmethod
                
                this.Position = new Vector2(-20, -20);

                var delta = (float)gt.ElapsedGameTime.TotalSeconds;
                timer += delta;

                if (timer >= this.SpawnTime)
                {
                    this.Position = this.SpawnLoc;

                    timer = 0;

                    if (this.Name == "Rifle")
                    {
                        this.Health = 200;
                    }
                    else
                    {
                        this.Health = 200;
                    }
                    return;
                }
            }
            #endregion
        }

        /// <summary>
        /// This method simply checks if the given unit is inside of the bounding shpere
        /// that is the enemies range of fire
        /// </summary>
        /// <param name="u">Unit that is being checked</param>
        /// <returns></returns>
        public bool CheckRange(Unit u)
        {
            var distance_x = Math.Abs(this.Position.X - u.Position.X);
            var distance_Y = Math.Abs(this.Position.Y - u.Position.Y);

            if (distance_x < FIRE_RADIUS && distance_Y < FIRE_RADIUS)
                return true;

            return false;
        }

        /// <summary>
        /// This method is used to shoot in the direction of another Unit object
        /// This should only be called if the unit is inside of the fire radius
        /// </summary>
        /// <param name="u">The unit to shoot</param>
        /// <param name="gt">The game time in order to calculate the delate between shots</param>
        public void Shoot(Unit u, GameTime gt)
        {
            Vector2 distance = new Vector2(this.Position.X - u.Position.X, Position.Y - u.Position.Y);

            double timer = gt.ElapsedGameTime.TotalSeconds;
             remainingTime_ -= timer;

            if (_DELAY_BETWEEN_SHOOTS <= 0)
            {
               // if (distance.Length() <= this.AttackRange)
               // {
                    Bullet newBullet = new Bullet(5, this.ATTACK_STRENGTH, FIRE_RADIUS, 5, this.Position, this.BulletTexture);
                    newBullet.Bounds = new BoundingSphere(new Vector3(this.Position.X, this.Position.Y, 0), (float)newBullet.Size / 2);
                    //newBullet.Destination = u.Center;
                    ActiveBullets.Add(newBullet);
                    newBullet = null;
               // }
                remainingTime_ = _DELAY_BETWEEN_SHOOTS;
            }
        }


        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.DrawString(font, "BOUND: " + this.Bounds.Center, new Vector2(this.Position.X, this.Position.Y - 20), _TEST_COLOR);

            sb.Draw(this.Texture, new Rectangle((int)Position.X, (int)Position.Y, 50, 50), this.UnitColor);

        }
        #endregion
    }
}

