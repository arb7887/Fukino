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
        private String name;
        private int visionRange, attackRange, attack, defense, x, y;
        private double speed;
        private double health;
        private Boolean isSelected, isMoving;
        private Rectangle position;
        private Texture2D texture;

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
            position = new Rectangle(x, y, 50, 50);
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                position = new Rectangle(x, y, position.Width, position.Height);
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
                position = new Rectangle(x, y, position.Width, position.Height);
            }
        }

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

        public Rectangle Position
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

        public void ProcessInput(Point mouseLoc)
        {
                double xDiff = mouseLoc.X - X;
                double yDiff = mouseLoc.Y - Y;
                double angle = Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
                X += (int)(Speed * Math.Cos(angle));
                Y += (int)(Speed * Math.Sin(angle));
        }

    }
}
