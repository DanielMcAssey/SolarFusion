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
using SolarFusion.Core.PostProcessing;

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
            this._obj_levelmanager = new LevelManager(this.GlobalContentManager, this.ScreenManager.GraphicsDevice, this.GlobalInput, this.ControllingPlayer);
            this._obj_entitymanager = new EntityManager(this.GlobalContentManager);

            //!Debug!
            AnimatedSprite _tmpPlayerAnim = new AnimatedSprite(this._local_content.Load<Texture2D>("Sprites/Characters/Jumpista/spritesheet"), 5, 3);
            _tmpPlayerAnim.AddAnimation("right", 1, 5, 7);
            _tmpPlayerAnim.AddAnimation("left", 2, 5, 7);
            _tmpPlayerAnim.AddAnimation("idle", 3, 3, 5);
            _tmpPlayerAnim.mCurrentAnimation = "idle";
            _tmpPlayerAnim.Loop = true;
            //!Debug!

            this._obj_activeplayer = new Player(this._obj_entitymanager.NextID(), _tmpPlayerAnim, Vector2.Zero, 1.8f, 100, 280, this._obj_entitymanager);
            this._obj_levelmanager.LoadLevel(1, this._obj_activeplayer, this._obj_entitymanager); //Load Test Level 0
            this.BGColour = Color.Blue;
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

            this._obj_levelmanager.Update(_gameTimer);

            if (this.GlobalInput.IsPressed("PLAY_MOVE_LEFT", this.ControllingPlayer)) //If player presses left button (Left Arrow/Left DPAD)
                this._obj_activeplayer.moveLeft();
            else if (this.GlobalInput.IsPressed("PLAY_MOVE_RIGHT", this.ControllingPlayer)) //If player presses left button (Right Arrow/Right DPAD)
                this._obj_activeplayer.moveRight();
            else
                this._obj_activeplayer.moveIdle();

            if (this.GlobalInput.IsPressed("PLAY_MOVE_JUMP", this.ControllingPlayer)) //If player presses the jump button (Spacebar/A)
                this._obj_activeplayer.jump();

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
