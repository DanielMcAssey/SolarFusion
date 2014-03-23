using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class BasePostProcessEffect
    {
        protected List<BasePostProcess> _obj_postprocesses = new List<BasePostProcess>();
        protected GraphicsDevice _obj_graphics;

        public Vector2 mHalfPixel;
        public Texture2D mLastScene;
        public Texture2D mOriginalScene;

        public bool Enabled = true;

        public BasePostProcessEffect(GraphicsDevice _graphics)
        {
            this._obj_graphics = _graphics;
        }

        public void AddPostProcess(BasePostProcess _postProcess)
        {
            this._obj_postprocesses.Add(_postProcess);
        }

        public virtual void Draw(Texture2D _scene)
        {
            if (!Enabled)
                return;

            this.mOriginalScene = _scene;
            int maxProcess = this._obj_postprocesses.Count;
            this.mLastScene = null;

            for (int p = 0; p < maxProcess; p++)
            {
                if (this._obj_postprocesses[p].Enabled)
                {
                    this._obj_postprocesses[p].mHalfPixel = this.mHalfPixel;
                    this._obj_postprocesses[p].mOriginalBuffer = this.mOriginalScene;

                    if (this._obj_postprocesses[p].mSceneTarget == null)
                        this._obj_postprocesses[p].mSceneTarget = new RenderTarget2D(this._obj_graphics, this._obj_graphics.Viewport.Width / 2, this._obj_graphics.Viewport.Height / 2, false, SurfaceFormat.Color, DepthFormat.None);

                    this._obj_graphics.SetRenderTarget(this._obj_postprocesses[p].mSceneTarget);

                    if (this.mLastScene == null)
                        this.mLastScene = this.mOriginalScene;

                    this._obj_postprocesses[p].mBackBuffer = this.mLastScene;
                    this._obj_graphics.Textures[0] = this._obj_postprocesses[p].mBackBuffer;
                    this._obj_postprocesses[p].Draw();
                    this._obj_graphics.SetRenderTarget(null);
                    this.mLastScene = this._obj_postprocesses[p].mSceneTarget;
                }
            }

            if (this.mLastScene == null)
                this.mLastScene = _scene;
        }
    }
}
