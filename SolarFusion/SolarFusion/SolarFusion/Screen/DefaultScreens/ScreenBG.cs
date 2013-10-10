using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SolarFusion.Core;
using SolarFusion.Screen.System;

namespace SolarFusion.Screen.DefaultScreens
{
    class ScreenBG : BaseScreen
    {
        public float DEFAULT_TRANS_TIME = 0.5f;

        ContentManager _content;
        Texture2D _bg_texture;

        public ScreenBG()
        {
            this._trans_on_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
            this._trans_off_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
        }

        public float TransitionTime
        {
            get { return this.DEFAULT_TRANS_TIME; }
            set { DEFAULT_TRANS_TIME = value; }
        }

        public override void loadContent()
        {
            if (this._content == null)
                this._content = new ContentManager(ScreenManager.Game.Services, SysConfig.CONFIG_CONTENT_ROOT);
            this._bg_texture = this._content.Load<Texture2D>("System/UI/Backgrounds/bg");
        }

        /// <summary>
        /// Master Update - Guarantee this screen is never transitioned, even if overlaid.
        /// </summary>
        /// <param name="potherfocused">True if another screen is topmost</param>
        /// <param name="poverlaid">True if overlaid by an overlay screen</param>
        public override void masterUpdate(bool potherfocused, bool poverlaid)
        {
            base.masterUpdate(potherfocused, false);
        }

        /// <summary>
        /// Default Implementation of the Background Update
        /// </summary>
        /// <param name="potherfocused">True if another screen is topmost - this is overriden to false</param>
        /// <param name="poverlaid">True if overlaid by an overlay screen</param>
        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            //Nothing to update since this is a static screen
        }

        /// <summary>
        /// Update the State of the Screen
        /// </summary>
        public override void update()
        {
            //Nothing to update since this is a static screen
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void render()
        {
            //Draw the Background Image using the spritebatch system
            SpriteBatch mSpriteBatch = ScreenManager.SpriteBatch;
            Viewport mViewport = ScreenManager.GameViewport;

            mSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            mSpriteBatch.Draw(this._bg_texture, Vector2.Zero, new Rectangle(0, 0, mViewport.Width, mViewport.Height), new Color(CurrentTransitionAlpha, CurrentTransitionAlpha, CurrentTransitionAlpha), 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            mSpriteBatch.End();
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void unloadContent()
        {
            this._content.Unload();
        }
    }
}
