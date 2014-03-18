using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class LightSourceMask : BasePostProcess
    {
        public Texture mLishsourceTexture;
        public Vector2 mLighScreenSourcePos;
        public float LightSize = 1500;

        public LightSourceMask(GraphicsDevice _graphics, Vector2 _sourcePos, float _lightSize, Effect _effect, Texture2D _texture)
            : base(_graphics)
        {
            this.UsesVertexShader = true;
            this.mLighScreenSourcePos = _sourcePos;
            this.mEffect = _effect;
            this.mLishsourceTexture = _texture;
            this.LightSize = _lightSize;
        }

        public override void Draw()
        {
            this.mEffect.Parameters["screenRes"].SetValue(new Vector2(16, 9));
            this.mEffect.Parameters["halfPixel"].SetValue(this.mHalfPixel);
            this.mEffect.CurrentTechnique = this.mEffect.Techniques["LightSourceMask"];
            this.mEffect.Parameters["flare"].SetValue(this.mLishsourceTexture);
            this.mEffect.Parameters["SunSize"].SetValue(this.LightSize);
            this.mEffect.Parameters["lightScreenPosition"].SetValue(this.mLighScreenSourcePos);
            base.Draw();
        }
    }
}
