using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    class Blast
    {
        protected static Texture2D mBlastTexture;
        protected static Vector2 mBlastTextureOrigin;
        protected static float mScale;
        protected Vector2 mPosition;
        protected float mMaxLife;
        protected float mSpeed;
        protected float mRotation;
        protected bool mDelete = false;

        public bool ForDeletion
        {
            get { return this.mDelete; }
        }

        public Vector2 Position
        {
            get { return this.mPosition; }
        }

        public static Texture2D Texture
        {
            get { return mBlastTexture; }
        }

        public Blast(Vector2 _position, float _maxLife, float _speed, float _rotation)
        {
            this.mPosition = _position;
            this.mMaxLife = _maxLife;
            this.mSpeed = _speed;
            this.mRotation = _rotation;
        }

        public static void Load(ContentManager _content)
        {
            mBlastTexture = _content.Load<Texture2D>("Sprites/Objects/Static/blast/sprite");
            mBlastTextureOrigin = new Vector2(mBlastTexture.Width / 2f, mBlastTexture.Height / 2f);
            mScale = 1f;
        }

        public void Update(float _elapsedTime)
        {
            if (!this.mDelete)
            {
                this.mPosition.X += this.mSpeed * _elapsedTime;
                this.mMaxLife -= _elapsedTime;
                if (this.mMaxLife <= 0)
                    this.mDelete = true;
            }
        }

        public void Draw(SpriteBatch _sb)
        {
            if (!this.mDelete)
                _sb.Draw(mBlastTexture, this.mPosition, null, Color.White, this.mRotation, mBlastTextureOrigin, mScale, SpriteEffects.None, 0f);
        }
    }
}
