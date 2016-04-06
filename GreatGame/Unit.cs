using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Unit
    {
        // Fields
        #region Fields
        // Keeping these private is the whole point of object oriented programming
        private String name;
        private int visionRange, attackRange, attack, defense;
        private double health, speed;
        private Boolean isSelected, isMoving;
        private Vector2 position;
        private Texture2D texture;
        private Vector2 destination;
        private Color color;

        private BoundingSphere bounds;
        private double rateOfFire;
        private int indexOfMe;
        private float radius;

        private Tag myTag;
        private Vector2 prevCamPos;


        #endregion

        // FSM for the alignment of this class
        public enum Tag
        {
            Player,
            Enemy,
            Neutral
        }

        #region Constructors
        public Unit(String name, int health, double speed, int attackRange, int attack, double rateOfFire, int indexOfMe)
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.attackRange = attackRange;
            this.attack = attack;
            this.rateOfFire = rateOfFire;
            isSelected = false;
            isMoving = false;
            position = new Vector2(0, 0);
            color = Color.White;

            this.indexOfMe = indexOfMe;
            radius = 25;
            bounds = new BoundingSphere(new Vector3(position, 0), radius);
            prevCamPos = new Vector2(0,0);

        }

        public Unit(Unit newUnit, int index)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attack, newUnit.rateOfFire, index)
        {

        }
        #endregion

        // Properties
        #region Properties
        public String Name { get { return name; } }

        public Tag MyTag { get { return this.myTag; } set { myTag = value; } }
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

        public Double RateOfFire { get { return this.rateOfFire; } set { this.rateOfFire = value; } }

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

        public BoundingSphere Bounds
        {
            get { return bounds; }
            set { this.bounds = value; }
        }
        #endregion
        // Methods
        #region methods
        public Boolean checkCollision(Unit u)
        {
            if (u.Bounds.Intersects(this.bounds))
                return true;

            return false;
        }

        public Boolean checkCollision(Wall wall)
        {
            if (wall.Bounds.Intersects(this.bounds))
                return true;

            return false;
        }


        /// <summary>
        /// All this does it take in a number of how much damage that 
        /// this unit will take
        /// </summary>
        /// <param name="damage">Amount of damage taken</param>
        public void TakeDamage(double damage)
        {
            health -= damage;
        }

        /// <summary>
        /// This is the method that will be called for this unit to attack, 
        /// which will try to attack another unit
        /// </summary>
        /// <param name="u">Other unit to attack</param>
        public void Attack(Unit u)
        {
            // Spawn a bullet in the directoin that the unit is facing
        }


        public void ProcessInput(Vector2 mouseLoc, Camera cam)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(mouseLoc.X - position.X + prevCamPos.X, mouseLoc.Y - position.Y + prevCamPos.Y);
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
                bounds = new BoundingSphere(new Vector3(position, 0), radius);
            }
        }

#endregion
        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            // Basic draw function for the units class
            sb.DrawString(font, this.name, new Vector2(this.position.X, this.position.Y - 10), Color.Black);

            sb.DrawString(font, "X:" + this.bounds.Center.X.ToString() + "Y:" + this.bounds.Center.Y.ToString(), new Vector2(this.position.X, this.position.Y - 30), Color.Black);

            sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);

            //sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 50, 50), color);

            sb.Draw(texture, new Rectangle((int)bounds.Center.X, (int)bounds.Center.Y,50,50), color);

        }

        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, List<Unit> otherUnits, Camera cam)
        {
            bool allowedToMove = true;
            // Check the collisions
            // Loop through and check the collisons with all of the other units
            
            for (int i = 0; i < otherUnits.Count; i++)
            {
                if(i != indexOfMe)
                {
                    if (checkCollision(otherUnits[i]))                                        
                        allowedToMove = false;  // Don't move                   
                }
            }

            // Checks the movement

            if (allowedToMove)
            {            
                if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                {
                    if ((previousMouse.X >= Position.X) && previousMouse.X  <= (Position.X + (radius * 2))
                        && previousMouse.Y >= Position.Y  && previousMouse.Y <= (Position.Y  + radius * 2))
                    {
                        prevCamPos = cam.Pos;
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
                    destination = new Vector2(previousMouse.X + cam.Pos.X, previousMouse.Y + cam.Pos.Y);
                    ProcessInput(destination, cam);
                    IsMoving = true;
                }
                else if (IsMoving)
                {
                    ProcessInput(destination, cam);
                }
            }
            else
            {
                // Move the unit away from said object
                ProcessInput(-destination, cam);
            }
        }



        public override string ToString()
        {
            return this.name + this.bounds.ToString();
        }
    }
}
