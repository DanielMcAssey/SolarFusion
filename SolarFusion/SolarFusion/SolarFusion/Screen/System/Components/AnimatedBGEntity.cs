using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SolarFusion.Core;
using SolarFusion.Screen.System;

namespace SolarFusion.Screen.System
{
    public class AnimatedBGEntity
    {
        protected AnimatedSprite baseAnimation = null;
        protected float speedX = 0f;
        protected float speedY = 0f;
        protected int directionX = 1;
        protected int directionY = 1;

        public AnimatedBGEntity(Texture2D spriteTexture, int frameCount, int animCount, float initRotation, Vector2 initPosition, int initFrame, int fps, float xspeed, float yspeed, int dirX, int dirY)
        {
            this.speedX = xspeed;
            this.speedY = yspeed;
            this.directionX = dirX;
            this.directionY = dirY;
            baseAnimation = new AnimatedSprite(spriteTexture, frameCount, animCount);
            baseAnimation.Rotation = initRotation;
            baseAnimation.Position = initPosition;
            baseAnimation.Origin = new Vector2(baseAnimation.AnimationWidth / 2f, baseAnimation.AnimationHeight / 2f);
            baseAnimation.AddAnimation("DEFAULT", 1, frameCount, fps);
            baseAnimation.Frame = initFrame;
            baseAnimation.mCurrentAnimation = "DEFAULT";
            baseAnimation.IsLoopAnimation = true;
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

        public AnimatedSprite Animation
        {
            get { return baseAnimation; }
            set { baseAnimation = value; }
        }

        public void Update(GameTime gt)
        {
            baseAnimation.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            baseAnimation.Draw(sb);
        }
    }
}
