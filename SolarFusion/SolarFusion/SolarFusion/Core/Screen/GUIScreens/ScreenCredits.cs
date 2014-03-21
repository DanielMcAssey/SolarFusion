using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    struct PeopleCredited
    {
        public string Position;
        public string Name;
        public string FullString;
        public Vector2 TextPosition;
        public Vector2 TextOrigin;
    }

    class ScreenCredits : BaseGUIScreen
    {
        protected List<PeopleCredited> mCreditList;
        protected int mMaxTextLength;
        protected float mMaxWidth;
        protected float mTextScale;

        public ScreenCredits()
            : base("CREDITS", Color.White, true, "System/UI/Logos/static_dimensionalwave", true, 1f)
        {
            this.mCreditList = new List<PeopleCredited>();
        }

        public override void loadContent()
        {
            Vector2 tmpStartPos = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width / 2, (this.ScreenManager.GraphicsDevice.Viewport.Height / 2) - 100);
            float tmpOffset = 10f;
            this.mMaxTextLength = 60;
            this.mMaxWidth = 700f;
            this.mTextScale = 0.7f;

            // Add People to Credits
            PeopleCredited tmpCredit = new PeopleCredited();
            tmpCredit.Position = "Programmer/Designer";
            tmpCredit.Name = "Daniel McAssey";
            this.mCreditList.Add(tmpCredit);
            tmpCredit.Position = "Programmer/Designer";
            tmpCredit.Name = "Jamie Finnegan";
            this.mCreditList.Add(tmpCredit);
            tmpCredit.Position = "Programmer/Designer";
            tmpCredit.Name = "James Adams";
            this.mCreditList.Add(tmpCredit);
            // !AddPeopleToCredits

            for (int i = 0; i < this.mCreditList.Count; i++)
            {
                PeopleCredited tmpFixCredit = this.mCreditList[i];

                if ((tmpFixCredit.Name.Length + tmpFixCredit.Position.Length) < this.mMaxTextLength)
                {
                    while (tmpFixCredit.Position.Length < (this.mMaxTextLength - tmpFixCredit.Name.Length))
                    {
                        tmpFixCredit.Position += ".";
                    }
                }

                tmpFixCredit.FullString = tmpFixCredit.Position + tmpFixCredit.Name;
                Vector2 tmpFontSize = this.ScreenManager.DefaultGUIFont.MeasureString(tmpFixCredit.FullString);
                tmpFixCredit.TextOrigin = new Vector2(tmpFontSize.X / 2, tmpFontSize.Y / 2);
                tmpFixCredit.TextPosition = new Vector2(tmpStartPos.X, (tmpStartPos.Y + (tmpFontSize.Y * (i + 1))) + tmpOffset); //NEED TO FIX
                this.mCreditList[i] = tmpFixCredit;
            }

            base.loadContent();
        }

        public override void update()
        {
            if(this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
                this.exitScreen(); //Exit the screen.

            base.update(); 
        }

        public override void render()
        {
            base.render();
            this.ScreenManager.SpriteBatch.Begin();

            for (int i = 0; i < this.mCreditList.Count; i++)
                this.ScreenManager.SpriteBatch.DrawString(this.ScreenManager.DefaultGUIFont, this.mCreditList[i].FullString, this.mCreditList[i].TextPosition, Color.White, 0f, this.mCreditList[i].TextOrigin, this.mTextScale, SpriteEffects.None, 0f);

            this.ScreenManager.SpriteBatch.End();
        }
    }
}
