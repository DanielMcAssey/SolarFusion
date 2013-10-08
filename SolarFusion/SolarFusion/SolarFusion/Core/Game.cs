using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SolarFusion.Core;
using SolarFusion.Core.Config;
using SolarFusion.Screen;
using SolarFusion.Screen.System;

namespace SolarFusion
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _obj_graphics;
        ConfigManager _obj_config;
        ScreenManager _obj_screenmanager;

        public Game()
        {
            this._obj_config = new ConfigManager();

            this.Window.Title = SysConfig.CONFIG_GAME_NAME;
            this.Content.RootDirectory = SysConfig.CONFIG_CONTENT_ROOT;

            this._obj_graphics = new GraphicsDeviceManager(this);
            this._obj_graphics.PreferredBackBufferWidth = this._obj_config.Settings.VIDEO_RES_WIDTH;
            this._obj_graphics.PreferredBackBufferHeight = this._obj_config.Settings.VIDEO_RES_HEIGHT;
            this._obj_graphics.IsFullScreen = this._obj_config.Settings.VIDEO_FULLSCREEN;

            this._obj_screenmanager = new ScreenManager(this);
            this.Components.Add(this._obj_screenmanager);

            this._obj_screenmanager.addScreen(new ScreenBG(), null);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
    }
}
