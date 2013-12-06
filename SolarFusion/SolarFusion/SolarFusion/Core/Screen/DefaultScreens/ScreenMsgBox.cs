using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.Screen
{
    class ScreenMsgBox : BaseScreen
    {
        //----------------CLASS CONSTANTS---------------------------------------------------------
        public const float DEFAULT_TRANS_TIME = 0.2f;
        public const float DEFAULT_ALPHA = 2.0f / 3.0f;
        public const int DEFAULT_PADDING_H = 32;
        public const int DEFAULT_PADDING_V = 16;
        public static readonly Color DEFAULT_COLOUR = Color.White;

        //----------------CLASS MEMBERS-----------------------------------------------------------
        protected string _message_text;
        protected Texture2D _message_bg;
        protected Color _message_colour;
        protected string _message_bg_file;
        protected int _message_padding_h;
        protected int _message_padding_v;
        protected float _message_alpha;

        //----------------CLASS EVENTS------------------------------------------------------------
        public event EventHandler<EventPlayer> onAccepted;
        public event EventHandler<EventPlayer> onCancelled;

        //----------------CONSTRUCTORS------------------------------------------------------------

        /// <summary>
        /// Full Constructor which creates a message box, with control over all settings
        /// </summary>
        /// <param name="ptexture">The background texture of the message box</param>
        /// <param name="pmessage">The message to display</param>
        /// <param name="pcolour">The Message Text Colour</param>
        /// <param name="pusagetext">Whether the Usage Text is displayed</param>
        /// <param name="pusagetextmsg">The Usage Text Message</param>
        public ScreenMsgBox(string ptexture, string pmessage, Color pcolour, bool pusagetext, string pusagetextmsg, float palpha = DEFAULT_ALPHA, float ptranstime = DEFAULT_TRANS_TIME, int ppaddingv = DEFAULT_PADDING_V, int ppaddingh = DEFAULT_PADDING_H)
        {
            this._message_bg_file = ptexture;
            this._message_text = pusagetext ? pmessage + pusagetextmsg : pmessage;
            this._is_popup = true;
            this._trans_on_time = TimeSpan.FromSeconds(ptranstime);
            this._trans_off_time = TimeSpan.FromSeconds(ptranstime);
            this._message_padding_h = ppaddingh;
            this._message_padding_v = ppaddingv;
            this._message_alpha = palpha;
            this._message_colour = pcolour;
        }

        /// <summary>
        /// Partial Constructor for the message box, which uses the default message text.
        /// </summary>
        /// <param name="ptexture">The Message Box Texture</param>
        /// <param name="pcolour">The Message Box Text Colour</param>
        /// <param name="pmessage">The Message Box Message Text</param>
        /// <param name="pusagetext">Whether the Usage Text is Displayed</param>
        public ScreenMsgBox(string ptexture, string pmessage, Color pcolour, bool pusagetext)
            : this(ptexture, pmessage, pcolour, pusagetext, SysConfig.ASSET_CONFIG_MSG_BOX_USAGE)
        { }

        /// <summary>
        /// Simple Constructor to control the background texture and the message text
        /// using a default colour and displaying the usage text.
        /// </summary>
        /// <param name="pmessage"></param>
        public ScreenMsgBox(string ptexture, string pmessage)
            : this(ptexture, pmessage, DEFAULT_COLOUR, true)
        { }

        //------------------METHOD OVERRIDES--------------------------------------------------------

        /// <summary>
        /// Load the Message Box Screen Content
        /// </summary>
        public override void loadContent()
        {
            this._message_bg = this.GlobalContentManager.Load<Texture2D>(this._message_bg_file);
        }

        /// <summary>
        /// Update the Screen, even if it isn't the top most screen
        /// </summary>
        /// <param name="potherfocused">True if another screen is topmost</param>
        /// <param name="poverlaid">True if overlaid by another overlay screen</param>
        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            //This is a static screen, so nothing to update by default.  
        }

        /// <summary>
        /// Update the Screen if this is the top most screen
        /// </summary>
        public override void update()
        {
            if (this.GlobalInput.IsPressed("NAV_SELECT", this.ControllingPlayer))
            {
                // Raise the accepted event, then exit the message box.
                if (this.onAccepted != null)
                    this.onAccepted(this, new EventPlayer(this.ControllingPlayer));

                this.exitScreen();
            }
            else if (this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer))
            {
                // Raise the cancelled event, then exit the message box.
                if (this.onCancelled != null)
                    this.onCancelled(this, new EventPlayer(ControllingPlayer));

                this.exitScreen();
            }
        }

        /// <summary>
        /// Render the Message Box on Screen
        /// </summary>
        public override void render()
        {
            SpriteBatch tsb = this.ScreenManager.SpriteBatch;
            SpriteFont tfont = this.ScreenManager.DefaultGUIFont;
            Viewport tviewport = this.ScreenManager.GameViewport;
            Vector2 tscreendim = new Vector2(tviewport.Width, tviewport.Height);
            Vector2 tsize = tfont.MeasureString(this._message_text);
            Vector2 ttextpos = (tscreendim - tsize) / 2;
            Rectangle ttextbound = new Rectangle((int)ttextpos.X - this._message_padding_h,
                                                    (int)ttextpos.Y - this._message_padding_v,
                                                    (int)tsize.X + this._message_padding_h * 2,
                                                    (int)tsize.Y + this._message_padding_v * 2);
            Color tcolour = this._message_colour * this.CurrentTransitionAlpha;

            this.ScreenManager.fadeBackBuffer(this.CurrentTransitionAlpha * this._message_alpha);
            tsb.Begin();
            tsb.Draw(this._message_bg, ttextbound, tcolour);
            tsb.DrawString(tfont, this._message_text, ttextpos, tcolour);
            tsb.End();
        }

        /// <summary>
        /// Unload the Content of this screen
        /// </summary>
        public override void unloadContent()
        {
            //Nothing to unload since the message box is cached by default.
        }
    }
}
