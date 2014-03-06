using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    class ScreenCharSelect : BaseGUIScreen
    {
        MenuItemCharacterSelect _obj_selector;

        public ScreenCharSelect()
            : base("Character Select", Color.White, false, null, false, 1f)
        {
        }

        public override void loadContent()
        {
            this._obj_selector = new MenuItemCharacterSelect(this.ScreenManager.ContentManager, this.ScreenManager.GameViewport);
            this._obj_selector.OnIncrement += EventTriggerNextCharacter;
            this._obj_selector.OnDecrement += EventTriggerPreviousCharacter;
            base.loadContent();
        }

        public override void update()
        {
            this._obj_selector.update(this);

            if (this.GlobalInput.IsPressed("NAV_RIGHT", this.ControllingPlayer))
                this._obj_selector.OnIncrementEntry(this.ControllingPlayer);
            else if (this.GlobalInput.IsPressed("NAV_LEFT", this.ControllingPlayer))
                this._obj_selector.OnDecrementEntry(this.ControllingPlayer);

            if (this.GlobalInput.IsPressed("NAV_SELECT", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
                ScreenLoading.Load(this.ScreenManager, "-LOADING-", true, this.ControllingPlayer, new ScreenGame()); //Load Game

            if (this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
                this.exitScreen(); //Exit the screen.

            base.update();
        }

        public override void render()
        {
            this._obj_selector.render(this);

            base.render();
        }


        /// <summary>
        /// Event Handler to show next character.
        /// </summary>
        void EventTriggerNextCharacter(object sender, EventPlayer e)
        {

        }

        /// <summary>
        /// Event Handler to show previous character.
        /// </summary>
        void EventTriggerPreviousCharacter(object sender, EventPlayer e)
        {

        }
    }
}
