using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class BasePostProcess
    {
        protected GraphicsDevice _obj_graphics;
        protected Effect mEffect;
        protected ScreenQuad mScreenQuad;

        public Vector2 mHalfPixel;
        public Texture2D mBackBuffer;
        public Texture2D mOriginalBuffer;
        public RenderTarget2D mSceneTarget;
        
        public bool Enabled = true;
        public bool UsesVertexShader = false;

        public BasePostProcess(GraphicsDevice _graphics)
        {
            this._obj_graphics = _graphics;
        }

        public virtual void Draw()
        {
            if (Enabled)
            {
                if (this.mScreenQuad == null)
                {
                    this.mScreenQuad = new ScreenQuad(this._obj_graphics);
                    this.mScreenQuad.Initialize();
                }

                this.mEffect.CurrentTechnique.Passes[0].Apply();
                this.mScreenQuad.Draw();
            }
        }
    }
}
