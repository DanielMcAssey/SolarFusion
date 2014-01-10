using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public abstract class Sprite
    {
        //Default Variables
        protected Texture2D mSpriteTexture;
        public Vector2 mSpritePosition = Vector2.Zero;
        protected Color mSpriteColor = Color.White;
        protected Vector2 mSpriteOrigin;
        protected float mSpriteRotation = 0f;
        protected float mSpriteScale = 1f;
        protected SpriteEffects mSpriteEffects;
        //!Default Variables
        
        //Animated Sprite
        protected Dictionary<string, Rectangle[]> mSpriteAnimations = new Dictionary<string, Rectangle[]>();
        protected Dictionary<string, int> mSpriteFramesCount = new Dictionary<string, int>();
        protected Dictionary<string, int> mSpriteFPS = new Dictionary<string, int>();
        protected int mFrameIndex = 0;
        public string mCurrentAnimation;
        protected int mAnimationFrames;
        private int mAnimationHeight;
        private int mAnimationWidth;
        protected bool IsAnimation = false;
        //!Animated Sprite

        // PROPERTIES
        public string CurrentAnimation
        {
            get { return this.mCurrentAnimation; }
            set { this.mCurrentAnimation = value; }
        }

        public Texture2D Texture
        {
            get { return mSpriteTexture; }
            set { mSpriteTexture = value; }
        }

        public Vector2 Position
        {
            get { return mSpritePosition; }
            set { mSpritePosition = value; }
        }

        public Color Colour
        {
            get { return mSpriteColor; }
            set { mSpriteColor = value; }
        }

        public Vector2 Origin
        {
            get { return mSpriteOrigin; }
            set { mSpriteOrigin = value; }
        }

        public float Rotation
        {
            get { return mSpriteRotation; }
            set { mSpriteRotation = value; }
        }

        public float Scale
        {
            get { return mSpriteScale; }
            set { mSpriteScale = value; }
        }

        public SpriteEffects Effects
        {
            get { return mSpriteEffects; }
            set { mSpriteEffects = value; }
        }

        public int Frame
        {
            get { return this.mFrameIndex; }
            set { this.mFrameIndex = value; }
        }

        public int AnimationHeight
        {
            get { return mAnimationHeight; }
        }

        public int AnimationWidth
        {
            get { return mAnimationWidth; }
        }
        //!PROPERTIES

        // FUNCTIONS

        public Sprite(Texture2D spriteTexture, int maxFrameCount, int animationCount)
        {
            mSpriteTexture = spriteTexture; //Assign the appropriate data.
            mAnimationFrames = maxFrameCount;
            mAnimationWidth = mSpriteTexture.Width / maxFrameCount;
            mAnimationHeight = mSpriteTexture.Height / animationCount;
        }

        public void AddAnimation(string animName, int animRow, int frameCount, int spriteFPS)
        {
            if (!IsAnimation)
                IsAnimation = true;

            Rectangle[] tmpRectangles = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                tmpRectangles[i] = new Rectangle(i * AnimationWidth, (animRow - 1) * AnimationHeight, AnimationWidth, AnimationHeight);

            mSpriteAnimations.Add(animName, tmpRectangles);
            mSpriteFramesCount.Add(animName, frameCount);
            mSpriteFPS.Add(animName, spriteFPS);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mFrameIndex > (mSpriteAnimations[mCurrentAnimation].Length - 1))
            {
                if (mSpriteTexture != null)
                    spriteBatch.Draw(mSpriteTexture, mSpritePosition, mSpriteAnimations[mCurrentAnimation][0], mSpriteColor, mSpriteRotation, mSpriteOrigin, mSpriteScale, mSpriteEffects, 0f);
            }
            else
            {
                if (mSpriteTexture != null)
                    spriteBatch.Draw(mSpriteTexture, mSpritePosition, mSpriteAnimations[mCurrentAnimation][mFrameIndex], mSpriteColor, mSpriteRotation, mSpriteOrigin, mSpriteScale, mSpriteEffects, 0f);
            }
        }
        //!FUNCTIONS
    }
}
