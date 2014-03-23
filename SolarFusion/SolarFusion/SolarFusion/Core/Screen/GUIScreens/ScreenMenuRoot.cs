using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    class ScreenMenuRoot : BaseGUIScreen
    {
        public ScreenMenuRoot()
            : base("Root_Menu", true, "System/UI/Logos/static_jumpista", true, 0.5f)
        {
        }

        public override void loadContent()
        {
            MenuItemBasic mi_play = new MenuItemBasic("PLAY", this.GlobalContentManager);
            MenuItemBasic mi_credits = new MenuItemBasic("CREDITS", this.GlobalContentManager);
            MenuItemBasic mi_exit = new MenuItemBasic("EXIT", this.GlobalContentManager);

            mi_play.OnSelected += EventTriggerGoToCharSelect;
            mi_credits.OnSelected += EventTriggerGoToCredits;
            mi_exit.OnSelected += DefaultTriggerMenuBack;

            this._list_menuitems.Add(mi_play);
            this._list_menuitems.Add(mi_credits);
            this._list_menuitems.Add(mi_exit);

            base.loadContent();
        }

        public override void update()
        {
            base.update();
        }

        public override void render()
        {
            base.render();
        }

        //---------------EVENT HANDLERS-------------------------------------------------------------

        /// <summary>
        /// Event Handler to Go to character select screen.
        /// </summary>
        void EventTriggerGoToCharSelect(object sender, EventPlayer e)
        {
            ScreenManager.addScreen(new ScreenCharSelect(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Credits Screen.
        /// </summary>
        void EventTriggerGoToCredits(object sender, EventPlayer e)
        {
            ScreenManager.addScreen(new ScreenCredits(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex? pplyrindex)
        {
            const string tmessage = "Are you sure you want to exit?";
            ScreenMsgBox tmsgbox = new ScreenMsgBox(SysConfig.ASSET_CONFIG_MSGBOX_BG, tmessage);

            tmsgbox.onAccepted += EventTriggerMsgBoxConfirm;
            ScreenManager.addScreen(tmsgbox, pplyrindex);
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
