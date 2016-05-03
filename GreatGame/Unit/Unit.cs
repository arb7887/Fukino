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
        private Vector2 prevCamPos;

        // Respawn stuff
        private Vector2 spawnLocation;
        private int respawnTime;

        // Stuff for pathfinding
        private Graph graph;
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

        // Properties
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

        // Bullet properties=======================
       // public Vector2 Center { get { return center; } set { center = value; } }
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

        public void ProcessInput(Vector2 mouseLoc, Map m)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(mouseLoc.X - position.X, mouseLoc.Y - position.Y);


            if (distance.Length() < speed)
            {
                // This checks the collisions with the walls

                BoundingSphere check = new BoundingSphere(new Vector3(mouseLoc, 0), radius);
                foreach (Wall w in m.Walls)
                {
                    if (w.Bounds.Intersects(check))
                    {
                        return;
                    }
                }

                position = mouseLoc;

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

        #region Update
        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, KeyboardState kbPrevState, KeyboardState kbState,
            List<Unit> userSelectedUnits, List<Enemy> otherUnits, Camera cam, Map map)
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
                    if (previousMouse.LeftButton == ButtonState.Pressed && currentMouse.LeftButton == ButtonState.Released)
                    {
                        // The previsous mouse vector
                        Vector2 prevMouseVector = new Vector2(previousMouse.X, previousMouse.Y);
                        // Get the mouse's world position
                        Vector2 mouseWorldPos = GetMouseWorldPos(prevMouseVector, cam.Pos * cam.CamSpeed);

                        // I need to account for the camera location in here
                        if ((mouseWorldPos.X >= Position.X - radius) && (mouseWorldPos.X) <= Position.X + radius
                            && (mouseWorldPos.Y) >= Position.Y - radius && (mouseWorldPos.Y) <= Position.Y + radius)
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

                    // Check if the use is hitting the shoot button
                    if (isSelected && (kbPrevState.IsKeyDown(Keys.Space) && kbState.IsKeyUp(Keys.Space)))
                    {
                        AttackPosition(new Vector2(currentMouse.X + cam.Pos.X * cam.CamSpeed, currentMouse.Y + cam.Pos.Y * cam.CamSpeed), gt);
                    }
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
        #endregion


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
