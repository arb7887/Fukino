using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Unit : ICollidable, IDamageable
    {
        // Fields
        public String name;
        public int visionRange, attackRange, attack, defense, size;
        public double health, speed;
        public Boolean isSelected, isMoving;
        public Vector2 position;
        public Vector2 center;
        public Texture2D texture;
        public Color color;

        enum Alignment
        {
            Player,
            Enemy,
            Neutral
        }

        public Unit(String name, int health, double speed, int attackRange, int attack)
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.attackRange = attackRange;
            this.attack = attack;
            isSelected = false;
            isMoving = false;
            position = new Vector2(0, 0);
            center = new Vector2(position.X + size/ 2, position.Y + size / 2);
            color = Color.White;
        }
        public String Name { get { return name; } }

        /*
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                position = new Vector2(x, y);
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                position = new Vector2(x, y);
            }
        }
        */
        public Boolean IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public double Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
            set
            {
                isMoving = value;
            }
        }

        public Color UnitColor
        {
            get { return color; }
            set { color = value; }
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

        public void AttackUnit(Unit u)
        {
            Vector2 distance = new Vector2(position.X - u.Position.X, position.Y - u.position.Y);
            if (distance.Length() <= attackRange)
            {
                u.health -= attack;
                Console.WriteLine(name + " has attacked " + u.name + " for " + attack + " damage!");
            }
        }

        public void ProcessInput(Vector2 mouseLoc)
        {
            Vector2 distance = new Vector2(mouseLoc.X - position.X - size/2, mouseLoc.Y - position.Y - size/2);
            if (distance.Length() < speed)
            {
                position = new Vector2(mouseLoc.X - (size/2), mouseLoc.Y - (size/2));
                isMoving = false;
            }
            else
            {
                distance.Normalize();
                Vector2 toMove = new Vector2((int)(distance.X * speed), (int)(distance.Y * speed));
                position += toMove;
            }
        }
    }
}
