using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreatGame
{/// <summary>
/// http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame
/// That is where I got the initial camera class
/// For some extra help with a couple world to global coordinate things I used the following link as a guide:
/// http://gamedev.stackexchange.com/questions/12442/mouse-location-is-off-due-to-camera
/// </summary>
    class Camera
    {
        // Fields
        private readonly Viewport _viewport;
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation
        private float _camSpeed;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;

            _rotation = 0;
            _zoom = 1f;
            //Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            // Position = Vector2.Zero;
            _pos = Vector2.Zero;
            _camSpeed = 5;
        }

        // Properties
        public Vector2 Pos { get { return _pos; } set { _pos = value; } }
        public float Zoom { get { return _zoom; } set { _zoom = value; } }
        public float Rotation { get { return _rotation; } set { _rotation = value; } }
        public float CamSpeed { get { return _camSpeed; } set { _camSpeed = value; } }



        public Matrix GetViewMatrix()
        {
            return
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X * _camSpeed, -_pos.Y * _camSpeed, 0)) *
              Matrix.CreateRotationZ(Rotation) *
              Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));

        }

        public Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return worldPosition - this.Pos;
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return screenPosition + this.Pos;
        }

    }

}