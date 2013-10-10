using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SolarFusion.Screen.System;
using SolarFusion.Screen.System.Components;
using SolarFusion.Screen.System.Events;

namespace SolarFusion.Screen.GUIScreens
{
    class ScreenMenuRoot : BaseGUIScreen
    {
        public ScreenMenuRoot()
            : base("Root_Menu")
        {
            MenuItemBasic mi_play = new MenuItemBasic("Play");
            MenuItemBasic mi_options = new MenuItemBasic("Options");
            MenuItemBasic mi_credits = new MenuItemBasic("Credits");
            MenuItemBasic mi_exit = new MenuItemBasic("Exit");

            mi_play.OnSelected += EventTriggerGoToCharSelect;
            mi_options.OnSelected += EventTriggerGoToOptions;
            mi_credits.OnSelected += EventTriggerGoToCredits;
            mi_exit.OnSelected += DefaultTriggerMenuBack;

            this._list_menuitems.Add(mi_play);
            this._list_menuitems.Add(mi_options);
            this._list_menuitems.Add(mi_credits);
            this._list_menuitems.Add(mi_exit);
        }

        //---------------EVENT HANDLERS-------------------------------------------------------------

        /// <summary>
        /// Event Handler to Go to character select screen.
        /// </summary>
        void EventTriggerGoToCharSelect(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS2(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Options Screen.
        /// </summary>
        void EventTriggerGoToOptions(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS3(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Credits Screen.
        /// </summary>
        void EventTriggerGoToCredits(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS3(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex? pplyrindex)
        {
            const string tmessage = "Are you sure you want to exit this test?";
            //LJMUBaseScreenMsgBox tmsgbox = new LJMUBaseScreenMsgBox(LabConfig.LAB_ASSET_GUI_MSGBOX_BG, tmessage);

            //tmsgbox.onAccepted += EventTriggerMsgBoxConfirm;
            //ScreenManager.addScreen(tmsgbox, pplyrindex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void EventTriggerMsgBoxConfirm(object sender, EventPlayer e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
