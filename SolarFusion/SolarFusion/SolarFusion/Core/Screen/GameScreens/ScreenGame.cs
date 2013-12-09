using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SolarFusion.Core;
using SolarFusion.Input;
using SolarFusion.Level;

namespace SolarFusion.Core.Screen
{
    public class ScreenGame : AppScreen
    {
        private EntityManager _obj_entitymanager = null;
        private LevelManager _obj_levelmanager = null;
        private Player _obj_activeplayer = null;

        public override void loadContent()
        {
            base.loadContent();
            this.BGColour = Color.Black;
            this._obj_levelmanager = new LevelManager(this.GlobalContentManager, this.ScreenManager.GraphicsDevice.Viewport);
            this._obj_entitymanager = new EntityManager(this.GlobalContentManager);

            //!Debug!
            AnimatedSprite _tmpPlayerAnim = new AnimatedSprite(this._local_content.Load<Texture2D>("Sprites/Characters/Jumpista/spritesheet"), 5, 3);
            _tmpPlayerAnim.AddAnimation("right", 0, 5, 7);
            _tmpPlayerAnim.AddAnimation("left", 1, 5, 7);
            _tmpPlayerAnim.AddAnimation("idle", 2, 3, 5);
            //!Debug!

            this._obj_activeplayer = new Player(this._obj_entitymanager.NextID(), _tmpPlayerAnim, Vector2.Zero, 1.8f, 100, 280, this._obj_entitymanager);
            this._obj_levelmanager.LoadLevel(1, this._obj_activeplayer, this._obj_entitymanager); //Load Test Level 0
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
            GameTime _gameTimer = this.GlobalGameTimer;
            TimeSpan _elapsedTime = _gameTimer.ElapsedGameTime;
            TimeSpan _totalTime = _gameTimer.TotalGameTime;
            


            base.update();
        }

        public override void appRender() //Render per frame
        {
            this.internResetRenderStatesFor2D(); //Reset 2D states

            this.ScreenManager.SpriteBatch.Begin();
            //Render HUD
            this.ScreenManager.SpriteBatch.End();

            this._obj_levelmanager.Draw(this.ScreenManager.SpriteBatch);
        }
    }
}
