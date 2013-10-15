using System;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SolarFusion.Core;
using SolarFusion.Input;
using SolarFusion.Screen.System;

namespace SolarFusion.Screen.System
{
    public enum ScreenMode
    {
        MODE_TRANSITION_ON,
        MODE_ACTIVE,
        MODE_TRANSITION_OFF,
        MODE_TRANSITION_HIDDEN,
    }

    public enum TransitionDirection
    {
        POSITIVE = 1,
        NEGATIVE = -1,
    }

    public abstract class BaseScreen
    {
        //---------------CLASS MEMBERS-------------------------------------------------------------
        protected ScreenManager _screen_manager = null;
        protected ContentManager _local_content = null;
        protected TimeSpan _trans_on_time = TimeSpan.Zero;
        protected TimeSpan _trans_off_time = TimeSpan.Zero;
        protected ScreenMode _screen_mode = ScreenMode.MODE_TRANSITION_ON;
        protected PlayerIndex? _primary_player = null;
        protected float _trans_position = 1;
        protected bool _is_exiting = false;
        protected bool _is_popup = false;
        protected bool _not_primary = false;
        protected String _screen_name = String.Empty;

        //---------------PROPERTIES----------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return _trans_on_time; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return _trans_off_time; }
        }


        /// <summary>
        /// 
        /// </summary>
        public float CurrentTransitionPos
        {
            get { return _trans_position; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float CurrentTransitionAlpha
        {
            get { return 1f - CurrentTransitionPos; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ScreenMode CurrentScreenMode
        {
            get { return _screen_mode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExiting
        {
            get { return _is_exiting; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPopupScreen
        {
            get { return _is_popup; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !_not_primary && (_screen_mode == ScreenMode.MODE_TRANSITION_ON || _screen_mode == ScreenMode.MODE_ACTIVE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return _screen_manager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return _primary_player; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ScreenName
        {
            get { return this._screen_name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ContentManager LocalContentManager
        {
            get { return this._local_content; }
        }

        //------------------LINK PROPERTY HELPERS-----------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public GameTime GlobalGameTimer
        {
            get { return this.ScreenManager.Timer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ContentManager GlobalContentManager
        {
            get { return this.ScreenManager.ContentManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public InputManager GlobalInput
        {
            get { return this.ScreenManager.InputManager; }
        }


        //------------------PUBLIC METHODS------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="potherfocused"></param>
        /// <param name="poverlaid"></param>
        public virtual void masterUpdate(bool potherfocused, bool poverlaid)
        {
            this._not_primary = potherfocused;

            if (this._is_exiting)
            {
                this._screen_mode = ScreenMode.MODE_TRANSITION_OFF;
                if (!internUpdateTrans(this._trans_off_time, TransitionDirection.POSITIVE))
                {
                    ScreenManager.removeScreen(this);
                }
            }
            else if (poverlaid)
            {
                if (internUpdateTrans(this._trans_off_time, TransitionDirection.POSITIVE))
                    this._screen_mode = ScreenMode.MODE_TRANSITION_OFF;
                else
                    this._screen_mode = ScreenMode.MODE_TRANSITION_HIDDEN;
            }
            else
            {
                if (internUpdateTrans(this._trans_on_time, TransitionDirection.NEGATIVE))
                    this._screen_mode = ScreenMode.MODE_TRANSITION_ON;
                else
                    this._screen_mode = ScreenMode.MODE_ACTIVE;
            }

            this.bgUpdate(potherfocused, poverlaid);
        }


        /// <summary>
        /// 
        /// </summary>
        public void exitScreen()
        {
            if (_trans_off_time == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.removeScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                _is_exiting = true;
            }
        }

        public void setScreenManager(ScreenManager mScreenManager, PlayerIndex? mPlayer)
        {
            this._screen_manager = mScreenManager;
            this._is_exiting = false;
            this._primary_player = mPlayer;
        }

        //-------------------ABSTRACT METHOD DEFINITIONS-------------------------------------------------------------

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public abstract void loadContent();

        /// <summary>
        /// Background Update Method.
        /// </summary>
        /// <param name="potherfocused"></param>
        /// <param name="poverlaid"></param>
        public abstract void bgUpdate(bool potherfocused, bool poverlaid);


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public abstract void update();

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public abstract void render();

        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public abstract void unloadContent();

        //------------------PRIVATE/PROTECTED METHODS---------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptranstime"></param>
        /// <param name="pdir"></param>
        /// <returns></returns>
        bool internUpdateTrans(TimeSpan ptranstime, TransitionDirection pdir)
        {
            float ttransdelta;
            if (ptranstime == TimeSpan.Zero)
                ttransdelta = 1;
            else
                ttransdelta = (float)(this.GlobalGameTimer.ElapsedGameTime.TotalMilliseconds / ptranstime.TotalMilliseconds);
            this._trans_position += ttransdelta * (int)pdir;
            if ((pdir.Equals(TransitionDirection.NEGATIVE) && _trans_position <= 0) || (pdir.Equals(TransitionDirection.POSITIVE) && _trans_position >= 1))
            {
                this._trans_position = MathHelper.Clamp(_trans_position, 0, 1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void internCreateLocalContent()
        {
            this._local_content = new ContentManager(ScreenManager.Game.Services, SysConfig.CONFIG_CONTENT_ROOT);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void internResetRenderStatesFor3D()
        {
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void internResetRenderStatesFor2D()
        {
            ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.None;
        }
    }
}
