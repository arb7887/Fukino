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
        private int  attackRange, attackStrength, size;
        private double health, speed, rateOfFire, remainingDelay;
        private Boolean isSelected, isMoving, isAlive;
        private Vector2 position;
        private Vector2 destination;
        private Texture2D texture, bulletTexture, icon;
        private Color color;
        private Bullet bullet;
        private List<Bullet> activeBullets;
        private BoundingSphere bounds;
        private float radius;
        private float spawnTimer;
        private Dictionary<string, Unit> unitsDictionary;
        private HealthBar healthbar;

        private Teams myTag;

        // Respawn stuff
        private Vector2 spawnLocation;
        private int respawnTime;

        // Pathfinding stuffs
        private Vertex _Destination_Vertex, _Vertex_Im_On;
        private List<Vertex> _backwards_List;
        private int _Where_I_Am_in_List;
        private float _timePassed, _timeBetween;
        private bool _IS_FIRST_MOVE, _DONE_MOVING;
        #endregion


        #region Constructors
        public Unit(String name, int health, double speed, int attackRange, int attack, double rateOfFire)
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.attackRange = attackRange;
            this.attackStrength = attack;
            this.rateOfFire = rateOfFire;

            isSelected = false;
            isMoving = false;

            color = Color.White;

            this.activeBullets = new List<Bullet>();
            radius = 25;

            bounds = new BoundingSphere(new Vector3(position, 0), radius);
            isAlive = true;
            respawnTime = 8;

            _timeBetween = 200f;
            _timePassed = 0f;
            _IS_FIRST_MOVE = true;

            healthbar = new HealthBar(position);
        }

        public Unit(Unit newUnit)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attackStrength, newUnit.rateOfFire)
        {}
        #endregion


        #region Properties
        public int Where_I_Am_In_List { get { return _Where_I_Am_in_List; } set { _Where_I_Am_in_List = value; } }
        public bool DONE_MOVING { get { return _DONE_MOVING; } set { _DONE_MOVING = value; }}
        public Vertex Vertex_Im_ON { get { return _Vertex_Im_On; } set { _Vertex_Im_On = value; } }
        public List<Vertex> Backwards_List { get { return _backwards_List; } set { _backwards_List = value; } }
        public Vertex Destination_Vertex { get { return _Destination_Vertex; } set { _Destination_Vertex = value; } }
        public int AttackStrength { get { return attackStrength; } set { attackStrength = value; } }
        public double Health { get { return health; } set { health = value; } }
        public String Name { get { return name; } }
        public Teams Team { get { return this.myTag; } set { myTag = value; } }
        public Boolean IsSelected { get { return isSelected; } set { isSelected = value; } }
        public double RateOfFire { get { return this.rateOfFire; } set { this.rateOfFire = value; } }
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
        public HealthBar Healthbar { get { return healthbar; } set { healthbar = value; } }
        // Bullet properties=======================
        public int Size { get { return size; } set { size = value; } }
        public Texture2D BulletTexture { get { return bulletTexture; } set { bulletTexture = value; } }
        public Bullet Bullet { get { return bullet; } set { bullet = value; } }
        public List<Bullet> ActiveBullets { get { return activeBullets; } set { activeBullets = value; } }
        //===========================================
        public Dictionary<string, Unit> UnitsDictionary { get { return unitsDictionary; } set { unitsDictionary = value; } }
        #endregion


        #region Methods

        #region Attacking Methods
        public void AttackUnit(Unit u, GameTime gt)
        {
            Vector2 distance = new Vector2(position.X - u.Position.X, position.Y - u.Position.Y);
            double timer = gt.ElapsedGameTime.TotalSeconds;
            remainingDelay -= timer;

            if (remainingDelay <= 0)
            {
                if (this.Name == "Shotgun")
                    ShotgunSpray(u.position);
                else
                    AttackPosition(u.Position);
                remainingDelay = 1/rateOfFire;
            }
        }

        public void AttackPosition(Vector2 target)
        {
            Bullet newBullet = new Bullet(50, attackStrength, attackRange, 5, this.position, bulletTexture);
            newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
            newBullet.Destination = target;
            activeBullets.Add(newBullet);
            newBullet = null;
        }


        public void ShotgunSpray(Vector2 target)
        {
            List<Bullet> bulletSpray = new List<Bullet>();
            double degree = -5;
            double angle = degree * (Math.PI / 180);
            for (int i = 0; i < 5; i++)
            {
                Bullet newBullet = new Bullet(50, attackStrength, attackRange, 5, this.position, bulletTexture);
                newBullet.Bounds = new BoundingSphere(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), (float)newBullet.Size / 2);
                newBullet.Destination = new Vector2((float)((target.X * Math.Cos(angle)) - (target.Y * Math.Sin(angle))), (float)((target.X * Math.Sin(angle)) + (target.Y * Math.Cos(angle))));
                bulletSpray.Add(newBullet);
                degree += 2.5;
                angle = degree * (Math.PI / 180);
            }
            foreach (Bullet b in bulletSpray)
            {
                activeBullets.Add(b);
            }
        }

        public void BulletCheck(Map m)
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
                    foreach (Wall w in m.Walls)
                    {
                        activeBullets[i].WallCheck(w);
                    }
                }
            }
        }
        #endregion



        public void MoveToVector2(Vector2 destination)
        {
            // SO, I have to take this mouse location, which is the location on the screen
            // And convert it to a "world" coordinate

            Vector2 distance = new Vector2(destination.X - position.X, destination.Y - position.Y);


            if (distance.Length() < speed)
            {
                // This checks the collisions with the walls
                position = destination;

                isMoving = false;
            }
            else
            {
                //distance.Normalize();
                Vector2 toMove = new Vector2((int)(distance.X * speed), (int)(distance.Y * speed));
                // Checks with the walls... again?

                position += distance;
                bounds = new BoundingSphere(new Vector3(position, 0), radius);
            }
        }


        /// <summary>
        /// This method will take in a vertex, and move the unit in the general direction of that
        /// </summary>
        /// <param name="selected"></param>
        public void Move(GameTime gameTime)
        {
            // Set my current vertex to the positoin in the backwards list
            _timePassed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if(_timePassed >= _timeBetween && _Where_I_Am_in_List != -1)
            {
                _DONE_MOVING = false;
                // Update the location of the unit
                if (_Where_I_Am_in_List >= 0)
                {
                    _timePassed = 0;
                    _Destination_Vertex = _backwards_List[_Where_I_Am_in_List];
                    _Where_I_Am_in_List--;
                    _Destination_Vertex.VertColor = Color.Aqua;
                    MoveToVector2(new Vector2(_Destination_Vertex.RECTANGLE.Center.X, _Destination_Vertex.RECTANGLE.Center.Y));
                    // Move the unit to the destination vertex

                }
                else
                {
                    // Tell me that I am done moving
                    _DONE_MOVING = true;
                }

            }

        }


        public void Reset()
        {
            isAlive = true;
            position = spawnLocation;
            spawnTimer = 0;
            this.health = unitsDictionary[this.name].Health;
            destination = position;
        }


        public void changeClass(string nameToBe)
        {
            this.health = unitsDictionary[nameToBe].Health;
            this.speed = unitsDictionary[nameToBe].Speed;
            this.attackRange = unitsDictionary[nameToBe].AttackRange;
            this.attackStrength = unitsDictionary[nameToBe].AttackStrength;
            this.rateOfFire = unitsDictionary[nameToBe].RateOfFire;
            this.name = nameToBe;
        }
        
        public void Update(GameTime gt, List<Enemy> otherUnits, Camera cam, Map map)
        {
            bounds = new BoundingSphere(new Vector3(position, 0), radius);
            // This boolean is just for collision detection, if it is true thne move, 
            // If it is ever changed to false then stop moving
            //bool allowedToMove = true;

            #region If the unit is dead
            if (!isAlive)
            {
                //allowedToMove = false;

                var delta = (float)gt.ElapsedGameTime.TotalSeconds;
                spawnTimer += delta;
                if (spawnTimer >= respawnTime)
                {
                    Reset();
                }
            }
            #endregion

            #region If the unit is alive
            else
            {
                // Check if the unit is on the capture point
                map.checkCapturing(this);

                // Checks the movement

                if(health <= 0)
                {
                    isAlive = false;
                    position = new Vector2(-200, -200);
                }
                if (_backwards_List != null)
                {
                    // This makes sure that the positoin in the list is not always rest, only when it needs to be
                    if (_IS_FIRST_MOVE)
                    {
                        _Where_I_Am_in_List = _backwards_List.Count - 1;
                        _IS_FIRST_MOVE = false;
                    }

                    // This method tells the unit to update it's position
                    Move(gt);
                }

                // Bullet check
                BulletCheck(map);
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
                    }
                    for (int b = 0; b < activeBullets.Count; b++)
                    {
                        activeBullets[b].DamageCheck(u);
                    }
                }
                if (minDistance < attackRange && closestEnemy != null)
                {
                    AttackUnit(closestEnemy, gt);
                }
            }
            #endregion
            healthbar.Update(health);
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
                healthbar.Draw(sb);

                sb.Draw(unitsDictionary[this.name].Texture, new Rectangle((int)(position.X - radius), (int)(position.Y - radius), 50, 50), color);
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
