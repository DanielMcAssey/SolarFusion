using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using SolarFusion.Core;
using SolarFusion.Input;

namespace SolarFusion.Core.Screen
{
    public abstract class BaseGUIScreen : BaseScreen
    {
        //----------------CLASS CONSTANTS------------------------------------------------------
        public float DEFAULT_TRANS_TIME  = 0.5f;
        public int DEFAULT_TITLE_START   = 200;
        public int DEFAULT_MENU_START    = 350;
        public static readonly Color DEFAULT_MENU_COLOUR = new Color(192, 192, 192); 

        //----------------CLASS MEMBERS_-------------------------------------------------------
        protected List<MenuItemBasic>  _list_menuitems      = new List<MenuItemBasic>();
        protected int                      _selected_item       = 0;
        protected string                   _menu_title_txt      = String.Empty;
        protected Color                    _menu_title_clr      = Color.White;
        protected int                      _menu_title_start;
        protected int                      _menu_item_start;
        protected float _menu_item_logo_scale = 1f;
        protected string _menu_item_logo_location = "";
        protected Texture2D _menu_item_logo = null;
        protected ContentManager _content = null;
        List<AnimatedBGEntity> mAnimatedBGObjects = null;
        Random _obj_random = null;
        bool _is_animated_bg;

        //Load Support Buttons
        protected Dictionary<string, Texture2D> _menu_supportIcons;
        protected Dictionary<string, Vector2> _menu_supportPos;
        protected Dictionary<string, string> _menu_supportMsg;
        protected Dictionary<string, Vector2> _menu_supportMsgOrigin;
        protected Dictionary<string, Vector2> _menu_supportMsgPos;

        //----------------CONSTRUCTORS---------------------------------------------------------

