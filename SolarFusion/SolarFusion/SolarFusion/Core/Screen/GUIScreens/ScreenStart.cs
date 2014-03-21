using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    class ScreenStart : BaseGUIScreen
    {
        public ScreenStart()
            : base("Start_Screen", true, "System/UI/Logos/static_jumpista", true, 0.5f)
        {
        }

        public override void loadContent()
        {
            MenuItemBasic mi_start = new MenuItemBasic("PRESS START", this.GlobalContentManager);
            this._list_menuitems.Add(mi_start);
            base.loadContent();
        }

        public override void update()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.GlobalInput.IsPressed("GLOBAL_START", (PlayerIndex)i))
                {
                    this.ControllingPlayer = (PlayerIndex)i;
                    this.EventTriggerGoToMenu(this.ControllingPlayer);
                    return;
                }
            }

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
        void EventTriggerGoToMenu(PlayerIndex? _player)
        {
            ScreenManager.addScreen(new ScreenMenuRoot(), _player);
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
