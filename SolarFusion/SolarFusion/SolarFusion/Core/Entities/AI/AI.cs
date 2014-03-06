using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public enum EnemyType
    {
        MercBot
    }

    public abstract class AI : GameObjects
    {
        public float Health = 1;
        public float Speed = 5f;
        public AnimatedSprite animation;
        public MoveDirection moveDirection = MoveDirection.Left;

        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(animation.Position.X - ((animation.AnimationWidth * animation.Scale) / 2f)), (int)(animation.Position.Y - ((animation.AnimationHeight * animation.Scale) / 2f)), (int)(animation.AnimationWidth * animation.Scale), (int)(animation.AnimationHeight * animation.Scale)); }
        }

        public Vector2 Position
        {
            get { return animation.Position; }
            set { animation.Position = value; }
        }

        public AI(uint id) : base(id) { }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, 1f);
        }

        public void moveLeft()
        {
            if (animation != null)
            {
                moveDirection = MoveDirection.Left;

                animation.mSpritePosition.X -= this.Speed;
                animation.CurrentAnimation = "left";
            }
        }

        public void moveRight()
        {
            if (animation != null)
            {
                moveDirection = MoveDirection.Right;

                animation.mSpritePosition.X += this.Speed;
                animation.CurrentAnimation = "right";
            }
        }
    }
}
