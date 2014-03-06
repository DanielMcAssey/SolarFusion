using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    public class MenuItemBasic
    {
        //--------------CLASS CONSTANTS-------------------------------------------------------
        public static readonly Color DEF_COLOUR_NORMAL   = Color.White;
        public static readonly Color DEF_COLOUR_SELECTED = new Color(46, 46, 46);

        //--------------CLASS MEMBERS---------------------------------------------------------
        protected string      _item_text;
        protected float       _item_fade;
        protected Vector2     _item_pos;
        protected Color       _item_colour_def;
        protected Color       _item_colour_selected;
        protected float       _item_pulse_rate;
        protected Texture2D _item_background;
        protected Texture2D _item_background_pressed;
        protected Texture2D _item_background_selected;
        protected float _item_press_time;
        protected float _item_press_time_passed;
        protected bool _item_pressed;

        //--------------CLASS EVENTS----------------------------------------------------------
        public event EventHandler<EventPlayer> OnSelected;
        public event EventHandler<EventPlayer> OnIncrement;
        public event EventHandler<EventPlayer> OnDecrement;

        //--------------CONSTRUCTORS----------------------------------------------------------

        /// <summary>
        /// Constructs a new menu entry with the specified text,
        /// this is the full constructor, allowing each property to be set.
        /// <param name="pitemtext">The menu item text</param>
        /// <param name="pnormal">The normal colour of the menu</param>
        /// <param name="pselected">The selected colour of the menu</param>
        /// <param name="ppulserate">The pulse rate of the menu</param>
        /// </summary>
        public MenuItemBasic(string pitemtext, Color pnormal, Color pselected, ContentManager _content)
        {
            this._item_text                 = pitemtext;
            this._item_colour_def           = pnormal;
            this._item_colour_selected      = pselected;
            this._item_background = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/rectangle/blue_01_btn");
            this._item_background_pressed = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/rectangle/blue_01_btn_press");
            this._item_background_selected = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/rectangle/yellow_01_btn");
            this._item_press_time = 120; //Time in Miliseconds
            
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// Using the Default Constants for selected and non-selected colour
        /// and the pulse rate.
        /// <param name="pitemtext">The text of the menu item.</param>
        /// </summary>
        public MenuItemBasic(string pitemtext, ContentManager _content) : this(pitemtext,DEF_COLOUR_NORMAL,DEF_COLOUR_SELECTED, _content)
        {

        }

        //---------------PROPERTIES-----------------------------------------------------------

        /// <summary>
        /// Get/Set the Menu Text
        /// </summary>
        public string Text
        {
            get { return _item_text; }
            set { _item_text = value; }
        }
        
        /// <summary>
        /// Get/Set the Menu Position
        /// </summary>
        public Vector2 Position
        {
            get { return _item_pos; }
            set { _item_pos = value; }
        }

        /// <summary>
        /// Get/Set the Non-Selected Colour
        /// </summary>
        public Color NonSelectedColour
        {
            get { return this._item_colour_def; }
            set { this._item_colour_def = value; }
        }

        /// <summary>
        /// Get/Set the Selected Colour
        /// </summary>
        public Color SelectedColour
        {
            get { return this._item_colour_selected; }
            set { this._item_colour_selected = value; }
        }
        
        //------------------PUBLIC OVERRIDABLE METHODS--------------------------------------------------------------

        /// <summary>
        /// Update the state of the menu item
        /// </summary>
        /// <param name="pscreen">The Screen this menu item belongs to.</param>
        /// <param name="pselected">True if this is the currently selected item.</param>
        public virtual void update(BaseGUIScreen pscreen, bool pselected)
        {
            float tfadespeed = (float)pscreen.GlobalGameTimer.ElapsedGameTime.TotalSeconds * this._item_pulse_rate;

            if (this._item_pressed)
            {
                this._item_press_time_passed += (float)pscreen.GlobalGameTimer.ElapsedGameTime.TotalMilliseconds;
                if (this._item_press_time_passed > this._item_press_time)
                {
                    this._item_pressed = false;
                    this._item_press_time_passed = 0;
                }
            }
            
            if (pselected)
                this._item_fade = Math.Min(this._item_fade + tfadespeed, 1);
            else
                this._item_fade = Math.Max(this._item_fade - tfadespeed, 0);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// <param name="pscreen">The Screen this menu item belongs to.</param>
        /// <param name="pselected">True if this is the currently selected item.</param>
        /// </summary>
        public virtual void render(BaseGUIScreen pscreen, bool pselected)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color tmenuitemclr = pselected ? this._item_colour_selected : this._item_colour_def;
            Color tmenuimgclr = this._item_colour_def;
            Vector2 tmenuposaddition = Vector2.Zero;

            // Modify the alpha to fade text out during transitions.
            tmenuitemclr *= pscreen.CurrentTransitionAlpha;
            tmenuimgclr *= pscreen.CurrentTransitionAlpha;

            if (this._item_pressed)
            {
                pscreen.ScreenManager.SpriteBatch.Draw(this._item_background_pressed, this._item_pos, null, tmenuimgclr, 0f, new Vector2(this._item_background_pressed.Width / 2, (this._item_background_pressed.Height / 2) - 3), 0.8f, SpriteEffects.None, 0);
                tmenuposaddition = new Vector2(0, 2);
            }
            else
            {
                if (pselected)
                {
                    pscreen.ScreenManager.SpriteBatch.Draw(this._item_background_selected, this._item_pos, null, tmenuimgclr, 0f, new Vector2(this._item_background_selected.Width / 2, (this._item_background_selected.Height / 2) - 3), 0.8f, SpriteEffects.None, 0);
                }
                else
                {
                    pscreen.ScreenManager.SpriteBatch.Draw(this._item_background, this._item_pos, null, tmenuimgclr, 0f, new Vector2(this._item_background.Width / 2, (this._item_background.Height / 2) - 3), 0.8f, SpriteEffects.None, 0);
                }
            }

            
            Vector2 textSize = pscreen.ScreenManager.DefaultGUIFont.MeasureString(this._item_text);
            Vector2 torigin = new Vector2(textSize.X / 2f, pscreen.ScreenManager.DefaultGUIFont.LineSpacing / 2);
            pscreen.ScreenManager.SpriteBatch.DrawString(pscreen.ScreenManager.DefaultGUIFont, this._item_text, this._item_pos + tmenuposaddition, tmenuitemclr, 0, torigin, 0.7f, SpriteEffects.None, 0);
        }

        //------------------PUBLIC METHODS-----------------------------------------------------------------------

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// <param name="pscreen">The Screen to place this item in.</param>
        /// </summary>
        public virtual int getMenuItemHeight(BaseGUIScreen pscreen)
        {
            return pscreen.ScreenManager.DefaultGUIFont.LineSpacing;
        }
        
        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// <param name="pscreen">The Screen to place this item in.</param>
        /// </summary>
        public virtual int getMenuItemWidth(BaseGUIScreen pscreen)
        {
            return (int)pscreen.ScreenManager.DefaultGUIFont.MeasureString(Text).X;
        }

        //-----------------PRIVATE / PROTECTED METHODS-----------------------------------------------------------

        /// <summary>
        /// Method for raising the Selected event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnSelected != null)
            {
                this.OnSelected(this, new EventPlayer(pplayerindex));
                this._item_pressed = true;
            }
        }

        /// <summary>
        /// Method for raising the Increment event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnIncrementEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnIncrement != null)
                this.OnIncrement(this, new EventPlayer(pplayerindex));
        }

        /// <summary>
        /// Method for raising the Decrement event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnDecrementEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnDecrement != null)
                this.OnDecrement(this, new EventPlayer(pplayerindex));
        }
    }
}
