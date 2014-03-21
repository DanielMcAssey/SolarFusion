using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    public class GameGUI
    {
        protected GraphicsDevice _obj_graphics = null;

        #region "Properties"
        protected int mAmmo = 0;
        protected float mHealth = 0.0f;
        protected float mTime = 0.0f;
        protected bool mCrystal = false;

        public int Ammo
        {
            get { return this.mAmmo; }
            set { this.mAmmo = value; }
        }

        public float Health
        {
            get { return this.mHealth; }
            set { this.mHealth = value; }
        }

        public float Time
        {
            get { return this.mTime; }
            set { this.mTime = value; }
        }

        public bool Crystal
        {
            get { return this.mCrystal; }
            set { this.mCrystal = value; }
        }
        #endregion

        public GameGUI(GraphicsDevice _graphics)
        {
            this._obj_graphics = _graphics;
        }

        public void Load(ContentManager _content)
        {

        }

        public void Unload()
        {
            this._obj_graphics = null;
        }

        public void Update(GameTime _elapsedTime)
        {

        }

        public void Draw(SpriteBatch _sb)
        {

        }
    }
}
