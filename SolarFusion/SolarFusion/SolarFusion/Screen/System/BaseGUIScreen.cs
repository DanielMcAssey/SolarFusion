using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using SolarFusion.Input;
using SolarFusion.Screen.System;

namespace SolarFusion.Screen.System
{
    public abstract class BaseGUIScreen : BaseScreen
    {
        //----------------CLASS CONSTANTS------------------------------------------------------
        public float DEFAULT_TRANS_TIME  = 0.5f;
        public int DEFAULT_TITLE_START   = 100;
        public int DEFAULT_MENU_START    = 200;
        public static readonly Color DEFAULT_MENU_COLOUR = new Color(192, 192, 192); 

        //----------------CLASS MEMBERS_-------------------------------------------------------
        protected List<MenuItemBasic>  _list_menuitems      = new List<MenuItemBasic>();
        protected int                      _selected_item       = 0;
        protected string                   _menu_title_txt      = String.Empty;
        protected Color                    _menu_title_clr      = Color.White;
        protected int                      _menu_title_start;
        protected int                      _menu_item_start;
        

        //----------------CONSTRUCTORS---------------------------------------------------------

        /// <summary>
        /// Constructor for the Screen Menu Base
        /// <param name="pmenutitle">The Menu Title Text</param>
        /// <param name="ptitleclr">The Menu Title Colour</param>
        /// <param name="pmenustart">The menu item vertical start position</param>
        /// <param name="ptitlestart">The Title vertical start position</param>
        /// <param name="ptranstime">The Transition on time</param>
        /// </summary>
        public BaseGUIScreen(string pmenutitle, Color ptitleclr)
        {
            this._menu_title_txt    = pmenutitle;
            this._trans_on_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
            this._trans_off_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
            this._menu_title_start = DEFAULT_TITLE_START;
            this._menu_item_start = DEFAULT_MENU_START;
            this._menu_title_clr    = ptitleclr;
        }

        public int TitleStart
        {
            get { return DEFAULT_TITLE_START; }
            set { DEFAULT_TITLE_START = value; }
        }

        public int MenuStart
        {
            get { return DEFAULT_MENU_START; }
            set { DEFAULT_MENU_START = value; }
        }

        public float TransitionTime
        {
            get { return DEFAULT_TRANS_TIME; }
            set { DEFAULT_TRANS_TIME = value; }
        }


        /// <summary>
        /// Partial Constructor which sets the title text.
        /// <param name="pmenutitle">The Menu Title Text</param>
        /// </summary>
        public BaseGUIScreen(string pmenutitle)
            : this(pmenutitle, DEFAULT_MENU_COLOUR)
        {}

        //----------------METHOD OVERRIDES----------------------------------------------------

        /// <summary>
        /// Load the Content of this Screen
        /// </summary>
        public override void loadContent()
        {

        }

        /// <summary>
        /// Update the Screen even if we are overlaid or not the topmost screen.
        /// </summary>
        /// <param name="potherfocused">True if another screen is topmost.</param>
        /// <param name="poverlaid">True if overlaid by an overlay screen</param>
        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            // Update each nested MenuEntry object.
            for (int i = 0; i < this._list_menuitems.Count; i++)
            {
                bool tselected = this.IsActive && (i == this._selected_item);
                _list_menuitems[i].update(this, tselected);
            }
        }
        
