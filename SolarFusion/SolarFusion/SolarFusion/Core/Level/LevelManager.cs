using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameData;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Project
using SolarFusion.Core;
using SolarFusion.Input;
using SolarFusion.Core.Screen;
using SolarFusion.Core.PostProcessing;

namespace SolarFusion.Level
{
    public class LevelManager
    {
        private InputManager _obj_input = null;
        private ContentManager _obj_contentmanager = null;
        private GraphicsDevice _obj_graphics = null;
        private Viewport _obj_viewport;
        private PostProcessingManager _obj_ppmanager = null;
        private Camera2D _obj_camera = null;
        private LevelTilemap _obj_map = null;
        private EntityManager _obj_entitymanager = null;
        private RenderTarget2D _obj_scene = null;
        private Player _obj_player = null;
        private GameGUI _obj_gui = null;

        private uint _current_level_id = 0;
        private PlayerIndex? mControllingPlayer;
        private bool isScrolling = false;

        // Effects
        private CrepuscularRays _effect_sun = null;
        private Vector2 _effect_sun_pos = Vector2.Zero;
        private Color _effect_sky_color = Color.Black;
        // !Effects

        // Properties
        #region "Properties"
        public uint CurrentGameLevelID
        {
            get { return this._current_level_id; }
        }

        public LevelTilemap CurrentGameMap
        {
            get { return this._obj_map; }
        }
        #endregion
        // !Properties

        public LevelManager(ContentManager _contentmanager, GraphicsDevice _graphics, InputManager _input, PlayerIndex? _controllingPlayer)
        {
            this._obj_contentmanager = _contentmanager;
            this._obj_viewport = _graphics.Viewport;
            this._obj_graphics = _graphics;
            this._obj_input = _input;
            this._obj_ppmanager = new PostProcessingManager(this._obj_graphics);
            this._obj_gui = new GameGUI(this._obj_graphics);
            this.mControllingPlayer = _controllingPlayer;
        }

