using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    class ScreenPause : BaseGUIScreen
    {
        //----------------CLASS CONSTANTS---------------------------------------------------------
        public const float DEFAULT_ALPHA = 2.0f / 3.0f;
        public const int DEFAULT_PADDING_H = 32;
        public const int DEFAULT_PADDING_V = 16;
        public static readonly Color DEFAULT_COLOUR = Color.White;

        //----------------CLASS MEMBERS-----------------------------------------------------------
        protected float _message_alpha;

        //-----------------CONSTRUCTOR----------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenPause()
            : base("- PAUSED -", false, null, false, 1f)
        {
            this._message_alpha = DEFAULT_ALPHA;
        }

        public override void loadContent()
        {
            // Create our menu entries.
            MenuItemBasic tentryresume = new MenuItemBasic("RESUME", this.GlobalContentManager);
            MenuItemBasic tentryquit = new MenuItemBasic("EXIT", this.GlobalContentManager);

            // Hook up menu event handlers.
            tentryresume.OnSelected += DefaultTriggerMenuBack;
            tentryquit.OnSelected += EventTriggerGoToMain;

            // Add entries to the menu.
            this._list_menuitems.Add(tentryresume);
            this._list_menuitems.Add(tentryquit);

            this.BGColour = Color.Black;
            base.loadContent();
        }

        public override void render()
        {
            this.ScreenManager.fadeBackBuffer(this.CurrentTransitionAlpha * this._message_alpha);
            base.render();
        }

        //-----------------EVENT HANDLER DELEGATES---------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EventTriggerGoToMain(object sender, EventPlayer e)
        {
            const string tmessage = "Are you sure you want to exit the simulation?";
            ScreenMsgBox tmsgboxconfirmquit = new ScreenMsgBox(SysConfig.ASSET_CONFIG_MSGBOX_BG, tmessage);
            tmsgboxconfirmquit.onAccepted += EventTriggerConfirmGoToMain;
            ScreenManager.addScreen(tmsgboxconfirmquit, ControllingPlayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EventTriggerConfirmGoToMain(object sender, EventPlayer e)
        {
            //Trigger the Loading Screen to load our Background Menu and Overlay it with our Menu Screen
            ScreenLoading.Load(ScreenManager, this.ControllingPlayer, null, new ScreenBG(), new ScreenMenuRoot());
        }
    }
}
