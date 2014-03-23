using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    public enum EndingType
    {
        Win = 0,
        Loss
    }

    public class ScreenEnding : BaseGUIScreen
    {
        //----------------CLASS CONSTANTS---------------------------------------------------------
        public const float DEFAULT_ALPHA = 2.0f / 3.0f;
        public const int DEFAULT_PADDING_H = 32;
        public const int DEFAULT_PADDING_V = 16;
        public static readonly Color DEFAULT_COLOUR = Color.White;

        //----------------CLASS MEMBERS-----------------------------------------------------------
        protected float _message_alpha;
        protected EndingType _end_type;

        //-----------------CONSTRUCTOR----------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenEnding(string _message, EndingType _type)
            : base(_message, Color.White, false, null, false, 1f)
        {
            this._is_popup = true;
            this._message_alpha = DEFAULT_ALPHA;
            this._end_type = _type;
        }

        public override void loadContent()
        {
            switch (this._end_type)
            {
                case EndingType.Win:
                    MenuItemBasic winEntryQuit = new MenuItemBasic("MAIN MENU", this.GlobalContentManager);
                    winEntryQuit.OnSelected += EventTriggerGoToMain;
                    this._list_menuitems.Add(winEntryQuit);
                    break;
                case EndingType.Loss:
                    MenuItemBasic lossEntryQuit = new MenuItemBasic("MAIN MENU", this.GlobalContentManager);
                    lossEntryQuit.OnSelected += EventTriggerGoToMain;
                    this._list_menuitems.Add(lossEntryQuit);
                    break;
            }

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
            //Trigger the Loading Screen to load our Background Menu and Overlay it with our Menu Screen
            ScreenLoading.Load(ScreenManager, this.ControllingPlayer, null, new ScreenBG(), new ScreenMenuRoot());
        }
    }
}
