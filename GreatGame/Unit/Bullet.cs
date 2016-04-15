using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatGame
{
    class Bullet
    {
        enum Alignment
        {
            Player,
            Enemy
        }
        private double speed, damage, range;
        int size;
        private Vector2 position;
        private Vector2 center;
        private Vector2 destination;
        private Vector2 startingLocation;
        private BoundingSphere bounds;
        private Texture2D texture;
        private bool toDelete;

        public Bullet(double speed, double damage, double range, int size, Texture2D texture)
        {
            this.speed = speed;
            this.damage = damage;
            this.range = range;
            this.size = size;
            this.texture = texture;
            this.bounds = new BoundingSphere(new Vector3(position.X, position.Y, 0), (float)size);
            this.center = new Vector2(position.X + size / 2, position.Y + size / 2);
        }

        public double Range
        {
            get { return range; }
            set { Range = value; }
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

        public Vector2 Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public Vector2 Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }

        public Vector2 StartingLocation
        {
            get { return startingLocation; }
            set { startingLocation = value; }
        }

        public BoundingSphere Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public bool ToDelete
        {
            get { return toDelete; }
            set { toDelete = value; }
        }

        public void Move()
        {
            Vector2 distance = new Vector2(destination.X - position.X - (int)size / 2, destination.Y - position.Y - (int)size / 2);
            distance.Normalize();
            Vector2 toMove = new Vector2((int)(distance.X * speed), (int)(distance.Y * speed));
            position += toMove;
            bounds.Center += new Vector3(toMove.X, toMove.Y, 0);
            DistanceCheck();
        }

        public void DistanceCheck()
        {
            Vector2 distance = new Vector2(startingLocation.X - position.X - (int)size / 2, startingLocation.Y - position.Y - (int)size / 2);
            if (distance.Length() > range)
            {
                toDelete = true;
            }
        }

        public void DamageCheck(Unit u)
        {
            Vector2 distance = new Vector2(position.X - u.Position.X - u.Size / 2, position.Y - u.Position.Y - u.Size / 2);
            if (distance.Length() < size)
            {
                u.Health -= damage;
                toDelete = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, size, size), Color.White);
        }
    }
}