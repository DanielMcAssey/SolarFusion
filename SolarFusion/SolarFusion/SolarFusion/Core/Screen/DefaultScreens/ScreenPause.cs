using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarFusion.Core.Screen
{
    class ScreenPause : BaseGUIScreen
    {
        //-----------------CONSTRUCTOR----------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenPause()
            : base("- PAUSED -", false, null)
        {
            // Create our menu entries.
            MenuItemBasic tentryresume = new MenuItemBasic("Resume");
            MenuItemBasic tentryquit = new MenuItemBasic("Exit");

            // Hook up menu event handlers.
            tentryresume.OnSelected += DefaultTriggerMenuBack;
            tentryquit.OnSelected += EventTriggerGoToMain;

            // Add entries to the menu.
            this._list_menuitems.Add(tentryresume);
            this._list_menuitems.Add(tentryquit);
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
