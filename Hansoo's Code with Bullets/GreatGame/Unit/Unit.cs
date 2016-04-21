﻿using System;
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
        // Fields
        private String name;
        private int visionRange, attackRange, attack, defense, size;
        private double health, speed, rateOfFire, remainingDelay, deathTimer;
        private Boolean isSelected, isMoving, isAlive;
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

        private Teams myTag;
        private Vector2 prevCamPos;



        #endregion

        // FSM for the alignment of this class
        //public enum Tag { Player, Enemy, Neutral }

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
            center = new Vector2(position.X + size / 2, position.Y + size / 2);
            color = Color.White;
            this.activeBullets = new List<Bullet>();
            this.indexOfMe = indexOfMe;
            radius = 25;
            bounds = new BoundingSphere(new Vector3(position, 0), radius);
            prevCamPos = new Vector2(0,0);
            deathTimer = 5;
            isAlive = true;
        }

        public Unit(Unit newUnit, int index)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attack, newUnit.rateOfFire, index)
        {

        }
        #endregion

        // Properties
        #region Properties
        public int Attack { get { return attack; } set { attack = value; } }

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
        public String Name { get { return name; } }

        public Teams Team { get { return this.myTag; } set { myTag = value; } }

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

        public bool IsAlive { get { return this.isAlive; } set { isAlive = value; } }

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

        public int AttackRange
        {
            get { return attackRange; }
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


        // Bullet properties=======================
        public Vector2 Center { get { return center; } set { center = value; } }

        public int Size { get { return size; } set { size = value; } }

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

        public double DeathTimer { get { return deathTimer; } set { deathTimer = value; } }
  

        public Bullet Bullet { get { return bullet; } set { bullet = value; } }

        public List<Bullet> ActiveBullets { get { return activeBullets; } }
        //===========================================
        #endregion

        // Methods
        #region methods
        public Boolean checkCollision(Unit u)
        {
            if (u.Bounds.Intersects(this.bounds))
            {
                // Move the object back a little bit in the opposite directoin then it was going
                // Get the destination of the two things
                if (this.IsMoving)
                {
                    Vector2 direction = new Vector2((position.X - destination.X) * -1, (position.Y - destination.Y) * -1);
                    direction.Normalize();
                    position += direction * 5;
                    destination = position;
                }
                return true;
            }
            return false;
        }

        public Boolean checkCollision(Wall wall)
        {
            if (wall.Bounds.Intersects(this.bounds))
            {
                Vector2 direction = new Vector2((position.X - destination.X), (position.Y - destination.Y)) * -1;
                direction.Normalize();
                position += direction * 5;
                destination = position;
                return true;
            }
            return false;
        }

        public void AttackPosition(Vector2 target, GameTime gt)
        {
            /*Vector2 distance = new Vector2(center.X - target.X, center.Y - target.Y);
            double timer = gt.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            if (remainingDelay <= 0)
            {*/
                Bullet newBullet = new Bullet(5, attack, attackRange, 5, center, bulletTexture);
                newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
                newBullet.Destination = target;
                activeBullets.Add(newBullet);
                newBullet = null;
            //}
        }

        public void AttackUnit(Unit u, GameTime gt)
        {
            Vector2 distance = new Vector2(center.X - u.Position.X, position.Y - u.position.Y);
            double timer = gt.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;
            if (remainingDelay <= 0)
            {
                if (distance.Length() <= attackRange)
                {
                    Bullet newBullet = new Bullet(5, attack, attackRange, 5, center, bulletTexture);
                    newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
                    newBullet.Destination = u.Center;
                    activeBullets.Add(newBullet);
                    newBullet = null;
                }
                remainingDelay = rateOfFire;
            }
            if (u.health <= 0)
            {
                u.IsAlive = false;
            }
        }

        public void BulletCheck()
        {
            for (int i = 0; i < activeBullets.Count; i++)
            {
                if (activeBullets[i].ToDelete)
                {
                    activeBullets.RemoveAt(i);
                }
                else
                {
                    activeBullets[i].Move();
                    activeBullets[i].DistanceCheck();
                }
            }
        }


        public void ProcessInput(Vector2 mouseLoc, Map m)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(mouseLoc.X - position.X , mouseLoc.Y - position.Y );

            if (distance.Length() < speed)
            {
                BoundingSphere check = new BoundingSphere(new Vector3(mouseLoc, 0), radius);
                foreach (Wall w in m.Walls)
                {
                    if (w.Bounds.Intersects(check))
                    {
                        return;
                    }
                }
                position = mouseLoc;
                center = new Vector2(mouseLoc.X + radius, mouseLoc.Y + radius);
                isMoving = false;
            }
            else
            {
                distance.Normalize();
                Vector2 toMove = new Vector2((int)(distance.X * speed), (int)(distance.Y * speed));

                BoundingSphere check = new BoundingSphere(new Vector3(position + toMove, 0), radius);
                foreach (Wall w in m.Walls)
                {
                    if (w.Bounds.Intersects(check))
                    {
                        return;
                    }
                }
                position += toMove;
                center += toMove;
                bounds = new BoundingSphere(new Vector3(position, 0), radius);
            }
        }

