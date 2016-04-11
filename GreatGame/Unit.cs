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
        private String name;
        private int visionRange, attackRange, attack, defense, size;
        private double health, speed, rateOfFire, remainingDelay;
        private Boolean isSelected, isMoving;
        private Vector2 position;
        private Vector2 center;
        private Vector2 destination;
        private Texture2D texture, bulletTexture;
        private Color color;
        private Bullet bullet;
        private List<Bullet> activeBullets;
        private BoundingSphere bounds;
        private int indexOfMe;
        private float radius;

        private Tag myTag;
        private Vector2 prevCamPos;


        // FSM for the alignment of this class
        public enum Tag
        {
            Player,
            Enemy,
            Neutral
        }

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
            center = new Vector2(position.X + size/ 2, position.Y + size / 2);
            color = Color.White;
            this.activeBullets = new List<Bullet>();
            bounds = new BoundingSphere(new Vector3(center.X, center.Y, 0), size/2);

            this.indexOfMe = indexOfMe;
            radius = 25;
            prevCamPos = new Vector2(0,0);

        }

        public Unit(Unit newUnit, int index)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attack, newUnit.rateOfFire, index)
        {

        }

        // Properties
        public String Name { get { return name; } }

        public int Attack { get { return attack;  } set { attack = value; } }

        public Double Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
        
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

        public Vector2 Center { get { return center; } set { center = value; } }

        public int Size { get { return size; } set { size = value; } }

        public Color UnitColor
        {
            get { return color; }
            set { color = value; }
        }

        public BoundingSphere Bounds { get { return bounds; } set { bounds = value; } }

        public Texture2D BulletTexture
        {
            get
            {
                return bulletTexture;
            }
            set
            {
                bulletTexture = value;
            }
        }

        public Bullet Bullet { get { return bullet; } set { bullet = value; } }

        public List<Bullet> ActiveBullets { get { return activeBullets; } }

        // Methods
        public Boolean checkCollision(Unit u)
        {
            if (u.Bounds.Intersects(this.bounds))
            {
                // Move the object back a little bit in the opposite directoin then it was going
                // Get the destination of the two things
                
                this.destination = new Vector2(u.destination.X - 50, u.destination.Y - 50);


                return true;
            }

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
        /*public void TakeDamage(double damage)
        {
            health -= damage;
        }*/

        public void AttackUnit(Unit u, GameTime gt)
        {
            Vector2 distance = new Vector2(position.X - u.Position.X, position.Y - u.position.Y);
            double timer = gt.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            if (remainingDelay <= 0)
            {
                if (distance.Length() <= attackRange)
                {
                    Bullet newBullet = new Bullet(5, attack, attackRange, 5, bulletTexture);
                    newBullet.Position = center;
                    newBullet.StartingLocation = center;
                    newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float) newBullet.Size/2);
                    newBullet.Destination = u.Center;
                    activeBullets.Add(newBullet);
                    newBullet = null;
                }
                remainingDelay = rateOfFire;
            }
            if (u.health <= 0)
            {
                Console.WriteLine(u.name + " has died");
            }
            for (int i = 0; i < activeBullets.Count; i++)
            {
                if (activeBullets[i].ToDelete)
                {
                    activeBullets.RemoveAt(i);
                }
                else
                {
                    activeBullets[i].Move();
                    activeBullets[i].DamageCheck(u);
                }
            }
        }

        public void ProcessInput(Vector2 mouseLoc, Camera cam)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(mouseLoc.X - position.X , mouseLoc.Y - position.Y );

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
                center += toMove;
                bounds = new BoundingSphere(new Vector3(toMove.X, toMove.Y, 0), size);
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Camera cam)
        {
            // Basic draw function for the units class
            //b.DrawString(font, this.name, new Vector2(this.position.X, this.position.Y - 10), Color.Black);

            //b.DrawString(font, "X:" + this.bounds.Center.X.ToString() + "Y:" + this.bounds.Center.Y.ToString(), new Vector2(this.position.X, this.position.Y - 30), Color.Black);

             sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);
             sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 50, 50), color);
        }

        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, List<Unit> enemyUnits)
        {
            //if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
            //.DrawString(font, "DESTINATION:" + this.destination.ToString(),new Vector2(this.position.X, this.position.Y - 20), Color.Black);
            //Console.WriteLine("CAM.POS: X = " + cam.Pos.X + " Y= " + cam.Pos.Y);

           // sb.Draw(texture, new Rectangle((int)bounds.Center.X, (int)bounds.Center.Y , 50,50), color);

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
                    if (checkCollision(otherUnits[i]))
                    {
                        // Dont move
                        allowedToMove = false;                      
                    }
                }
            }
            // Checks the movement

            if (allowedToMove)
            {            
                if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                {
                    Vector2 prevMouseVector = new Vector2(previousMouse.X, previousMouse.Y);

                    // I need to account for the camera location in here
                    if (((GetMouseWorldPos(prevMouseVector, cam.Pos).X ) >= Position.X ) && (GetMouseWorldPos(prevMouseVector, cam.Pos).X) <= (Position.X + (radius * 2))
                        && (GetMouseWorldPos(prevMouseVector, cam.Pos).Y) >= Position.Y && (GetMouseWorldPos(prevMouseVector, cam.Pos).Y) <= (Position.Y  + (radius * 2)))
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
            // if there is a collision between units
            else
            {
                // Move the unit away from said object
                ProcessInput(-destination, cam);
                // Move the unit away from said unit

                ProcessInput(destination, cam);
                //ProcessInput(new Vector2(destination.X, destination.Y - 50));
            }
            foreach (Unit u in otherUnits)
            {
                AttackUnit(u, gt);
            }
        }



        public override string ToString()
        {
            return this.name + this.destination.ToString() + this.bounds.ToString();
        }

        public Vector2 GetMouseWorldPos(Vector2 screenPos, Vector2 camPos)
        {
            return screenPos + camPos;
        }
    }
}