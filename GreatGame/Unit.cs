using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Unit : IDamageable
    {
        // Fields

        #region Fields
        
        public String name;
        public int visionRange, attackRange, attack, defense;
        public double health, speed;
        public Boolean isSelected, isMoving;
        public Vector2 position;
        public Texture2D texture;
        public Vector2 destination;
        public Color color;
        private BoundingSphere bounds;
        #endregion

        // FSM for the alignment of this class
        enum Tag
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
            color = Color.White;
        }

        public Unit(Unit newUnit)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attack)
        {

        }

        // Properties
        #region Properties
        public String Name { get { return name; } }
        
        
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

        public BoundingSphere Bounds { get { return bounds; } }
        #endregion
        // Methods
        public Boolean checkCollision(Wall w)
        {
            if (w.Bounds.Intersects(bounds))
                return true;
            return false;
        }

        public Boolean checkCollision(Unit u)
        {
            if (u.Bounds.Intersects(bounds))
                return true;
            return false;
        }

        public void TakeDamage(double damage)
        {
            health -= damage;
        }

        public void Attack(Unit u)
        {

        }


        public void ProcessInput(Vector2 mouseLoc)
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


        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            // Basic draw function for the units class
            sb.DrawString(font, this.name, new Vector2(this.position.X, this.position.Y - 10), Color.Black);

            sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 50, 50), color);

        }

        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits)
        {

            if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
            {
                if ((previousMouse.X >= Position.X) && previousMouse.X <= (Position.X + 50)
                    && previousMouse.Y >= Position.Y && previousMouse.Y <= (Position.Y + 50))
                {
                    IsSelected = true;
                    color = Color.Cyan;
                    userSelectedUnits.Add(this);
                }
                else
                {
                    IsSelected = false;
                    color = Color.White;
                    userSelectedUnits.Remove(this);
                }
            }
            if (IsSelected && (previousMouse.RightButton == ButtonState.Pressed && currentMouse.RightButton == ButtonState.Released))
            {
                destination = new Vector2(previousMouse.X, previousMouse.Y);
                ProcessInput(destination);
                IsMoving = true;
            }
            else if (IsMoving)
            {
                ProcessInput(destination);
            }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
