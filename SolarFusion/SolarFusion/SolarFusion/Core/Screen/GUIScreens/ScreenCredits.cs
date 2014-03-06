using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    class ScreenCredits : BaseGUIScreen
    {
        public ScreenCredits()
            : base("CREDITS", Color.White, true, "System/UI/Logos/static_dimensionalwave", true, 1f)
        {

        }

        public override void loadContent()
        {
            base.loadContent();
            //load credits
        }

        public override void update()
        {
            if(this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
            {
                this.exitScreen(); //Exit the screen.
            }

            base.update(); 
        }

        public override void render()
        {


            base.render();
        }
    }
}
