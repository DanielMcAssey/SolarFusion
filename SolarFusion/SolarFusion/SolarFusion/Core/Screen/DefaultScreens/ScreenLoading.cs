using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    class ScreenLoading : BaseScreen
    {
        //--------------STATIC METHODS------------------------------------------------------------

        /// <summary>
        /// Activates the loading screen.
        /// <param name="pmanager">The Screen Manager</param>
        /// <param name="ptexture">The loading screen texture</param>
        /// <param name="pisdisplayed">Is the Default Loading Message Displayed</param>
        /// <param name="pcontrollingplayer">The player who is set as the controlling player</param>
        /// <param name="pnewscreens">The Screens to Load</param>
        /// </summary>
        public static void Load(ScreenManager pmanager, PlayerIndex? pcontrollingplayer, params BaseScreen[] pnewscreens)
        {
            Load(pmanager, String.Empty, false, pcontrollingplayer, pnewscreens);
        }


        /// <summary>
        /// Activates the loading screen.
        /// <param name="pmanager">The Screen Manager</param>
        /// <param name="ptexture">The loading screen texture</param>
        /// <param name="pisdisplayed">Is the Default Loading Message Displayed</param>
        /// <param name="pcontrollingplayer">The player who is set as the controlling player</param>
        /// <param name="pnewscreens">The Screens to Load</param>
        /// </summary>
        public static void Load(ScreenManager pmanager, bool pisdisplayed, PlayerIndex? pcontrollingplayer, params BaseScreen[] pnewscreens)
        {
            Load(pmanager, SysConfig.ASSET_CONFIG_MSG_LOADING, pisdisplayed, pcontrollingplayer, pnewscreens);
        }


        /// <summary>
        /// Activates the loading screen.
        /// <param name="pmanager">The Screen Manager</param>
        /// <param name="ptexture">The loading screen texture</param>
        /// <param name="pmessage">The Message to Display</param>
        /// <param name="pisdisplayed">Is the Loading Message Displayed</param>
        /// <param name="pcontrollingplayer">The player who is set as the controlling player</param>
        /// <param name="pnewscreens">The Screens to Load</param>
        /// </summary>
        public static void Load(ScreenManager pmanager, String pmessage, bool pisdisplayed, PlayerIndex? pcontrollingplayer, params BaseScreen[] pnewscreens)
        {
            // Tell all the current screens to transition off.
            foreach (BaseScreen tscreen in pmanager.getScreens())
                tscreen.exitScreen();

            // Create and activate the loading screen.
            ScreenLoading tloadingscreen = new ScreenLoading(pmanager, pmessage, pisdisplayed, pnewscreens);
            pmanager.addScreen(tloadingscreen, pcontrollingplayer);
        }

        //--------------CLASS MEMBERS-------------------------------------------------------------
        protected bool _is_displayed;
        protected bool _prev_screens_clean;
        protected BaseScreen[] _new_screen_stack;
        protected String _loading_msg;

        //--------------CONSTRUCTORS--------------------------------------------------------------

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// <param name="pmanager">The Screen Manager</param>
        /// <param name="ptexture">The Loading Screen Texture</param>
        /// <param name="pmessage">The Loading Screen Message</param>
        /// <param name="pisdisplayed">Is the Message to be displayed?</param>
        /// <param name="pnewscreens">A List of the New Screens</param>
        /// </summary>
        private ScreenLoading(ScreenManager pmanager, String pmessage, bool pisdisplayed, BaseScreen[] pnewscreens)
        {
            this._loading_msg = pmessage;
            this._is_displayed = pisdisplayed;
            this._new_screen_stack = pnewscreens;
            this._trans_on_time = TimeSpan.FromSeconds(0.5);
        }

        //---------------METHOD OVERRIDES---------------------------------------------------------

        /// <summary>
        /// Load the Screen Content.
        /// </summary>
        public override void loadContent()
        {
        }

        /// <summary>
        /// Updates the loading screen.
        /// <param name="potherfocused">Is another screen the topmost</param>
        /// <param name="poverlaid">Is this screen overlaid by an overlay screen</param>
        /// </summary>
        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            if (this._prev_screens_clean)
            {
                this.ScreenManager.removeScreen(this);

                foreach (BaseScreen tscreen in this._new_screen_stack)
                {
                    if (tscreen != null)
                    {
                        this.ScreenManager.addScreen(tscreen, ControllingPlayer);
                    }
                }
                this.ScreenManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Function which updates the screen if it is active.
        /// </summary>
        public override void update()
        {
            //Nothing to update, since this loading screen is static.
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void render()
        {
            this.ScreenManager.GraphicsDevice.Clear(Color.Black);
            if ((this.CurrentScreenMode == ScreenMode.MODE_ACTIVE) && (this.ScreenManager.getScreens().Length == 1))
            {
                this._prev_screens_clean = true;
            }

            if (!this._is_displayed)
                return;

            SpriteBatch tspritebatch = this.ScreenManager.SpriteBatch;
            SpriteFont tdisplayfont = this.ScreenManager.DefaultGUIFont;

            // Center the text in the viewport.
            Viewport tobjviewport = this.ScreenManager.GraphicsDevice.Viewport;
            Vector2 tviewportdim = new Vector2(tobjviewport.Width, tobjviewport.Height);
            Rectangle tscreenrect = new Rectangle(0, 0, tobjviewport.Width, tobjviewport.Height);

            // Draw the text.
            tspritebatch.Begin();

            if (!String.IsNullOrEmpty(this._loading_msg))
            {
                Vector2 ttextdim = tdisplayfont.MeasureString(this._loading_msg);
                Vector2 ttextpos = (tviewportdim - ttextdim) / 2;
                Color tcolour = Color.White * CurrentTransitionAlpha;
                tspritebatch.DrawString(tdisplayfont, this._loading_msg, ttextpos, tcolour);
            }
            tspritebatch.End();
        }

        /// <summary>
        /// Unload the Screen's Content.
        /// </summary>
        public override void unloadContent()
        {
            //Don't Unload the Content, since we want the loading screen to stay cached.
        }
    }
}
