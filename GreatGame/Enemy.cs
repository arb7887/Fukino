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
        private BoundingSphere _FIRE_RADIUS;   // This is the radius that this enemy will use to see if it is colliding with any other units
        private double _DELAY_BETWEEN_SHOOTS;
        private Vector2 _SPAWN_POINT;

        public BoundingSphere FIRE_RADIUS { get { return this._FIRE_RADIUS; } set { _FIRE_RADIUS = value; } }
        public Double DELAY_BETWEEN_SHOOTS { get { return this._DELAY_BETWEEN_SHOOTS; } set { _DELAY_BETWEEN_SHOOTS = value; } }
        public Vector2 SPAWN_POINT { get { return this._SPAWN_POINT;  } set { this._SPAWN_POINT = value; } }

        /// <summary>
        /// This is the constructor that we will use for the "AI" in the game. 
        /// This needs to be able to find the shortest path to the objective. 
        /// Enemy's should shoot on sight when a player unit comes inside of a radius to
        /// this unit
        /// </summary>
        /// <param name="u"></param>
        /// <param name="i"></param>
        public Enemy(Unit u, int i, float fireRadius) 
            : base(u, i)
        {
            _FIRE_RADIUS = new BoundingSphere(new Vector3(this.Position, 0), fireRadius);
            this.Team = Teams.Enemy;
        }

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
            // Check if the unit is still alive
            if(this.Health > 0)
            {
                // Check if there are any player units inside of the range, if so, shoot them   
                foreach (Unit u in units)
                {
                    if (CheckRange(u))
                    {
                        Shoot(u, gt);
                        // Move away from that unit a little bit
                    }
                }
            }
            else
            {
                // Kill the unit
            }
        }

        /// <summary>
        /// This method simply checks if the given unit is inside of the bounding shpere
        /// that is the enemies range of fire
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool CheckRange(Unit u)
        {
            // Check to see if any units are inside of this units range sphere
            if (_FIRE_RADIUS.Intersects(u.Bounds))
            {
                return true;
            }
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
            _DELAY_BETWEEN_SHOOTS -= timer;
            if (_DELAY_BETWEEN_SHOOTS <= 0)
            {
                if (distance.Length() <= this.AttackRange)
                {
                    Bullet newBullet = new Bullet(5, this.ATTACK_STRENGTH, FIRE_RADIUS.Radius, 5, Center, this.BulletTexture);
                    newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
                    newBullet.Destination = u.Center;
                    ActiveBullets.Add(newBullet);
                    newBullet = null;
                }
                _DELAY_BETWEEN_SHOOTS = this.RateOfFire;
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.DrawString(font, "HEALTH: " + this.Health, new Vector2(this.Position.X, this.Position.Y - 20), Color.Black);

            sb.Draw(this.Texture, new Rectangle((int)Position.X, (int)Position.Y, 50, 50), this.UnitColor);
        }
    }
}

