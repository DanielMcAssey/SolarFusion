using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolarFusion.Core.Screen
{
    public abstract class AppScreen : BaseScreen
    {
        public AppScreen()
        {
            this._trans_on_time = TimeSpan.FromSeconds(1.5);
            this._trans_off_time = TimeSpan.FromSeconds(0.5);
        }

        //-------------------METHOD OVERRIDES-------------------------------------------------------------

        /// <summary>
        /// Load graphics content for the game state
        /// This happens when our screen is added to the screen manager.
        /// </summary>
        public override void loadContent()
        {
            //Reset the Elapsed Time and create a content manager for this screen state.
            this.ScreenManager.resetElapsedTime();
            this.internCreateLocalContent();
        }


        /// <summary>
        /// Unload graphics content used by the game state.
        /// </summary>
        public override void unloadContent()
        {
            //Unload any content associated with this screen.
            this._local_content.Unload();
        }


        /// <summary>
        /// Update the game logic as long as we are in the manager stack.
        /// </summary>
        /// <param name="potherscreen"></param>
        /// <param name="poverlaid"></param>
        public override void bgUpdate(bool potherscreen, bool poverlaid)
        {
            //Update the Screen Fade
            this.updateScreenFade(poverlaid);
        }


        /// <summary>
        /// Update the game logic as long as we are the topmost screen
        /// </summary>
        public override void update()
        {
            //Check to see if the Pause Action has been Triggered
            this.checkForPauseAction();
        }

        /// <summary>
        /// Render our Screen's content.
        /// </summary>
        public override void render()
        {
            //Clear Screen Black
            this.ScreenManager.GraphicsDevice.Clear(Color.Black);
            //Render the screen
            this.appRender();
            //Draw our overlay screen fade.
            this.renderScreenFade();
        }

        //------------------ABSTRACT METHODS-------------------------------------------------------------------------

        public abstract void appRender();
    }
}
