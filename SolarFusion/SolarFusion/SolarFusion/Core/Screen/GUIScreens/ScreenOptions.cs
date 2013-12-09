using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarFusion.Core.Screen
{
    class ScreenOptions : BaseGUIScreen
    {
        public ScreenOptions()
            : base("Options", false, null)
        {

        }

        public override void update()
        {
            if (this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
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
