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
        private int  attackRange, _ATTACK_STRENGTH, size;
        private double health, speed, rateOfFire, remainingDelay;
        private Boolean isSelected, isMoving, isAlive;
        private Vector2 position;
        private Vector2 destination;
        private Texture2D texture, bulletTexture, icon;
        private Color color;
        private Bullet bullet;
        private List<Bullet> activeBullets;
        private BoundingSphere bounds;
        private int indexOfMe;
        private float radius;
        private float timer;


        private Teams myTag;

        // Respawn stuff
        private Vector2 spawnLocation;
        private int respawnTime;

        #endregion


        #region Constructors
        public Unit(String name, int health, double speed, int attackRange, int attack, double rateOfFire, int indexOfMe)
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.attackRange = attackRange;
            this._ATTACK_STRENGTH = attack;
            this.rateOfFire = rateOfFire;
            this.indexOfMe = indexOfMe;

            isSelected = false;
            isMoving = false;

            color = Color.White;

            this.activeBullets = new List<Bullet>();
            radius = 25;

            bounds = new BoundingSphere(new Vector3(position, 0), radius);
            isAlive = true;
            respawnTime = 8;

            
        }

        public Unit(Unit newUnit, int index)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.ATTACK_STRENGTH, newUnit.rateOfFire, index)
        {

        }
        #endregion


        #region Properties
        public int ATTACK_STRENGTH { get { return _ATTACK_STRENGTH; } set { _ATTACK_STRENGTH = value; } }
        public Double Health { get { return health; } set { health = value; } }
        public String Name { get { return name; } }
        public Teams Team { get { return this.myTag; } set { myTag = value; } }
        public Boolean IsSelected { get { return isSelected; } set { isSelected = value; } }
        public Double RateOfFire { get { return this.rateOfFire; } set { this.rateOfFire = value; } }
        public bool IsAlive { get { return this.isAlive; } set { isAlive = value; } }
        public int SpawnTime { get { return respawnTime; } set { SpawnTime = value; } }
        public Vector2 SpawnLoc { get { return spawnLocation; } set { spawnLocation = value; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Texture2D Icon { get { return icon; } set { icon = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public double Speed { get { return speed; } set { speed = value; } }
        public int AttackRange { get { return this.attackRange; } set { this.attackRange = value; } }
        public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
        public Color UnitColor{get { return color; }set { color = value; }}
        public BoundingSphere Bounds{get { return bounds; }set { this.bounds = value; }}
        public float Radius { get { return radius; } set { radius = value; } }
        public Color Tint { get { return color; } set { color = value; } }
        public Vector2 Destination { get { return destination; } set { destination = value; } }
        // Bullet properties=======================
        public int Size { get { return size; } set { size = value; } }
        public Texture2D BulletTexture { get { return bulletTexture; } set { bulletTexture = value; } }
        public Bullet Bullet { get { return bullet; } set { bullet = value; } }
        public List<Bullet> ActiveBullets { get { return activeBullets; } set { activeBullets = value; } }
        //===========================================
        #endregion


        #region methods

        #region Attacking Methods
        public void AttackUnit(Unit u, GameTime gt)
        {
            Vector2 distance = new Vector2(position.X - u.Position.X, position.Y - u.position.Y);
            double timer = gt.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;

            if (remainingDelay <= 0)
            {
                if (distance.Length() <= attackRange)   
                {
                    Bullet newBullet = new Bullet(5, ATTACK_STRENGTH, attackRange, 5, this.position, bulletTexture);
                    newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
                    activeBullets.Add(newBullet);
                    newBullet = null;
                }
                remainingDelay = rateOfFire;
            }
        }

        public void AttackPosition(Vector2 target, GameTime gt)
        {
            Bullet newBullet = new Bullet(5, ATTACK_STRENGTH, attackRange, 5, this.position, bulletTexture);
            newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
            newBullet.Destination = target;
            activeBullets.Add(newBullet);
            newBullet = null;
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
        #endregion

        public void ProcessInput(Vector2 destination, Map m)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(destination.X - position.X, destination.Y - position.Y);


            if (distance.Length() < speed)
            {
                // This checks the collisions with the walls

                BoundingSphere check = new BoundingSphere(new Vector3(destination, 0), radius);
                foreach (Wall w in m.Walls)
                {
                    if (w.Bounds.Intersects(check))
                    {
                        return;
                    }
                }

                position = destination;

                isMoving = false;
            }
            else
            {
                distance.Normalize();
                Vector2 toMove = new Vector2((int)(distance.X * speed), (int)(distance.Y * speed));
                // Checks with the walls... again?
                BoundingSphere check = new BoundingSphere(new Vector3(position + toMove, 0), radius);
                foreach (Wall w in m.Walls)
                {
                    if (w.Bounds.Intersects(check))
                    {
                        return;
                    }
                }
                position += toMove;
                bounds = new BoundingSphere(new Vector3(position, 0), radius);
            }
        }
        
        public void Update(GameTime gt, List<Enemy> otherUnits, Camera cam, Map map)
        {
            // This boolean is just for collision detection, if it is true thne move, 
            // If it is ever changed to false then stop moving
            bool allowedToMove = true;

            #region If the unit is dead
            if (!isAlive)
            {
                allowedToMove = false;

                var delta = (float)gt.ElapsedGameTime.TotalSeconds;
                timer += delta;
                if (timer >= respawnTime)
                {
                    position = spawnLocation;
                    //center = position;
                    timer = 0;
                    isAlive = true;
                    if(name == "Rifle")
                    {
                        health = 200;
                    }
                    return;
                }
            }
            #endregion

            #region If the unit is alive
            else
            {
                // Check if the unit is on the capture point
                map.checkCapturing(this);

                // Checks the movement

                if (allowedToMove)
                {
                    ProcessInput(destination, map);
                }

                // Bullet check
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
            #endregion
        }
        
        public bool CheckClicked(Vector2 clickedLoc)
        {
            BoundingBox clicked = new BoundingBox(new Vector3(clickedLoc.X, clickedLoc.Y, 0), new Vector3(clickedLoc.X+1, clickedLoc.Y+1, 0));
            if (clicked.Intersects(bounds))
            {
                isSelected = true;
                color = Color.Cyan;
                return true;
            }
            else
            {
                isSelected = false;
                color = Color.White;
                return false;
            }
        }

        #region Draw
        public void Draw(SpriteBatch sb, SpriteFont font, Camera cam)
        {
            if (isAlive)
            {
                sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);

                sb.Draw(texture, new Rectangle((int)(position.X - radius), (int)(position.Y - radius), 50, 50), color);

                sb.DrawString(font, "X", this.position, Color.Red);
                sb.DrawString(font, "X", new Vector2(this.bounds.Center.X, this.bounds.Center.Y), Color.Blue);

            }
        }
        #endregion 

        public Vector2 GetMouseWorldPos(Vector2 screenPos, Vector2 camPos)
        {
            return screenPos + camPos;
        }
        #endregion
    }
}