        /// <summary>
        /// Constructor for the Screen Menu Base
        /// <param name="pmenutitle">The Menu Title Text</param>
        /// <param name="ptitleclr">The Menu Title Colour</param>
        /// <param name="pmenustart">The menu item vertical start position</param>
        /// <param name="ptitlestart">The Title vertical start position</param>
        /// <param name="ptranstime">The Transition on time</param>
        /// </summary>
        public BaseGUIScreen(string pmenutitle, Color ptitleclr, bool IsLogo, string logoLocation, bool animBG, float logoScale)
        {
            this._menu_title_txt    = pmenutitle;
            this._trans_on_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
            this._trans_off_time = TimeSpan.FromSeconds(DEFAULT_TRANS_TIME);
            this._menu_title_start = DEFAULT_TITLE_START;
            this._menu_item_start = DEFAULT_MENU_START;
            this._menu_title_clr    = ptitleclr;
            this._obj_random = new Random();
            this._menu_item_logo_scale = logoScale;
            this._is_animated_bg = animBG;

            if(IsLogo)
                this._menu_item_logo_location = logoLocation;
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
        public BaseGUIScreen(string pmenutitle, bool IsLogo, string logoLocation, bool animBG, float logoScale)
            : this(pmenutitle, DEFAULT_MENU_COLOUR, IsLogo, logoLocation, animBG, logoScale)
        {}

        //----------------METHOD OVERRIDES----------------------------------------------------

        /// <summary>
        /// Load the Content of this Screen
        /// </summary>
        public override void loadContent()
        {
            if (this._content == null)
                this._content = new ContentManager(ScreenManager.Game.Services, SysConfig.CONFIG_CONTENT_ROOT);

            if (this._menu_item_logo_location != "")
                this._menu_item_logo = this._content.Load<Texture2D>(_menu_item_logo_location);

            this._menu_supportIcons = new Dictionary<string, Texture2D>();
            this._menu_supportPos = new Dictionary<string, Vector2>();
            this._menu_supportMsg = new Dictionary<string, string>();
            this._menu_supportMsgOrigin = new Dictionary<string, Vector2>();
            this._menu_supportMsgPos = new Dictionary<string, Vector2>();

            this._menu_supportMsg.Add("GO", "OK");
            this._menu_supportMsg.Add("BACK", "CANCEL");

            this._menu_supportMsgOrigin.Add("GO", ScreenManager.DefaultGUIFont.MeasureString(this._menu_supportMsg["GO"]));
            this._menu_supportMsgOrigin.Add("BACK", ScreenManager.DefaultGUIFont.MeasureString(this._menu_supportMsg["BACK"]));

#if WINDOWS
            this._menu_supportIcons.Add("GO", this._content.Load<Texture2D>("Sprites/Misc/UI/ControlButtons/Keyboard/enter"));
            this._menu_supportIcons.Add("BACK", this._content.Load<Texture2D>("Sprites/Misc/UI/ControlButtons/Keyboard/esc"));
#else
            this._menu_supportIcons.Add("GO", this._content.Load<Texture2D>("Sprites/Misc/UI/ControlButtons/Gamepad/xboxControllerButtonA"));
            this._menu_supportIcons.Add("BACK", this._content.Load<Texture2D>("Sprites/Misc/UI/ControlButtons/Gamepad/xboxControllerButtonB"));
#endif
            
            this._menu_supportPos.Add("GO", new Vector2(((this._menu_supportIcons["GO"].Width / 2f) + 5), this.ScreenManager.GameViewport.Height - ((this._menu_supportIcons["GO"].Height / 2) + 5)));
            this._menu_supportPos.Add("BACK", new Vector2(this.ScreenManager.GameViewport.Width - ((this._menu_supportIcons["BACK"].Width / 2f) + 5), this.ScreenManager.GameViewport.Height - ((this._menu_supportIcons["BACK"].Height / 2) + 5)));

            this._menu_supportMsgPos.Add("GO", new Vector2((this._menu_supportPos["GO"].X + (this._menu_supportIcons["GO"].Width /2f)), this._menu_supportPos["GO"].Y));
            this._menu_supportMsgPos.Add("BACK", new Vector2((this._menu_supportPos["BACK"].X - (this._menu_supportIcons["BACK"].Width /2f)), this._menu_supportPos["BACK"].Y));

            if (this.mAnimatedBGObjects == null && this._is_animated_bg)
            {
                //Animated BG
                this.mAnimatedBGObjects = new List<AnimatedBGEntity>();
                bool IsSelectedUnique = false;
                for (int i = 0; i < 20; i++)
                {
                    int randItem = this._obj_random.Next(0, 2);

                    int randDirX = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                    float randPosX = 0f;
                    float randPosY = 0f;
                    int randRotDir = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                    float randRotSpeed = (float)((this._obj_random.NextDouble() * Math.Abs(0.04 - 0.01)) + 0.01); // Generate random rotation speed between 0.01 and 0.06 every frame.

                    if (randDirX == 0)
                        randPosX = (float)(this._obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                    else
                        randPosX = (float)(this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                    randPosY = (float)this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                    switch (randItem)
                    {
                        case 0: //Grandfather clock
                            this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_grandfather_clock"), 4, 1, (float)((this._obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), this._obj_random.Next(0, 4), 4, 1f, 1f, randDirX, 0, randRotDir, randRotSpeed));
                            break;
                        case 1: //Other items
                            this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_coin"), 9, 1, (float)((this._obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), this._obj_random.Next(0, 10), 20, 1f, 1f, randDirX, 0, randRotDir, randRotSpeed));
                            break;
                    }

                    if (IsSelectedUnique == false)
                    {
                        IsSelectedUnique = true;

                        int randItemUnique = this._obj_random.Next(0, 1);
                        int randDirXUnique = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                        float randPosXUnique = 0f;
                        float randPosYUnique = 0f;
                        int randRotDirUnique = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                        float randRotSpeedUnique = (float)((this._obj_random.NextDouble() * Math.Abs(0.04 - 0.01)) + 0.01); // Generate random rotation speed between 0.01 and 0.06 every frame.

                        if (randPosXUnique == 0)
                            randPosXUnique = (float)(this._obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                        else
                            randPosXUnique = (float)(this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                        randPosYUnique = (float)this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                        switch (randItemUnique)
                        {
                            case 0: //Megaman
                                this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Unique/anim_megaman"), 8, 1, (float)((_obj_random.NextDouble() * 10) - 5), new Vector2(randPosXUnique, randPosYUnique), _obj_random.Next(0, 9), 24, 1f, 1f, randDirXUnique, 0, randRotDirUnique, randRotSpeedUnique));
                                break;
                        }
                    }
                }
                //Animated BG
            }
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

            if (this.mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in this.mAnimatedBGObjects)
                {
                    entity.Update(GlobalGameTimer);

                    if (entity.RotationDirection == 0)
                        entity.Animation.Rotation += entity.RotationSpeed; //Rotate Left to Right
                    else
                        entity.Animation.Rotation -= entity.RotationSpeed; //Rotate Right to Left

                    if (entity.DirectionX == 0) //Left to Right
                    {
                        if (entity.Animation.Position.X > ScreenManager.GraphicsDevice.Viewport.Width + (entity.Animation.AnimationWidth + entity.Animation.AnimationHeight))
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X - (ScreenManager.GraphicsDevice.Viewport.Width + entity.Animation.AnimationWidth + entity.Animation.AnimationHeight + 100), entity.Animation.Position.Y);
                        }
                        else
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X + entity.GetSpeedX, entity.Animation.Position.Y);
                        }
                    }
                    else //Right to Left
                    {
                        if (entity.Animation.Position.X < 0 - (entity.Animation.AnimationWidth + entity.Animation.AnimationHeight))
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X + (ScreenManager.GraphicsDevice.Viewport.Width + entity.Animation.AnimationWidth + entity.Animation.AnimationHeight + 100), entity.Animation.Position.Y);
                        }
                        else
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X - entity.GetSpeedX, entity.Animation.Position.Y);
                        }
                    }
                }
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
            Color           _alphaMixer = Color.White;
            _alphaMixer *= this.CurrentTransitionAlpha;

            tsb.Begin();
            if (mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in mAnimatedBGObjects)
                {
                    entity.Draw(tsb, this.CurrentTransitionAlpha);
                }
            }

            for (int i = 0; i < this._list_menuitems.Count; i++)
            {
                MenuItemBasic tmenuitem = this._list_menuitems[i];
                bool tselected = IsActive && (i == this._selected_item);
                tmenuitem.render(this, tselected);
            }

            float   ttransoffset    = (float)Math.Pow(this._trans_position, 2);

            Vector2 ttitlepos = Vector2.Zero;
            Vector2 ttitleorigin = Vector2.Zero;
            Color ttitlecolour = Color.Black;

            ttitleorigin = tfont.MeasureString(this._menu_title_txt) / 2;
            ttitlecolour = this._menu_title_clr * this.CurrentTransitionAlpha;
            ttitlepos.Y -= ttransoffset * 100;

            if (_menu_item_logo == null)
                ttitlepos = new Vector2(tgd.Viewport.Width / 2, this._menu_title_start);
            else
            {
                ttitlepos = new Vector2(tgd.Viewport.Width / 2, this._menu_title_start);
                ttitleorigin = new Vector2(this._menu_item_logo.Width / 2, this._menu_item_logo.Height / 2);
            }

            if (_menu_item_logo == null)
                tsb.DrawString(tfont, this._menu_title_txt, ttitlepos, ttitlecolour, 0, ttitleorigin, this._menu_item_logo_scale, SpriteEffects.None, 0);
            else
            {
                Rectangle tlogorec = new Rectangle(tgd.Viewport.Width / 2, this._menu_title_start, _menu_item_logo.Width, _menu_item_logo.Height);
                tsb.Draw(this._menu_item_logo, ttitlepos, null, ttitlecolour, 0f, ttitleorigin, this._menu_item_logo_scale, SpriteEffects.None, 0);
            }

            tsb.Draw(this._menu_supportIcons["BACK"], this._menu_supportPos["BACK"], null, _alphaMixer, 0f, new Vector2(this._menu_supportIcons["BACK"].Width / 2f, this._menu_supportIcons["BACK"].Height / 2f), 0.5f, SpriteEffects.None, 0);
            tsb.DrawString(tfont, this._menu_supportMsg["BACK"], this._menu_supportMsgPos["BACK"], _alphaMixer, 0, new Vector2(this._menu_supportMsgOrigin["BACK"].X, this._menu_supportMsgOrigin["BACK"].Y / 2f), 0.5f, SpriteEffects.None, 0);

            tsb.Draw(this._menu_supportIcons["GO"], this._menu_supportPos["GO"], null, _alphaMixer, 0f, new Vector2(this._menu_supportIcons["GO"].Width / 2f, this._menu_supportIcons["GO"].Height / 2f), 0.5f, SpriteEffects.None, 0);
            tsb.DrawString(tfont, this._menu_supportMsg["GO"], this._menu_supportMsgPos["GO"], _alphaMixer, 0, new Vector2(0, this._menu_supportMsgOrigin["GO"].Y / 2f), 0.5f, SpriteEffects.None, 0);

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
            if (this._list_menuitems.Count > 0)
                this._list_menuitems[pitemindex].OnSelectEntry(pplyrindex);
        }

        /// <summary>
        /// Handler for when the user has decremented the menu entry
        /// </summary>
        /// <param name="pitemindex">The selected item index</param>
        /// <param name="pplyrindex">The Index of the Controlling Player</param>
        protected virtual void OnDecrementEntry(int pitemindex, PlayerIndex? pplyrindex)
        {
            if (this._list_menuitems.Count > 0)
                this._list_menuitems[pitemindex].OnDecrementEntry(pplyrindex);
        }

        /// <summary>
        /// Handler for when the user has incremented the menu entry
        /// </summary>
        /// <param name="pitemindex">The selected item index</param>
        /// <param name="pplyrindex">The Index of the Controlling Player</param>
        protected virtual void OnIncrementEntry(int pitemindex, PlayerIndex? pplyrindex)
        {
            if (this._list_menuitems.Count > 0)
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
