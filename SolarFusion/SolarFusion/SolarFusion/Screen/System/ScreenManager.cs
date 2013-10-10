using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using SolarFusion.Input;
using SolarFusion.Core;

namespace SolarFusion.Screen.System
{
    public class ScreenManager : DrawableGameComponent
    {
        List<BaseScreen>    _list_screens_master    = new List<BaseScreen>();
        List<BaseScreen>    _list_screens_updating  = new List<BaseScreen>();
        ContentManager      _obj_content            = null;
        InputManager        _obj_input              = null;
        GameTime            _obj_timer              = null;
        SpriteBatch         _obj_sprite_batch       = null;
        SpriteFont          _obj_default_guifont    = null;
        SpriteFont          _obj_default_gamefont   = null;
        SpriteFont          _obj_default_debugfont  = null;
        Texture2D           _obj_tex_blank          = null;
        bool                _is_init                = false;


        public ScreenManager(Game mGame)
            : base(mGame)
        {

        }

        /// <summary>
        /// Get Access to the Global Content Manager
        /// </summary>
        public ContentManager ContentManager
        {
            get { return this._obj_content; }
        }

        /// <summary>
        /// Get Access to the Global Input Manager
        /// </summary>
        public InputManager InputManager
        {
            get { return this._obj_input; }
        }

        /// <summary>
        /// Get Access to the Global Timer
        /// </summary>
        public GameTime Timer
        {
            get { return this._obj_timer; }
        }