        public void LoadLevel(uint _LevelID, Player _activePlayer, EntityManager _objManager)
        {
            this._obj_map = this._obj_contentmanager.Load<LevelTilemap>("Levels/level_" + _LevelID.ToString() + "/level"); //Load level data from selected level.
            this._obj_map.LoadContent(this._obj_contentmanager);
            this._current_level_id = _LevelID;

            // Load Level Objects
            for (int i = 0; i < this._obj_map.tmGameEntityGroupCount; i++) //Loop over the amount of objects in the level and load them.
            {
                for (int j = 0; j < this._obj_map.tmGameEntityGroups[i].GameEntityData.Count(); j++)
                {
                    GameEntity goData = this._obj_map.tmGameEntityGroups[i].GameEntityData[j];
                    Vector2 position = new Vector2(goData.entPosition.Center.X, goData.entPosition.Center.Y);
                    GameObjects go;

                    this._obj_player = _activePlayer; //Assign instances to local class variables.
                    this._obj_entitymanager = _objManager;
                    switch (goData.entCategory) //Swtich by object category.
                    {
                        case "PlayerStart":
                            Vector2 newPos = new Vector2(position.X, position.Y + ((this._obj_player.Height / 2) / 2));
                            this._obj_player.Position = newPos;
                            this._obj_player.floorHeight = position.Y + ((this._obj_player.Height / 2) / 2);
                            this._obj_player.isSingleplayer = true;
                            this._obj_player.LayerDepth = this._obj_map.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "Powerup": //NEED TO FIX
                            //go = this._obj_entitymanager.CreatePowerup((PowerUpType)Enum.Parse(typeof(PowerUpType), goData.entType, true), position);
                            //this._obj_map.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            //go.LayerDepth = this._obj_map.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "Enemy": //NEED TO FIX
                            //go = this._obj_entitymanager.CreateEnemy((EnemyType)Enum.Parse(typeof(EnemyType), goData.entType, true), position);
                            //this._obj_map.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            //go.LayerDepth = this._obj_map.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "LevelObject":
                            go = this._obj_entitymanager.CreateLevelObject((LevelObjectType)Enum.Parse(typeof(LevelObjectType), goData.entType, true), position);
                            this._obj_map.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            go.defaultBounds = goData.entPosition;
                            go.LayerDepth = this._obj_map.tmGameEntityGroups[i].LayerDepth;
                            break;
                    }
                }
            }
            // !Load Level Objects

            this._effect_sun = new CrepuscularRays(this._obj_graphics, this._effect_sun_pos, 1.5f, 0.97f, 0.97f, 0.1f, 0.25f, this._obj_contentmanager.Load<Effect>("Core/Shaders/PostProcessing/LightSourceMask"), this._obj_contentmanager.Load<Texture2D>("Core/Textures/sun_flare"), this._obj_contentmanager.Load<Effect>("Core/Shaders/PostProcessing/LigthRays"));
            this._effect_sun_pos = new Vector2(0.2f, 0.35f);
            this._obj_ppmanager.AddEffect(this._effect_sun);
            this._obj_scene = new RenderTarget2D(this._obj_graphics, this._obj_graphics.Viewport.Width, this._obj_graphics.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            this._effect_sky_color = new Color(135, 206, 250);

            this._obj_camera = new Camera2D(this._obj_viewport);
            this._obj_camera.Position = new Vector2(this._obj_viewport.Width / 2f, this._obj_viewport.Height / 2f);
            this._obj_camera.Zoom = 1.0f;
            this._obj_camera.Speed = 60f;
            this._obj_entitymanager.camera = this._obj_camera;
            this._obj_gui.Load(this._obj_contentmanager);


            this.isScrolling = true;
        }

        public void UnloadLevel()
        {
            this._obj_camera = null;
            this._obj_map = null;
            this._obj_entitymanager = null;
            this._obj_scene = null;
            this._obj_player = null;
            this._obj_gui.Unload();
        }

        public void Update(GameTime _gameTime)
        {
            float timeDiff = (float)_gameTime.ElapsedGameTime.TotalSeconds;
            this._effect_sun.LightSource = this._effect_sun_pos;
            this._obj_entitymanager.Update(_gameTime);

            if ((this._obj_camera.Position.X + (this._obj_viewport.Width / 2)) >= ((this._obj_map.tmWidth * this._obj_map.tmTileWidth) - 10))
                this.isScrolling = false; //If player reaches the end of the map, stop the scrolling.

            if (this.isScrolling) //If the map is scrolling, do the following:
            {
                float scrollDelta = (float)_gameTime.ElapsedGameTime.TotalSeconds * this._obj_camera.Speed; //Gets delta to increment camera position.
                this._obj_camera.Position += new Vector2(scrollDelta, 0); //Increments the camera speed.
            }

            foreach (uint goID in this._obj_entitymanager.dynamicObjects) //Checks all the dynamic objects in the level, and loops through updating them.
            {
                GameObjects go = this._obj_entitymanager.GetObject(goID); //Gets the game object.
                this._obj_entitymanager.UpdateGameObject(goID); //Updates the game object.
            }

            if ((this._obj_player.Position.X - (this._obj_player.Width / 2f)) < (this._obj_camera.Position.X - (this._obj_viewport.Width / 2))) //Check if player is in the camera.
                this._obj_player.Position = new Vector2(((this._obj_camera.Position.X - (this._obj_viewport.Width / 2)) + (this._obj_player.Width / 2f)), this._obj_player.Position.Y); //Re-adjust position if player is outside camera.

            if (this._obj_player.Position.X > (this._obj_camera.Position.X + (this._obj_viewport.Width / 2))) //Check if player is in the camera.
                this._obj_player.Position = new Vector2((this._obj_camera.Position.X + (this._obj_viewport.Width / 2)), this._obj_player.Position.Y); //Re-adjust position if player is outside camera.

            Rectangle bounds = new Rectangle((int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2f)) - 500, 0, this._obj_map.tmWidth * this._obj_map.tmTileWidth, this._obj_map.tmHeight * this._obj_map.tmTileHeight); //Sets bounds for creating objects.
            foreach (uint goID in this._obj_entitymanager.QueryRegion(bounds)) //Checks if any objects are in the bounds, and loops through updating them.
            {
                GameObjects go = this._obj_entitymanager.GetObject(goID); //Gets the object if they are in bounds.

                if (go.Hidden == false) //Make sure the object isnt hidden.
                {
                    if (go.ObjectType == ObjectType.Enemy)
                    {
                        AI bot = (AI)this._obj_entitymanager.GetObject(goID); //Gets the AI object

                        switch (bot.moveDirection) //Switches the bots current direction, and moves that way.
                        {
                            case MoveDirection.Left:
                                bot.moveLeft();
                                break;
                            case MoveDirection.Right:
                                bot.moveRight();
                                break;
                        }

                        foreach (uint goID1 in this._obj_entitymanager.QueryRegion(go.Bounds)) //Collision detection for player, checks the player if its colliding with anything.
                        {
                            GameObjects go1 = this._obj_entitymanager.GetObject(goID1); //Gets the

                            if (go1.ObjectType == ObjectType.LevelObject)
                            {
                                if (go.Bounds.Intersects(go1.Bounds))
                                {
                                    switch (bot.moveDirection) //Checks bot direction.
                                    {
                                        case MoveDirection.Right: //AI Direction is right so, we change it to left.
                                            bot.moveDirection = MoveDirection.Left;
                                            break;
                                        case MoveDirection.Left:
                                            bot.moveDirection = MoveDirection.Right;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    go.Update(_gameTime); //Updates the object.
                    this._obj_entitymanager.UpdateGameObject(goID); //Updates the object within the object manager.
                }
            }

            Rectangle deleteBounds = new Rectangle((int)((this._obj_camera.Position.X - (this._obj_viewport.Width / 2f)) - 50), 0, -4480, this._obj_map.tmHeight * this._obj_map.tmTileHeight); //Sets bounds for deleting objects.
            foreach (uint goID in this._obj_entitymanager.QueryRegion(deleteBounds)) //Checks if any objects are in the bounds, and loops through deleting them.
                this._obj_entitymanager.DestroyObject(goID); //Delets the object if its in the bounds.

            this._obj_player.Update(_gameTime);
            this._obj_gui.Update(_gameTime);
        }

        public void Draw(SpriteBatch _sb)
        {
            //Create a mask for the occlusion ray
            this._obj_graphics.SetRenderTarget(this._obj_scene);
            this._obj_graphics.Clear(Color.White);
            _sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, null, this._obj_camera.calculateTransform());
            this.DrawLevel(_sb);
            this._obj_player.Draw(_sb);
            _sb.End();
            this._obj_graphics.SetRenderTarget(null);

            //Apply Post Processing Effects
            this._obj_ppmanager.Draw(_sb, this._obj_scene);

            //Draw Scene in Colour
            this._obj_graphics.SetRenderTarget(this._obj_scene);
            this._obj_graphics.Clear(this._effect_sky_color); //Background Colour
            _sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, null, this._obj_camera.calculateTransform());
            this.DrawLevel(_sb);
            this._obj_player.Draw(_sb);
            _sb.End();
            this._obj_graphics.SetRenderTarget(null);
            this._obj_graphics.Clear(Color.Black);

            //Draw the post processing effects
            _sb.Begin(SpriteSortMode.BackToFront, BlendState.Additive, SamplerState.LinearClamp, null, null, null, this._obj_camera.calculateTransform());
            _sb.Draw(this._obj_ppmanager.mScene, new Rectangle((int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2)), 0, this._obj_graphics.Viewport.Width, this._obj_graphics.Viewport.Height), Color.White);
            _sb.Draw(this._obj_scene, new Rectangle((int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2)), 0, this._obj_graphics.Viewport.Width, this._obj_graphics.Viewport.Height), Color.White);
            _sb.End();

            _sb.Begin();
            this._obj_gui.Draw(_sb);
            _sb.End();
        }

        private void DrawLevel(SpriteBatch _sb)
        {
            int minY = (int)(this._obj_camera.Position.Y - (this._obj_viewport.Height / 2f)) / this._obj_map.tmTileHeight;
            int minX = (int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2f)) / this._obj_map.tmTileWidth;

            if (minY < 0)
                minY = 0;

            if (minX < 0)
                minX = 0;

            int maxX = minX + (this._obj_viewport.Width / this._obj_map.tmTileWidth) + 2;
            int maxY = this._obj_map.tmHeight;

            if (maxX >= this._obj_map.tmWidth)
                maxX = this._obj_map.tmWidth;

            for (int i = 0; i < this._obj_map.tmLayerCount; i++) //Draws on layer by layer basis.
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int x = minX; x < maxX; x++) //Draws tiles based on their ID, which MinX means the first tile shown on the screen, and MaxX is the last tile.
                    {
                        int index = x + y * this._obj_map.tmWidth; //Calculates what tile to draw.
                        LevelTileData tileData = this._obj_map.tmLayers[i].TileData[index]; //Gets the data for that tile.
                        if (tileData.TileID != 0) //If there is no data, dont bother drawing.
                        {
                            LevelTile tile = this._obj_map.tmTiles[tileData.TileID - 1]; //Gets the tile data.
                            Rectangle onScreen = new Rectangle(x * this._obj_map.tmTileWidth, (y * this._obj_map.tmTileHeight) + (this._obj_map.tmTileHeight / 2), this._obj_map.tmTileWidth, this._obj_map.tmTileHeight); //Calculates where on screen the tile has to be shown.
                            _sb.Draw(this._obj_map.tmTextures[tile.textureID], onScreen, tile.source, Color.White, 0f, new Vector2(0, this._obj_map.tmTileHeight / 2), tileData.SpriteEffects, this._obj_map.tmLayers[i].LayerDepth); //Draw the tile.
                        }
                    }
                }
            }

            Rectangle bounds = new Rectangle((int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2f)) - 100, 0, (((this._obj_viewport.Width / this._obj_map.tmTileWidth) * this._obj_map.tmTileWidth) + 300), this._obj_map.tmHeight * this._obj_map.tmTileHeight); //Calculates what objects need to be drawn.
            foreach (uint goID in _obj_entitymanager.QueryRegion(bounds)) //Checks what objects are in the calculated boundry.
            {
                GameObjects go = _obj_entitymanager.GetObject(goID);

                if (go.Hidden == false)
                    go.Draw(_sb); //Draws The Object.
            }
        }
    }
}
