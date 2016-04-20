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

        public BoundingSphere FIRE_RADIUS { get { return this._FIRE_RADIUS; } set { _FIRE_RADIUS = value; } }

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

        }

        /// <summary>
        /// All this update needs to do is check to see if there is an
        /// enemy withinthe range, and if soo then shoot in that direction
        /// </summary>
        /// <param name="units"></param>
        public void Update(GameTime gt, List<Unit> units)
        {
            // Check if the unit is still alive
            if (IsAlive)
            {
                BulletCheck();
                foreach (Unit u in units)
                {
                    if (u.IsAlive)
                    {
                        AttackUnit(u, gt);
                        for (int b = 0; b < ActiveBullets.Count; b++)
                        {
                            ActiveBullets[b].DamageCheck(u);
                        }
                    }
                }                
            }
            else
            {
                if (DeathTimer < 0)
                {
                    DeathTimer = 5;
                    IsAlive = true;
                    Health = 100;
                }
                else
                {
                    DeathTimer -= gt.ElapsedGameTime.TotalSeconds;
                }
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
                    Bullet newBullet = new Bullet(5, this.Attack, FIRE_RADIUS.Radius, 5, Center, this.BulletTexture);
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

/*
public void UpdateEnemy(GameTime gt, List<Unit> playerUnits)
{
if (isAlive)
{
    foreach (Unit u in playerUnits)
    {
        if (u.isAlive)
        {
            AttackUnit(u, gt);
        }
    }
}
else
{
    if (deathTimer < 0)
    {
        deathTimer = 5;
        isAlive = true;
        health = 100;
    }
    else
    {
        deathTimer -= gt.ElapsedGameTime.TotalSeconds;
    }
}
}*/
