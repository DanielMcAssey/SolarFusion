using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SolarFusion.Core;
using SolarFusion.Screen.System;

namespace SolarFusion.Screen.GUIScreens
{
    class ScreenMenuRoot : BaseGUIScreen
    {
        List<AnimatedBGEntity> mAnimatedBGObjects = null;
        ContentManager _content = null;
        Random _obj_random = null;

        public ScreenMenuRoot()
            : base("Root_Menu")
        {
            _obj_random = new Random();

            MenuItemBasic mi_play = new MenuItemBasic("Play");
            MenuItemBasic mi_options = new MenuItemBasic("Options");
            MenuItemBasic mi_credits = new MenuItemBasic("Credits");
            MenuItemBasic mi_exit = new MenuItemBasic("Exit");

            mi_play.OnSelected += EventTriggerGoToCharSelect;
            mi_options.OnSelected += EventTriggerGoToOptions;
            mi_credits.OnSelected += EventTriggerGoToCredits;
            mi_exit.OnSelected += DefaultTriggerMenuBack;

            this._list_menuitems.Add(mi_play);
            this._list_menuitems.Add(mi_options);
            this._list_menuitems.Add(mi_credits);
            this._list_menuitems.Add(mi_exit);
        }

        public override void loadContent()
        {
            if (this._content == null)
                this._content = new ContentManager(ScreenManager.Game.Services, SysConfig.CONFIG_CONTENT_ROOT);

            mAnimatedBGObjects = new List<AnimatedBGEntity>();

            bool IsSelectedUnique = false;  

            for (int i = 0; i < 20; i++)
            {
                int randItem = _obj_random.Next(0, 2);

                int randDirX = _obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                int randDirY = _obj_random.Next(0, 2); // 0 = Up to Down, 1 = Down to Up
                float randPosX = 0f;
                float randPosY = 0f;

                if(randDirX == 0)
                    randPosX = (float)(_obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                else
                    randPosX = (float)(_obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                randPosY = (float)_obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                switch (randItem)
                {
                    case 0: //Grandfather clock
                        mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_grandfather_clock"), 4, 1, (float)((_obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), _obj_random.Next(0, 4), 4, 1f, 1f, randDirX, randDirY));
                        break;
                    case 1: //Other items
                        mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_coin"), 9, 1, (float)((_obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), _obj_random.Next(0, 10), 20, 1f, 1f, randDirX, randDirY));
                        break;
                }

                if (IsSelectedUnique == false)
                {
                    IsSelectedUnique = true;
                    int randItemUnique = _obj_random.Next(0, 1);
                    int randDirXUnique = _obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                    int randDirYUnique = _obj_random.Next(0, 2); // 0 = Up to Down, 1 = Down to Up
                    float randPosXUnique = 0f;
                    float randPosYUnique = 0f;

                    if (randPosXUnique == 0)
                        randPosXUnique = (float)(_obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                    else
                        randPosXUnique = (float)(_obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                    randPosYUnique = (float)_obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                    switch (randItemUnique)
                    {
                        case 0: //Megaman
                            mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Unique/anim_megaman"), 8, 1, (float)((_obj_random.NextDouble() * 10) - 5), new Vector2(randPosXUnique, randPosYUnique), _obj_random.Next(0, 9), 24, 1f, 1f, randDirXUnique, randDirYUnique));
                            break;
                    }
                }

            }

            base.loadContent();
        }

        public override void update()
        {
            if (mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in mAnimatedBGObjects)
                {
                    entity.Update(GlobalGameTimer);
                    entity.Animation.Rotation += 0.01f;

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

                    if (entity.DirectionY == 0) //Up to Down
                    {
                        if (entity.Animation.Position.Y > ScreenManager.GraphicsDevice.Viewport.Height + (entity.Animation.AnimationHeight + entity.Animation.AnimationWidth))
                        {
                            entity.DirectionY = 1;
                        }
                        else
                        {
                            //entity.Animation.Position = new Vector2(entity.Animation.Position.X, entity.Animation.Position.Y - entity.GetSpeedY);
                        }
                    }
                    else //Down to Up
                    {
                        if (entity.Animation.Position.Y < 0 - (entity.Animation.AnimationHeight + entity.Animation.AnimationWidth))
                        {
                            entity.DirectionY = 0;
                        }
                        else
                        {
                            //entity.Animation.Position = new Vector2(entity.Animation.Position.X, entity.Animation.Position.Y + entity.GetSpeedY);
                        }
                    }
                }
            }

            base.update();
        }

        public override void render()
        {
            GraphicsDevice tgd = ScreenManager.GraphicsDevice;
            SpriteBatch tsb = ScreenManager.SpriteBatch;
            tsb.Begin();

            if (mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in mAnimatedBGObjects)
                {
                    entity.Draw(tsb);
                }
            }

            tsb.End();
            
            base.render();
        }

        //---------------EVENT HANDLERS-------------------------------------------------------------

        /// <summary>
        /// Event Handler to Go to character select screen.
        /// </summary>
        void EventTriggerGoToCharSelect(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS2(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Options Screen.
        /// </summary>
        void EventTriggerGoToOptions(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS3(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Credits Screen.
        /// </summary>
        void EventTriggerGoToCredits(object sender, EventPlayer e)
        {
            //ScreenManager.addScreen(new LabScreenMenuS3(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex? pplyrindex)
        {
            const string tmessage = "Are you sure you want to exit this test?";
            //LJMUBaseScreenMsgBox tmsgbox = new LJMUBaseScreenMsgBox(LabConfig.LAB_ASSET_GUI_MSGBOX_BG, tmessage);

            //tmsgbox.onAccepted += EventTriggerMsgBoxConfirm;
            //ScreenManager.addScreen(tmsgbox, pplyrindex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void EventTriggerMsgBoxConfirm(object sender, EventPlayer e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
