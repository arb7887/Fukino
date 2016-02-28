using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSGame
{
    abstract class Unit : ICollidable, IDamageable
    {
        int size, visionRange, attackRange, attack, defense, speed;
        double health;
        enum Alignment
        {
            Player,
            Enemy,
            Neutral
        }

        public Unit()
        {

        }

        public bool IsColliding(Unit u)
        {
            return false;
        }

        public bool IsColliding(Terrain t)
        {
            return false;
        }

        public void TakeDamage(double damage)
        {
            health -= damage;
        }

        public void Attack(Unit u)
        {

        }
    }
}
