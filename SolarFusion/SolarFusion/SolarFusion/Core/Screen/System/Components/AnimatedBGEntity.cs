using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    public class AnimatedBGEntity
    {
        protected AnimatedSprite baseAnimation = null;
        protected float speedX = 0f;
        protected float speedY = 0f;
        protected int directionX = 1;
        protected int directionY = 1;
        protected int rotDirection = 0;
        protected float rotSpeed = 0.01f;

        public AnimatedBGEntity(Texture2D spriteTexture, int frameCount, int animCount, float initRotation, Vector2 initPosition, int initFrame, int fps, float xspeed, float yspeed, int dirX, int dirY, int rotDir, float rotSpd)
        {
            this.speedX = xspeed;
            this.speedY = yspeed;
            this.directionX = dirX;
            this.directionY = dirY;
            this.rotDirection = rotDir;
            this.rotSpeed = rotSpd;
            baseAnimation = new AnimatedSprite(spriteTexture, frameCount, animCount);
            baseAnimation.Rotation = initRotation;
            baseAnimation.Position = initPosition;
            baseAnimation.Origin = new Vector2(baseAnimation.AnimationWidth / 2f, baseAnimation.AnimationHeight / 2f);
            baseAnimation.AddAnimation("DEFAULT", 1, frameCount, fps);
            baseAnimation.Frame = initFrame;
            baseAnimation.mCurrentAnimation = "DEFAULT";
            baseAnimation.Loop = true;
        }

        public float GetSpeedX
        {
            get { return speedX; }
        }

        public float GetSpeedY
        {
            get { return speedY; }
        }

        public int DirectionX
        {
            get { return directionX; }
            set { directionX = value; }
        }

        public int DirectionY
        {
            get { return directionY; }
            set { directionY = value; }
        }

        public int RotationDirection
        {
            get { return this.rotDirection; }
            set { this.rotDirection = value; }
        }

        public float RotationSpeed
        {
            get { return this.rotSpeed; }
            set { this.rotSpeed = value; }
        }

        public AnimatedSprite Animation
        {
            get { return baseAnimation; }
            set { baseAnimation = value; }
        }

        public void Update(GameTime gt)
        {
            baseAnimation.Update(gt);
        }

        public void Draw(SpriteBatch sb, float alpha)
        {
            baseAnimation.Draw(sb, alpha);
        }
    }
}
