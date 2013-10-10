using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Camera
{
    public class Camera2D
    {
        Viewport vp;
        private float camZoom = 1.0f;
        private Matrix camTransform;
        private Vector2 camPosition = Vector2.Zero;
        private float camRotation = 0.0f;
        private float camSpeed = 10.0f;

        public float Zoom
        {
            get { return camZoom; }
            set { camZoom = value; }
        }

        public Vector2 Position
        {
            get { return camPosition; }
            set { camPosition = value; }
        }

        public float Rotation
        {
            get { return camRotation; }
            set { camRotation = value; }
        }

        public float Speed
        {
            get { return camSpeed; }
            set { camSpeed = value; }
        }

        public Camera2D(Viewport vp)
        {
            this.vp = vp;
        }

        public Matrix calculateTransform()
        {
            camTransform = Matrix.CreateTranslation(new Vector3(-camPosition.X, -camPosition.Y, 0)) * Matrix.CreateRotationZ(camRotation) * Matrix.CreateScale(new Vector3(camZoom, camZoom, 1)) * Matrix.CreateTranslation(new Vector3(vp.Width * 0.5f, vp.Height * 0.5f, 0));
            return camTransform;
        }
    }
}
