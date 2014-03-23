using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class Blast : GameObjects
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

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(this.mPosition.X - (mBlastTexture.Width / 2f)), (int)(this.mPosition.Y - (mBlastTexture.Height / 2f)), Blast.Texture.Width, Blast.Texture.Height); }
        }

        public Blast(uint id, Vector2 _position, float _maxLife, float _speed, float _rotation)
            : base(id)
        {
            this.mPosition = _position;
            this.mMaxLife = _maxLife;
            this.mSpeed = _speed;
            this.mRotation = _rotation;
            this.ObjectType = ObjectType.Bullet;
        }

        public static void Load(ContentManager _content)
        {
            mBlastTexture = _content.Load<Texture2D>("Sprites/Objects/Static/blast/sprite");
            mBlastTextureOrigin = new Vector2(mBlastTexture.Width / 2f, mBlastTexture.Height / 2f);
            mScale = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.mDelete)
            {
                this.mPosition.X += this.mSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.mMaxLife -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.mMaxLife <= 0)
                    this.mDelete = true;
            }
        }

        public override void Draw(SpriteBatch _sb)
        {
            if (!this.mDelete || !this.Hidden)
                _sb.Draw(mBlastTexture, this.mPosition, null, Color.White, this.mRotation, mBlastTextureOrigin, mScale, SpriteEffects.None, 0f);
        }
    }
}
