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
<<<<<<< HEAD
        public String name;
        public int visionRange, attackRange, attack, defense, speed;
        public double health;
        public Boolean isSelected, isMoving;
        public Vector2 position;
        public Texture2D texture;
=======
        private String name;
        private int visionRange, attackRange, attack, defense, x, y;
        private double speed;
        private double health;
        private Boolean isSelected, isMoving;
        private Rectangle position;
        private Texture2D texture;
>>>>>>> origin/master

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
        }
<<<<<<< HEAD
        /*
=======

        public String Name { get { return name; } }

>>>>>>> origin/master
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

<<<<<<< HEAD
        public void ProcessInput(Vector2 mouseLoc)
=======


        public void ProcessInput(Point mouseLoc)
>>>>>>> origin/master
        {
            Vector2 distance = new Vector2(mouseLoc.X - position.X, mouseLoc.Y - position.Y);
            if (distance.Length() < speed)
            {
                position = mouseLoc;
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
