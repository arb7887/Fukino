using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatGame
{
    class Unit : ICollidable, IDamageable
    {
        // Feilds
        private String name;
        private int size, visionRange, attackRange, attack, defense, speed;
        private double health;

        enum Alignment
        {
            Player,
            Enemy,
            Neutral
        }

        public Unit(String name, int health, int speed, int range, int damage)
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

        // Methods

        public void TakeDamage(double damage)
        {
            health -= damage;
        }

        public void Attack(Unit u)
        {

        }
    }
}
