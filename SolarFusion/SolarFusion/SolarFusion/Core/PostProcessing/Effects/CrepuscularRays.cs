using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class CrepuscularRays : BasePostProcessEffect
    {
        public LightSourceMask mLightSourceMask;
        public LightRay mLightRays;

        #region "Properties"
        public Vector2 LightSource
        {
            set
            {
                this.mLightSourceMask.mLighScreenSourcePos = value;
                this.mLightRays.mLighScreenSourcePos = value;
            }
            get
            {
                return this.mLightRays.mLighScreenSourcePos;
            }
        }

        public Texture LightTexture
        {
            get { return this.mLightSourceMask.mLishsourceTexture; }
            set { this.mLightSourceMask.mLishsourceTexture = value; }
        }

        public float LightSourceSize
        {
            set { this.mLightSourceMask.LightSize = value; }
            get { return this.mLightSourceMask.LightSize; }
        }

        public float Density
        {
            get { return this.mLightRays.Density; }
            set { this.mLightRays.Density = value; }
        }

        public float Decay
        {
            get { return this.mLightRays.Decay; }
            set { this.mLightRays.Decay = value; }
        }

        public float Weight
        {
            get { return this.mLightRays.Weight; }
            set { this.mLightRays.Weight = value; }
        }

        public float Exposure
        {
            get { return this.mLightRays.Exposure; }
            set { this.mLightRays.Exposure = value; }
        }
        #endregion

        public CrepuscularRays(GraphicsDevice _graphics, Vector2 _lightScreenSourcePos, float _lightSourceSize, float _density, float _decay, float _weight, float _exposure, Effect _lightSourceEffect, Texture2D _lightSourceTexture, Effect _lightRayEffect)
            : base(_graphics)
        {
            this.mLightSourceMask = new LightSourceMask(_graphics, _lightScreenSourcePos, _lightSourceSize, _lightSourceEffect, _lightSourceTexture);
            this.mLightRays = new LightRay(_graphics, _lightScreenSourcePos, _density, _decay, _weight, _exposure, _lightRayEffect);
            AddPostProcess(this.mLightSourceMask);
            AddPostProcess(this.mLightRays);
        }
    }
}
