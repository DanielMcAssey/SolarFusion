using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core.Screen
{
    public class MenuItemCharacterSelect
    {
        //--------------CLASS CONSTANTS-------------------------------------------------------
        public static readonly Color DEF_COLOUR_NORMAL = Color.White;

        //--------------CLASS MEMBERS---------------------------------------------------------
        protected float _item_fade;
        protected Color _item_colour_def;

        protected Vector2 _item_text_pos;
        protected string _item_text;

        protected Vector2 _item_box_pos;
        protected Texture2D _item_box_tex;

        protected Vector2 _item_selector_left_pos;
        protected Texture2D _item_selector_left_tex;
        protected Texture2D _item_selector_left_tex_press;
        protected Vector2 _item_selector_right_pos;
        protected Texture2D _item_selector_right_tex;
        protected Texture2D _item_selector_right_tex_press;

        protected float _item_press_time;
        protected float _item_press_time_passed;
        protected bool _item_pressed_left;
        protected bool _item_pressed_right;

        //--------------CLASS EVENTS----------------------------------------------------------
        public event EventHandler<EventPlayer> OnSelected;
        public event EventHandler<EventPlayer> OnIncrement;
        public event EventHandler<EventPlayer> OnDecrement;

        //--------------CONSTRUCTORS----------------------------------------------------------

        /// <summary>
        /// Get/Set the Menu Text
        /// </summary>
        public string Text
        {
            get { return _item_text; }
            set { _item_text = value; }
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text,
        /// this is the full constructor, allowing each property to be set.
        /// <param name="pitemtext">The menu item text</param>
        /// <param name="pnormal">The normal colour of the menu</param>
        /// <param name="pselected">The selected colour of the menu</param>
        /// <param name="ppulserate">The pulse rate of the menu</param>
        /// </summary>
        public MenuItemCharacterSelect(Color pnormal, ContentManager _content, Viewport _vp)
        {
            this._item_colour_def = pnormal;
            this._item_box_tex = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/misc/blank_panel");
            this._item_selector_left_tex = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/elements/blue_sliderLeft");
            this._item_selector_right_tex = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/elements/blue_sliderRight");
            this._item_selector_left_tex_press = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/elements/yellow_sliderLeft");
            this._item_selector_right_tex_press = _content.Load<Texture2D>("Sprites/Misc/UI/Buttons/elements/yellow_sliderRight");
            this._item_press_time = 80; //Time in Miliseconds

            Vector2 tmp_vp_center = new Vector2(_vp.Width / 2f, _vp.Height / 2f);
            float tmp_offset_center_x = 160f;
            float tmp_offset_center_y = 160f;

            this._item_box_pos = tmp_vp_center;
            this._item_selector_left_pos = new Vector2(tmp_vp_center.X - tmp_offset_center_x, tmp_vp_center.Y);
            this._item_selector_right_pos = new Vector2(tmp_vp_center.X + tmp_offset_center_x, tmp_vp_center.Y);
            this._item_text = "CHARACTER";
            this._item_text_pos = new Vector2(tmp_vp_center.X, tmp_vp_center.Y + tmp_offset_center_y);
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// Using the Default Constants for selected and non-selected colour
        /// and the pulse rate.
        /// <param name="pitemtext">The text of the menu item.</param>
        /// </summary>
        public MenuItemCharacterSelect(ContentManager _content, Viewport _vp)
            : this(DEF_COLOUR_NORMAL, _content, _vp)
        {

        }

        //------------------PUBLIC OVERRIDABLE METHODS--------------------------------------------------------------

        /// <summary>
        /// Update the state of the menu item
        /// </summary>
        /// <param name="pscreen">The Screen this menu item belongs to.</param>
        /// <param name="pselected">True if this is the currently selected item.</param>
        public virtual void update(BaseGUIScreen pscreen)
        {
            if (this._item_pressed_right || this._item_pressed_left)
            {
                this._item_press_time_passed += (float)pscreen.GlobalGameTimer.ElapsedGameTime.TotalMilliseconds;
                if (this._item_press_time_passed > this._item_press_time)
                {
                    this._item_pressed_right = false;
                    this._item_pressed_left = false;
                    this._item_press_time_passed = 0;
                }
            }
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// <param name="pscreen">The Screen this menu item belongs to.</param>
        /// <param name="pselected">True if this is the currently selected item.</param>
        /// </summary>
        public virtual void render(BaseGUIScreen pscreen)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color tmenuimgclr = this._item_colour_def;

            // Modify the alpha to fade text out during transitions.
            tmenuimgclr *= pscreen.CurrentTransitionAlpha;

            pscreen.ScreenManager.SpriteBatch.Begin();
            pscreen.ScreenManager.SpriteBatch.Draw(this._item_box_tex, this._item_box_pos, null, tmenuimgclr, 0f, new Vector2(this._item_box_tex.Width / 2f, this._item_box_tex.Height / 2f), 1f, SpriteEffects.None, 0f);
            if (this._item_pressed_left)
                pscreen.ScreenManager.SpriteBatch.Draw(this._item_selector_left_tex_press, this._item_selector_left_pos, null, tmenuimgclr, 0f, new Vector2(this._item_selector_left_tex_press.Width / 2f, this._item_selector_left_tex_press.Height / 2f), 0.8f, SpriteEffects.None, 0f);
            else
                pscreen.ScreenManager.SpriteBatch.Draw(this._item_selector_left_tex, this._item_selector_left_pos, null, tmenuimgclr, 0f, new Vector2(this._item_selector_left_tex.Width / 2f, this._item_selector_left_tex.Height / 2f), 0.8f, SpriteEffects.None, 0f);
            
            if (this._item_pressed_right)
                pscreen.ScreenManager.SpriteBatch.Draw(this._item_selector_right_tex_press, this._item_selector_right_pos, null, tmenuimgclr, 0f, new Vector2(this._item_selector_right_tex_press.Width / 2f, this._item_selector_right_tex_press.Height / 2f), 0.8f, SpriteEffects.None, 0f);
            else
                pscreen.ScreenManager.SpriteBatch.Draw(this._item_selector_right_tex, this._item_selector_right_pos, null, tmenuimgclr, 0f, new Vector2(this._item_selector_right_tex.Width / 2f, this._item_selector_right_tex.Height / 2f), 0.8f, SpriteEffects.None, 0f);

            Vector2 tmp_txt_origin = pscreen.ScreenManager.DefaultGUIFont.MeasureString(this._item_text);
            pscreen.ScreenManager.SpriteBatch.DrawString(pscreen.ScreenManager.DefaultGUIFont, this._item_text, this._item_text_pos, tmenuimgclr, 0f, new Vector2(tmp_txt_origin.X / 2f, tmp_txt_origin.Y / 2f), 0.6f, SpriteEffects.None, 0);
            pscreen.ScreenManager.SpriteBatch.End();
        }

        //-----------------PRIVATE / PROTECTED METHODS-----------------------------------------------------------

        /// <summary>
        /// Method for raising the Selected event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnSelected != null)
                this.OnSelected(this, new EventPlayer(pplayerindex));
        }

        /// <summary>
        /// Method for raising the Increment event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnIncrementEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnIncrement != null)
            {
                this.OnIncrement(this, new EventPlayer(pplayerindex));
                this._item_pressed_right = true;
            }
        }

        /// <summary>
        /// Method for raising the Decrement event.
        /// <param name="pplayerindex">The active player's index</param>
        /// </summary>
        protected internal virtual void OnDecrementEntry(PlayerIndex? pplayerindex)
        {
            if (this.OnDecrement != null)
            {
                this.OnDecrement(this, new EventPlayer(pplayerindex));
                this._item_pressed_left = true;
            }
        }
    }
}
