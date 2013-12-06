using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarFusion.Core.Screen
{
    class ScreenCredits : BaseGUIScreen
    {
        public ScreenCredits()
            : base("CREDITS", false, null)
        {

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
