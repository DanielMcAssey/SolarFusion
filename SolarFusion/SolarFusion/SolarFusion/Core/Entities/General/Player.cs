using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class Player : GameObjects
    {
        protected EntityManager _obj_entitymanager;

        public bool isSingleplayer = false;
        public bool isMultiplayer = false;
        public bool inControl = false;
        public bool isGemCollected = false;
        public bool isJumping = false;
        public bool isUpdateGravity = false;

        protected string mCharacterName = "";
        protected float mMoveSpeed = 1f;
        protected MoveDirection mMoveDirection = MoveDirection.Idle;
        protected float mFloorHeight = 0f;
        protected float mOriginalFloorHeight = 0f;

        protected Vector2 mPosition = Vector2.Zero;
        protected Vector2 mVelocity = Vector2.Zero;
        protected AnimatedSprite mAnimation;
        protected float mHealth = 100.0f;
        protected float mJumpSpeed = 1f;

        #region "Properties"
        public String CharacterName
        {
            get { return this.mCharacterName; }
            set { this.mCharacterName = value; }
        }

        public Vector2 Position
        {
            get { return this.mPosition; }
            set { this.mPosition = value; this.mAnimation.Position = this.mPosition; }
        }

        public Vector2 Velocity
        {
            get { return this.mVelocity; }
            set { this.mVelocity = value; }
        }

        public int Width
        {
            get { return this.mAnimation.AnimationWidth; }
        }

        public int Height
        {
            get { return this.mAnimation.AnimationHeight; }
        }

        public int PlayerScore
        {
            get { return this.Score; }
        }

        public float PlayerHealth
        {
            get { return this.mHealth; }
            set { this.mHealth = value; }
        }

        public float JumpHeight
        {
            get { return this.mFloorHeight; }
        }

        public float OriginalJumpHeight
        {
            get { return this.mOriginalFloorHeight; }
        }

        public AnimatedSprite PlayerAnimation
        {
            get { return this.mAnimation; }
            set { this.mAnimation = value; }
        }

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(Position.X - ((this.mAnimation.AnimationWidth * this.mAnimation.Scale) / 2f)), (int)(Position.Y - (this.mAnimation.AnimationHeight * this.mAnimation.Scale)), (int)(this.mAnimation.AnimationWidth * this.mAnimation.Scale), (int)(this.mAnimation.AnimationHeight * this.mAnimation.Scale)); }
        }
        #endregion

        public Player(uint id, AnimatedSprite _animation, Vector2 _position, float _speed, float _jumpSpeed, EntityManager _entityManager)
            : base(id)
        {
            this._obj_entitymanager = _entityManager;
            this.mAnimation = _animation;
            this.mPosition = _position;
            this.mMoveSpeed = _speed;
            this.mJumpSpeed = _jumpSpeed;
            this.ObjectType = ObjectType.Player;
        }

        public void SetFloorHeight(float _floorHeight)
        {
            if (this.mOriginalFloorHeight == 0f)
                this.mOriginalFloorHeight = _floorHeight;

            this.mFloorHeight = _floorHeight;
        }

        public void moveLeft()
        {
            if (this.mAnimation != null)
            {
                if (isJumping == false)
                    this.mMoveDirection = MoveDirection.Left;

                this.mPosition.X -= this.mMoveSpeed;
                this.mAnimation.CurrentAnimation = "left";
            }
        }

        public void moveRight()
        {
            if (this.mAnimation != null)
            {
                if (this.isJumping == false)
                    this.mMoveDirection = MoveDirection.Right;

                this.mPosition.X += this.mMoveSpeed;
                this.mAnimation.CurrentAnimation = "right";
            }
        }

        public void moveIdle()
        {
            if (this.mAnimation != null)
            {
                if (this.isJumping == false)
                    this.mMoveDirection = MoveDirection.Idle;

                this.mAnimation.CurrentAnimation = "idle";
            }
        }

        public void fire()
        {
            if (this.Score >= 2)
            {
                switch (this.mMoveDirection)
                {
                    case MoveDirection.Left:
                        this._obj_entitymanager.CreateBullet(new Blast(this._obj_entitymanager.NextID(), new Vector2(this.Position.X - 30, this.Position.Y - (this.PlayerAnimation.AnimationHeight / 2f)), 2f, -250f, 1.5f));
                        break;
                    case MoveDirection.Right:
                        this._obj_entitymanager.CreateBullet(new Blast(this._obj_entitymanager.NextID(), new Vector2(this.Position.X + 30, this.Position.Y - (this.PlayerAnimation.AnimationHeight / 2f)), 2f, 250f, -1.5f));
                        break;
                    default:
                        this._obj_entitymanager.CreateBullet(new Blast(this._obj_entitymanager.NextID(), new Vector2(this.Position.X + 30, this.Position.Y - (this.PlayerAnimation.AnimationHeight / 2f)), 2f, 250f, -1.5f));
                        break;
                }
                this.Score -= 2;
            }
        }

        public void jump()
        {
            if (this.mAnimation != null)
                if (this.isJumping == false) //Only allow 1 Jump
                {
                    this.mPosition.Y -= this.mJumpSpeed;
                    this.isJumping = true;
                    this.mAnimation.CurrentAnimation = "idle";
                }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.mPosition.Y >= this.mFloorHeight && (this.isJumping || this.isUpdateGravity))
            {
                this.isJumping = false;
                this.mPosition.Y = this.mFloorHeight;
                this.mVelocity.Y = 0f;
            }

            if (this.isJumping || this.isUpdateGravity)
            {
                this.mVelocity += this._obj_entitymanager.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.mPosition -= this.mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.isUpdateGravity = false;
            }

            //Update Animation
            if (this.mAnimation != null)
            {
                this.mAnimation.Position = this.mPosition;
                this.mAnimation.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.mAnimation != null)
                if (this.Hidden == false)
                    this.mAnimation.Draw(spriteBatch, 1f);
        }
    }
}