#endregion
        public void Draw(SpriteBatch sb, SpriteFont font, Camera cam)
        {
            // Basic draw function for the units class
            //b.DrawString(font, this.name, new Vector2(this.position.X, this.position.Y - 10), Color.Black);

            //b.DrawString(font, "X:" + this.bounds.Center.X.ToString() + "Y:" + this.bounds.Center.Y.ToString(), new Vector2(this.position.X, this.position.Y - 30), Color.Black);

             sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);

            //.DrawString(font, "DESTINATION:" + this.destination.ToString(),new Vector2(this.position.X, this.position.Y - 20), Color.Black);
            //Console.WriteLine("CAM.POS: X = " + cam.Pos.X + " Y= " + cam.Pos.Y);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y , 50,50), color);

        }

        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, KeyboardState kbState, KeyboardState kbPState,
            List<Unit> userSelectedUnits, List<Enemy> otherUnits, Camera cam, Map map)
        {
           if (isAlive)
            {
                bool allowedToMove = true;

                foreach(Wall w in map.Walls)
                {
                    if (checkCollision(w))
                        allowedToMove = false;
                }
                map.checkCapturing(this);

                // Checks the movement

                if (allowedToMove)
                {
                    if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                    {
                        Vector2 prevMouseVector = new Vector2(previousMouse.X, previousMouse.Y);

                        // I need to account for the camera location in here
                        if (((GetMouseWorldPos(prevMouseVector, cam.Pos).X) >= Position.X) && (GetMouseWorldPos(prevMouseVector, cam.Pos).X) <= (Position.X + (radius * 2))
                            && (GetMouseWorldPos(prevMouseVector, cam.Pos).Y) >= Position.Y && (GetMouseWorldPos(prevMouseVector, cam.Pos).Y) <= (Position.Y + (radius * 2)))
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
                        destination = new Vector2(previousMouse.X + cam.Pos.X * cam.CamSpeed, previousMouse.Y + cam.Pos.Y * cam.CamSpeed);


                        ProcessInput(destination, map);
                        IsMoving = true;
                    }
                    else if (IsMoving)
                    {
                        ProcessInput(destination, map);
                    }
                    if (isSelected && (kbPState.IsKeyDown(Keys.Space) && kbState.IsKeyUp(Keys.Space)))
                    {
                        AttackPosition(new Vector2(currentMouse.X + cam.Pos.X * cam.CamSpeed, currentMouse.Y + cam.Pos.Y* cam.CamSpeed), gt);
                    }
                }

                BulletCheck();
                Unit closestEnemy = null;
                double minDistance = Double.MaxValue;
                foreach (Unit u in otherUnits)
                {
                    if (u.isAlive)
                    {
                        if (minDistance > new Vector2(position.X - u.Position.X, position.Y - u.Position.Y).Length())
                        {
                            closestEnemy = u;
                            minDistance = new Vector2(position.X - u.Position.X, position.Y - u.Position.Y).Length();
                        }
                        for (int b = 0; b < activeBullets.Count; b++)
                        {
                            activeBullets[b].DamageCheck(u);
                        }
                    }
                }
                if (minDistance < attackRange && closestEnemy != null)
                {
                    AttackUnit(closestEnemy, gt);
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
        }

        /*public void UpdateEnemy(GameTime gt, List<Unit> playerUnits)
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
                BulletCheck();
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
