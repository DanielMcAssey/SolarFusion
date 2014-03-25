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

        public ScreenGame(Player _player, EntityManager _entitym)
        {
            this._obj_activeplayer = _player;
            this._obj_entitymanager = _entitym;
        }

        public override void loadContent()
        {
            base.loadContent();
            this.BGColour = Color.Black;
            this._obj_levelmanager = new LevelManager(this.GlobalContentManager, this.ScreenManager.GraphicsDevice, this.GlobalInput, this.ControllingPlayer, this.ScreenManager, this.ScreenManager.DefaultGUIFont);
            this._obj_levelmanager.LoadLevel(1, this._obj_activeplayer, this._obj_entitymanager); //Load Level
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

            if (this.GlobalInput.IsPressed("PLAY_WEAPON_FIRE", this.ControllingPlayer)) //If player presses the jump button (Spacebar/A)
                this._obj_activeplayer.fire();

            if (this.GlobalInput.IsPressed("GLOBAL_DEBUG", this.ControllingPlayer)) //If player presses the jump button (Spacebar/A)
                this._obj_levelmanager.Debug = !this._obj_levelmanager.Debug;

            base.update();
        }

        public override void appRender() //Render per frame
        {
            this.internResetRenderStatesFor2D(); //Reset 2D states
            this._obj_levelmanager.Draw(this.ScreenManager.SpriteBatch);
        }
    }
}
