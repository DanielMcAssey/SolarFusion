using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class PostProcessingManager
    {
        protected GraphicsDevice _obj_graphics;
        protected List<BasePostProcessEffect> _obj_postProcessEffects = new List<BasePostProcessEffect>();

        public Texture2D mScene;
        public Vector2 mHalfPixel;

        public PostProcessingManager(GraphicsDevice _graphics)
        {
            this._obj_graphics = _graphics;
        }

        public void AddEffect(BasePostProcessEffect _ppEffect)
        {
            this._obj_postProcessEffects.Add(_ppEffect);
        }

        public virtual void Draw(SpriteBatch _sb, Texture2D _scene)
        {
            this.mHalfPixel = -new Vector2(.5f / (float)this._obj_graphics.Viewport.Width, .5f / (float)this._obj_graphics.Viewport.Height);
            int maxEffect = this._obj_postProcessEffects.Count;

            this.mScene = _scene;

            for (int e = 0; e < maxEffect; e++)
            {
                if (this._obj_postProcessEffects[e].Enabled)
                {
                    this._obj_postProcessEffects[e].mHalfPixel = this.mHalfPixel;

                    this._obj_postProcessEffects[e].mOriginalScene = _scene;
                    this._obj_postProcessEffects[e].Draw(this.mScene);
                    this.mScene = this._obj_postProcessEffects[e].mLastScene;
                }
            }

            _sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _sb.Draw(this.mScene, new Rectangle(0, 0, this._obj_graphics.Viewport.Width, this._obj_graphics.Viewport.Height), Color.White);
            _sb.End();
        }

        protected void SaveTexture(Texture2D texture, string name)
        {
            FileStream stream = new FileStream(name, FileMode.Create);
            texture.SaveAsJpeg(stream, texture.Width, texture.Height);
            stream.Close();
        }
    }
}
