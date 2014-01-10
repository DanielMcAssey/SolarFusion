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
                public bool isSingleplayer = false;
        public bool isMultiplayer = false;
        public bool isHidden = false;
        public bool inControl = false;
        AnimatedSprite playerAnimation;
        public string CharacterName = "";
        public float moveSpeed = 1f;
        public float jumpSpeed = 1f;
        public float jumpHeight = 10f;
        public float maxHeight = 0f;
        public float jumpDistance = 0f;
        public float originalJumpSpeed = 0f;
        public int jumpDirection = 0;
        public MoveDirection moveDirection = MoveDirection.Idle;
        public float floorHeight = 0f;
        public float originalFloorHeight = 0f;
        Vector2 position = Vector2.Zero;
        public bool isJumping = false;
        private EntityManager mObjectManager;
        private float Health = 100.0f;
        public bool isOnTop = false;
        public bool addGravity = false;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; playerAnimation.Position = position; }
        }

        public int Width
        {
            get { return playerAnimation.AnimationWidth; }
        }

        public int Height
        {
            get { return playerAnimation.AnimationHeight; }
        }

        public int PlayerScore
        {
            get { return this.Score; }
        }

        public float PlayerHealth
        {
            get { return this.Health; }
            set { this.Health = value; }
        }

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(Position.X - ((playerAnimation.AnimationWidth * playerAnimation.Scale) / 2f)), (int)(Position.Y - ((playerAnimation.AnimationHeight * playerAnimation.Scale) / 2f)), (int)(playerAnimation.AnimationWidth * playerAnimation.Scale), (int)(playerAnimation.AnimationHeight * playerAnimation.Scale)); }
        }

        public Player(uint id, AnimatedSprite spriteAnimation, Vector2 startPosition, float speed, float jHeight, float jSpeed, EntityManager objManager)
            : base(id)
        {
            playerAnimation = spriteAnimation;
            position = startPosition;
            moveSpeed = speed;
            jumpHeight = jHeight;
            jumpSpeed = jSpeed;
            mObjectManager = objManager;
        }

        public void moveLeft()
        {
            if (playerAnimation != null)
            {
                if (isJumping == false)
                    moveDirection = MoveDirection.Left;

                position.X -= moveSpeed;
                playerAnimation.CurrentAnimation = "left";
            }
        }

        public void moveRight()
        {
            if (playerAnimation != null)
            {
                if (isJumping == false && isOnTop == false)
                    moveDirection = MoveDirection.Right;
                
                position.X += moveSpeed;
                playerAnimation.CurrentAnimation = "right";
            }
        }

        public void moveIdle()
        {
            if (playerAnimation != null)
            {
                if (isJumping == false && isOnTop == false)
                {
                    moveDirection = MoveDirection.Idle;
                }

                playerAnimation.CurrentAnimation = "idle";
            }
        }

        public void jump()
        {
            if (playerAnimation != null)
            {
                if (position.Y == floorHeight && isJumping == false)
                {
                    originalJumpSpeed = jumpSpeed;
                    maxHeight = position.Y - jumpHeight;
                    isJumping = true;
                    moveDirection = MoveDirection.Jump;
                    playerAnimation.CurrentAnimation = "idle";
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isJumping)
            {
                jumpDistance = (float)gameTime.ElapsedGameTime.TotalSeconds * jumpSpeed;

                if (jumpDirection == 0)
                {
                    //Slow Up Speed
                }
                else
                {
                    jumpSpeed = jumpSpeed * 1.2f;
                }
                
                if (position.Y <= maxHeight || jumpDirection == 1)
                {
                    if (jumpDirection != 1)
                    {
                        jumpDirection = 1; //Falling Jump Direction
                    }
                    position.Y += jumpDistance;
                }
                else
                {
                    position.Y -= jumpDistance;
                }

                if (position.Y >= floorHeight)
                {
                    position.Y = floorHeight;
                    isJumping = false;
                    jumpDirection = 0;
                    jumpSpeed = originalJumpSpeed;
                }
            }

            if (addGravity == true)
            {
                jumpDistance = (float)gameTime.ElapsedGameTime.TotalSeconds * jumpSpeed;
                jumpSpeed = jumpSpeed * 1.2f;
                position.Y += jumpDistance;

                if (position.Y >= floorHeight)
                {
                    position.Y = floorHeight;
                    addGravity = false;
                    jumpDirection = 0;
                    jumpSpeed = originalJumpSpeed;
                }
            }

            if (playerAnimation != null)
            {
                playerAnimation.Position = position;
                playerAnimation.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (playerAnimation != null)
            {
                if (isHidden == false)
                {
                    playerAnimation.Draw(spriteBatch);
                }
            }
        }
    }
}