        /// <summary>
        /// Get Access to the Global Spritebatch
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return this._obj_sprite_batch; }
        }

        /// <summary>
        /// Get Access to the Default GUI Font.
        /// </summary>
        public SpriteFont DefaultGUIFont
        {
            get { return this._obj_default_guifont; }
        }

        /// <summary>
        /// Get Access to the Default HUD Font.
        /// </summary>
        public SpriteFont DefaultHUDFont
        {
            get { return this._obj_default_gamefont; }
        }

        /// <summary>
        /// Get Access to the Default Debug Font
        /// </summary>
        public SpriteFont DefaultDebugFont
        {
            get { return this._obj_default_debugfont; }
        }

        /// <summary>
        /// Return True if the Screen Manager has been
        /// Initialised, false otherwise.
        /// </summary>
        public bool Initialised
        {
            get { return this._is_init; }
        }

        //------------------------PROPERTY SHORTCUTS-------------------------------------------------------------

        /// <summary>
        /// Get Access to the Game's Viewport
        /// </summary>
        public Viewport GameViewport
        {
            get { return this.Game.GraphicsDevice.Viewport; }
        }

        /// <summary>
        /// Get Access to the Game Service Manager Container.
        /// </summary>
        public GameServiceContainer GameServiceManager
        {
            get { return this.Game.Services; }
        }

        /// <summary>
        /// Get Access to the Game Components.
        /// </summary>
        public GameComponentCollection GameComponents
        {
            get { return this.Game.Components; }
        }

        /// <summary>
        /// Get Access to the Presentation Parameters.
        /// </summary>
        public PresentationParameters GamePresentationParameters
        {
            get { return this.Game.GraphicsDevice.PresentationParameters; }
        }

        /// <summary>
        /// Get Access to the Graphics Profile.
        /// </summary>
        public GraphicsProfile GameGraphicsProfile
        {
            get { return this.Game.GraphicsDevice.GraphicsProfile; }
        }

        /// <summary>
        /// Get Access to the Status of the Graphics Device.
        /// </summary>
        public GraphicsDeviceStatus GameGraphicsDeviceStatus
        {
            get { return this.Game.GraphicsDevice.GraphicsDeviceStatus; }
        }

        /// <summary>
        /// Get Access to the Graphics Adapter.
        /// </summary>
        public GraphicsAdapter GameGraphicsAdapter
        {
            get { return this.Game.GraphicsDevice.Adapter; }
        }

        /// <summary>
        /// Return's True if the Game class is active, false otherwise.
        /// </summary>
        public bool GameIsActive
        {
            get { return this.Game.IsActive; }
        }

        /// <summary>
        /// Get/Set whether the game should run with a fixed time step.
        /// </summary>
        public bool GameFixedTimeStep
        {
            get { return this.Game.IsFixedTimeStep; }
            set { this.Game.IsFixedTimeStep = value; }
        }

        /// <summary>
        /// Get/Set whether the Mouse Cursor is Visible.
        /// </summary>
        public bool GameIsMouseVisible
        {
            get { return this.Game.IsMouseVisible; }
            set { this.Game.IsMouseVisible = value; }
        }

        /// <summary>
        /// Get Access to the Game's Window.
        /// </summary>
        public GameWindow GameWindow
        {
            get { return this.Game.Window; }
        }

        /// <summary>
        /// Get/Set the Games Target Elapsed Time if the game
        /// is a fixed timestep application.
        /// </summary>
        public TimeSpan GameTargetElapsedTime
        {
            get { return this.Game.TargetElapsedTime; }
            set { this.Game.TargetElapsedTime = value; }
        }

        /// <summary>
        /// Get Access to the Game Launch Parameters.
        /// </summary>
        public LaunchParameters GameLaunchParameters
        {
            get { return this.Game.LaunchParameters; }
        }

        //------------------------METHOD OVERRIDES---------------------------------------------------------------

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._is_init = true;
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            this._obj_content = Game.Content;
            this._obj_input = new InputManager();

            this._obj_sprite_batch = new SpriteBatch(GraphicsDevice);
            this._obj_default_guifont = this._obj_content.Load<SpriteFont>(SysConfig.ASSET_CONFIG_GUI_FONT);
            this._obj_default_gamefont = this._obj_content.Load<SpriteFont>(SysConfig.ASSET_CONFIG_HUD_FONT);
            this._obj_default_debugfont = this._obj_content.Load<SpriteFont>(SysConfig.ASSET_CONFIG_DEBUG_FONT);
            //this._obj_tex_blank = this._obj_content.Load<Texture2D>(SysConfig.ASSET_CONFIG_BLANK_BG);

            // Tell each of the screens to load their content.
            foreach (BaseScreen tscreen in this._list_screens_master)
            {
                tscreen.loadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (BaseScreen tscreen in _list_screens_master)
            {
                tscreen.unloadContent();
            }
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// <param name="pgametime">The Game Timer</param>
        /// </summary>
        public override void Update(GameTime mGameTime)
        {
            this._obj_timer = mGameTime;

            // Read the keyboard and gamepad.
            this._obj_input.startUpdate();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            this._list_screens_updating.Clear();

            foreach (BaseScreen tscreen in this._list_screens_master)
            {
                this._list_screens_updating.Add(tscreen);
            }

            bool totherfocused = !Game.IsActive;
            bool tcovered = false;

            // Loop as long as there are screens waiting to be updated.
            while (this._list_screens_updating.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                BaseScreen tscreen = this._list_screens_updating[this._list_screens_updating.Count - 1];
                this._list_screens_updating.RemoveAt(this._list_screens_updating.Count - 1);

                // Update the screen.
                tscreen.masterUpdate(totherfocused, tcovered);

                if (tscreen.CurrentScreenMode == ScreenMode.MODE_TRANSITION_ON || tscreen.CurrentScreenMode == ScreenMode.MODE_ACTIVE)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!totherfocused)
                    {
                        tscreen.update();
                        totherfocused = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!tscreen.IsPopupScreen)
                        tcovered = true;
                }
            }

            this._obj_input.endUpdate();
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// <param name="pgametime">The Game Timer</param>
        /// </summary>
        public override void Draw(GameTime mGameTime)
        {
            foreach (BaseScreen mScreen in this._list_screens_master)
            {
                if (mScreen.CurrentScreenMode == ScreenMode.MODE_TRANSITION_HIDDEN)
                    continue;
                mScreen.render();
            }
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// <param name="pcontrollingplayer">The Active Controlling Player.</param>
        /// <param name="pscreen">The Screen to be added to the Screen Manager.</param>
        /// </summary>
        public void addScreen(BaseScreen mScreen, PlayerIndex? mPlayer)
        {
            mScreen.setScreenManager(this, mPlayer);

            // If we have a graphics device, tell the screen to load content.
            if (this._is_init)
                mScreen.loadContent();

            this._list_screens_master.Add(mScreen);
        }


        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// <param name="pscreen">Screen to be transitioned off.</param>
        /// </summary>
        public void removeScreen(BaseScreen mScreen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (this._is_init)
            {
                mScreen.unloadContent();
            }

            this._list_screens_master.Remove(mScreen);
            this._list_screens_updating.Remove(mScreen);
        }

        /// <summary>
        /// Fade the Back Buffer to Black, with the given alpha value
        /// </summary>
        /// <param name="palpha">The alpha value between 0-1</param>
        public void fadeBackBuffer(float palpha)
        {
            this.fadeBackBuffer(Color.Black * palpha);
        }

        /// <summary>
        /// Reset the Elapsed Time of the Game Timer.
        /// </summary>
        public void resetElapsedTime()
        {
            this.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Fade the Backbuffer with the given colour.
        /// </summary>
        /// <param name="pcolour">The colour to fade.</param>
        public void fadeBackBuffer(Color pcolour)
        {
            this._obj_sprite_batch.Begin();
            this._obj_sprite_batch.Draw(this._obj_tex_blank, new Rectangle(0, 0, this.GameViewport.Width, this.GameViewport.Height), pcolour);
            this._obj_sprite_batch.End();
        }


        //--------------------SCREEN DATA STRUCTURE ACCESSORS------------------------------------------------------------

        /// <summary>
        /// Get the List of Screens currently stored by the manager
        /// as an Array.
        /// </summary>
        public BaseScreen[] getScreens()
        {
            return this._list_screens_master.ToArray();
        }

        /// <summary>
        /// Get Access to a Screen in the Screen Manager by its String Identifier.
        /// </summary>
        /// <param name="pname">The String representing the name of the screen.</param>
        /// <returns>The Screen that is called this name, false otherwise.</returns>
        public BaseScreen getScreenByName(String pname)
        {
            foreach (BaseScreen tscreen in this._list_screens_master)
            {
                if (tscreen.ScreenName.Equals(pname))
                    return tscreen;
            }
            return null;
        }
    }
}
