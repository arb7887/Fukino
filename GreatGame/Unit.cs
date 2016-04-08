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

        // FSM for the alignment of this class
        enum Tag
        {
            Player,
            Enemy,
            Neutral
        }

        public Unit(String name, int health, double speed, int attackRange, int attack, double rateOfFire)
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
        }

        public Unit(Unit newUnit)
            : this(newUnit.name, (int)newUnit.health, newUnit.Speed, newUnit.attackRange, newUnit.attack, newUnit.rateOfFire)
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
            if (u.Bounds.Intersects(bounds))
                return true;
            return false;
        }

        public void TakeDamage(double damage)
        {
            health -= damage;
        }

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
                center += toMove;
                bounds = new BoundingSphere(new Vector3(toMove.X, toMove.Y, 0), size);
            }
        }


        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            // Basic draw function for the units class
            sb.DrawString(font, this.name, new Vector2(this.position.X, this.position.Y - 10), Color.Black);

            sb.DrawString(font, "HEALTH: " + this.health, new Vector2(this.position.X, this.position.Y - 20), Color.Black);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 50, 50), color);
        }

        public void Update(GameTime gt, MouseState previousMouse, MouseState currentMouse, List<Unit> userSelectedUnits, List<Unit> enemyUnits)
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
            foreach (Unit u in enemyUnits)
            {
                AttackUnit(u, gt);
            }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}