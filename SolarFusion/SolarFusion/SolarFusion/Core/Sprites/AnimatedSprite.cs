﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public class AnimatedSprite : Sprite
    {
        private float mTimeElapsed;
        public bool IsLoopAnimation = false;
        private float mTimeToUpdate = 0.05f;

        public int FramesPerSecond
        {
            set { mTimeToUpdate = (1f / value); }
        }

        public AnimatedSprite(Texture2D spriteTexture, int frameCount, int animCount)
            : base(spriteTexture, frameCount, animCount)
        {
        }

        public void Update(GameTime gameTime)
        {
            mTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeToUpdate = (1f / mSpriteFPS[mCurrentAnimation]);

            if (mTimeElapsed > mTimeToUpdate)
            {
                mTimeElapsed -= mTimeToUpdate;

                if (IsAnimation == true)
                {
                    if (mFrameIndex < (mSpriteFramesCount[mCurrentAnimation] - 1))
                    {
                        mFrameIndex++;
                    }
                    else if (IsLoopAnimation == true)
                    {
                        mFrameIndex = 0;
                    }
                }
                else
                {
                    if (mFrameIndex < (mAnimationFrames - 1))
                    {
                        mFrameIndex++;
                    }
                    else if (IsLoopAnimation == true)
                    {
                        mFrameIndex = 0;
                    }
                }
            }
        }
    }
}
