using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class LightRay : BasePostProcess
    {
        public Vector2 mLighScreenSourcePos;
        public float Density = .5f;
        public float Decay = .95f;
        public float Weight = 1.0f;
        public float Exposure = .15f;

        public LightRay(GraphicsDevice _graphics, Vector2 _sourcePos, float _density, float _decay, float _weight, float _exposure, Effect _effect)
            : base(_graphics)
        {
            this.UsesVertexShader = true;
            this.mLighScreenSourcePos = _sourcePos;
            this.mEffect = _effect;
            Density = _density;
            Decay = _decay;
            Weight = _weight;
            Exposure = _exposure;
        }

        public override void Draw()
        {
            this.mEffect.CurrentTechnique = this.mEffect.Techniques["LightRayFX"];
            this.mEffect.Parameters["halfPixel"].SetValue(this.mHalfPixel);
            this.mEffect.Parameters["Density"].SetValue(Density);
            this.mEffect.Parameters["Decay"].SetValue(Decay);
            this.mEffect.Parameters["Weight"].SetValue(Weight);
            this.mEffect.Parameters["Exposure"].SetValue(Exposure);
            this.mEffect.Parameters["lightScreenPosition"].SetValue(this.mLighScreenSourcePos);
            base.Draw();
        }
    }
}