        /// <summary>
        /// Update the Screen if we are topmost.
        /// </summary>
        public override void update()
        {
            
            // Move to the previous menu entry?
            if (this.GlobalInput.IsPressed("NAV_UP", this.ControllingPlayer))
            {
                this._selected_item--;

                if (this._selected_item < 0)
                    this._selected_item = _list_menuitems.Count - 1;
            }

            // Move to the next menu entry?
            if (this.GlobalInput.IsPressed("NAV_DOWN", this.ControllingPlayer))
            {
                this._selected_item++;

                if (this._selected_item >= _list_menuitems.Count)
                    this._selected_item = 0;
            }

            //-------------MENU ITEM ACTIONS-----------------------------------------

            if (this.GlobalInput.IsPressed("NAV_SELECT", this.ControllingPlayer))
            {
                this.OnSelectEntry(this._selected_item,ControllingPlayer);
            }
            else if (this.GlobalInput.IsPressed("NAV_RIGHT", this.ControllingPlayer))
            {
                this.OnIncrementEntry(this._selected_item, ControllingPlayer);
            }
            else if (this.GlobalInput.IsPressed("NAV_LEFT", this.ControllingPlayer))
            {
                this.OnDecrementEntry(this._selected_item, ControllingPlayer);
            }
            else if (this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer))
            {
                this.OnCancel(ControllingPlayer);
            }
        }



        /// <summary>
        /// Render the Menu Screen.
        /// </summary>
        public override void render()
        {
            this.internUpdateMenuItemPos();

            GraphicsDevice  tgd         = ScreenManager.GraphicsDevice;
            SpriteBatch     tsb         = ScreenManager.SpriteBatch;
            SpriteFont      tfont       = ScreenManager.DefaultGUIFont;

            tsb.Begin();
            for (int i = 0; i < this._list_menuitems.Count; i++)
            {
                MenuItemBasic tmenuitem = this._list_menuitems[i];
                bool tselected = IsActive && (i == this._selected_item);
                tmenuitem.render(this, tselected);
            }

            float   ttransoffset    = (float)Math.Pow(this._trans_position, 2);
            Vector2 ttitlepos       = new Vector2(tgd.Viewport.Width / 2,this._menu_title_start);
            Vector2 ttitleorigin    = tfont.MeasureString(this._menu_title_txt) / 2;
            Color   ttitlecolour    = this._menu_title_clr * this.CurrentTransitionAlpha;
            float   ttitlescale     = 1.25f;
            ttitlepos.Y -= ttransoffset * 100;
            tsb.DrawString(tfont,this._menu_title_txt, ttitlepos, ttitlecolour, 0,ttitleorigin, ttitlescale, SpriteEffects.None, 0);
            tsb.End();
        }

        /// <summary>
        /// Unload the Screen Content
        /// </summary>
        public override void unloadContent()
        {
            //No content to unload by default
        }

        //--------------------PRIVATE / PROTECTED METHODS----------------------------------------------------------

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// <param name="pitemindex">The selected item index</param>
        /// <param name="pplyrindex">The Index of the Controlling Player</param>
        /// </summary>
        protected virtual void OnSelectEntry(int pitemindex, PlayerIndex? pplyrindex)
        {
            this._list_menuitems[pitemindex].OnSelectEntry(pplyrindex);
        }

        /// <summary>
        /// Handler for when the user has decremented the menu entry
        /// </summary>
        /// <param name="pitemindex">The selected item index</param>
        /// <param name="pplyrindex">The Index of the Controlling Player</param>
        protected virtual void OnDecrementEntry(int pitemindex, PlayerIndex? pplyrindex)
        {
            this._list_menuitems[pitemindex].OnDecrementEntry(pplyrindex);
        }

        /// <summary>
        /// Handler for when the user has incremented the menu entry
        /// </summary>
        /// <param name="pitemindex">The selected item index</param>
        /// <param name="pplyrindex">The Index of the Controlling Player</param>
        protected virtual void OnIncrementEntry(int pitemindex, PlayerIndex? pplyrindex)
        {
            this._list_menuitems[pitemindex].OnIncrementEntry(pplyrindex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex? pplyrindex)
        {
            this.exitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected virtual void DefaultTriggerMenuBack(object psender, EventPlayer pargs)
        {
            this.OnCancel(pargs.PlayerIndex);
        }

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void internUpdateMenuItemPos()
        {
            float ttransoffset = (float)Math.Pow(CurrentTransitionPos, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 tbasepos = new Vector2(0f,this._menu_item_start);

            // update each menu entry's location in turn
            for (int i = 0; i < this._list_menuitems.Count; i++)
            {
                MenuItemBasic tmenuitem = _list_menuitems[i];
                
                // each entry is to be centered horizontally
                tbasepos.X = ScreenManager.GameViewport.Width / 2;

                if (this._screen_mode == ScreenMode.MODE_TRANSITION_ON)
                    tbasepos.X -= ttransoffset * 256;
                else
                    tbasepos.X += ttransoffset * 512;

                // set the entry's position
                tmenuitem.Position = tbasepos;

                // move down for the next entry the size of this entry
                tbasepos.Y += tmenuitem.getMenuItemHeight(this);
            }
        }
    }
}
