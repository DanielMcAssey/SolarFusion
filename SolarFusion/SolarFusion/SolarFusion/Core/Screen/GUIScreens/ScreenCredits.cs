using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    class ScreenCredits : BaseGUIScreen
    {

        Texture2D Logo;
        Vector2 Position;
        Vector2 Origion;

        public ScreenCredits()
            : base("", false, null)
        {
            
        }

        public override void loadContent()
        {
            base.loadContent();
            //Load stuff
            Logo = _content.Load<Texture2D>("System/UI/Logos/DW");
            Origion = new Vector2(Logo.Width/2, Logo.Height/2);
            Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2,
                                    ScreenManager.GraphicsDevice.Viewport.Height /3);
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
            SpriteBatch tsb = ScreenManager.SpriteBatch;

            tsb.Begin();

            tsb.Draw(Logo, Position, null, Color.White, 0f, Origion, 1.5f, SpriteEffects.None, 0);

            tsb.End();
            base.render();
        }

    }
}
