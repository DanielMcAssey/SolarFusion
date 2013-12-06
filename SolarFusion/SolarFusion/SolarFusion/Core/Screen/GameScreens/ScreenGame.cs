using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Core.Screen
{
    class ScreenGame : AppScreen
    {
        public override void loadContent()
        {
            base.loadContent();

            this.BGColour = Color.Black;
        }

        public override void unloadContent()
        {
            this._local_content.Unload();
            base.unloadContent();
        }

        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            base.bgUpdate(potherfocused, poverlaid);
        }

        public override void update() //Update per frame
        {
            TimeSpan _elapsedTime = this.GlobalGameTimer.ElapsedGameTime;
            TimeSpan _totalTime = this.GlobalGameTimer.TotalGameTime;
            

            base.update();
        }

        public override void appRender() //Render per frame
        {
            this.internResetRenderStatesFor2D(); //Reset 2D states

            this.ScreenManager.SpriteBatch.Begin();

            this.ScreenManager.SpriteBatch.End();
        }
    }
}
