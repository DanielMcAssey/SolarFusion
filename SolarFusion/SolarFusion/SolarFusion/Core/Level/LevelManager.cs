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

namespace SolarFusion.Level
{
    public class LevelManager
    {
        private ContentManager _obj_contentmanager = null;
        private Viewport _obj_viewport;
        private Camera2D _obj_camera = null;
        private LevelTilemap _obj_map = null;
        private EntityManager _obj_entitymanager = null;
        private uint _current_level_id = 0;
        private Player player;
        
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

        public LevelManager(ContentManager _contentmanager, Viewport _viewport)
        {
            this._obj_contentmanager = _contentmanager;
            this._obj_viewport = _viewport;
        }

        public void LoadLevel(uint _LevelID, Player _activePlayer, EntityManager _objManager)
        {
            this._obj_map = this._obj_contentmanager.Load<LevelTilemap>("Levels/level_" + _LevelID.ToString() + "/level"); //Load level data from selected level.
            this._obj_map.LoadContent(this._obj_contentmanager);
            this._current_level_id = _LevelID;

            for (int i = 0; i < this._obj_map.tmGameEntityGroupCount; i++) //Loop over the amount of objects in the level and load them.
            {
                for (int j = 0; j < this._obj_map.tmGameEntityGroups[i].GameEntityData.Count(); j++)
                {
                    GameEntity goData = this._obj_map.tmGameEntityGroups[i].GameEntityData[j];
                    Vector2 position = new Vector2(goData.entPosition.Center.X, goData.entPosition.Center.Y);
                    GameObjects go;

                    player = _activePlayer; //Assign instances to local class variables.
                    this._obj_entitymanager = _objManager;
                    switch (goData.entCategory) //Swtich by object category.
                    {
                        case "PlayerStart":
                            Vector2 newPos = new Vector2(position.X, (position.Y - ((player.Height / 2) / 2)));
                            player.Position = newPos;
                            player.floorHeight = position.Y + ((player.Height / 2) / 2);
                            player.isSingleplayer = true;
                            player.LayerDepth = this._obj_map.tmGameEntityGroups[i].LayerDepth;
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

            this._obj_camera = new Camera2D(this._obj_viewport);
            this._obj_camera.Position = new Vector2(this._obj_viewport.Width / 2f, this._obj_viewport.Height / 2f);
            this._obj_camera.Zoom = 1.0f;
            this._obj_camera.Speed = 60f;
            this._obj_entitymanager.camera = this._obj_camera;
        }

        public void UnloadLevel()
        {
            this._obj_map = null;
        }

        public void Update(GameTime _gameTime)
        {
            float timeDiff = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            foreach (uint goID in this._obj_entitymanager.dynamicObjects) //Checks all the dynamic objects in the level, and loops through updating them.
            {
                GameObjects go = this._obj_entitymanager.GetObject(goID); //Gets the game object.
                this._obj_entitymanager.UpdateGameObject(goID); //Updates the game object.
            }

            if ((player.Position.X - (player.Width / 2f)) < (this._obj_camera.Position.X - (this._obj_viewport.Width / 2))) //Check if player is in the camera.
            {
                player.Position = new Vector2(((this._obj_camera.Position.X - (this._obj_viewport.Width / 2)) + (player.Width / 2f)), player.Position.Y); //Re-adjust position if player is outside camera.
            }

            if (player.Position.X > (this._obj_camera.Position.X + (this._obj_viewport.Width / 2))) //Check if player is in the camera.
            {
                player.Position = new Vector2((this._obj_camera.Position.X + (this._obj_viewport.Width / 2)), player.Position.Y); //Re-adjust position if player is outside camera.
            }

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

            player.Update(_gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, this._obj_camera.calculateTransform());

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
                            spriteBatch.Draw(this._obj_map.tmTextures[tile.textureID], onScreen, tile.source, Color.White, 0f, new Vector2(0, this._obj_map.tmTileHeight / 2), tileData.SpriteEffects, this._obj_map.tmLayers[i].LayerDepth); //Draw the tile.
                        }
                    }
                }
            }

            Rectangle bounds = new Rectangle((int)(this._obj_camera.Position.X - (this._obj_viewport.Width / 2f)) - 100, 0, (((this._obj_viewport.Width / this._obj_map.tmTileWidth) * this._obj_map.tmTileWidth) + 300), this._obj_map.tmHeight * this._obj_map.tmTileHeight); //Calculates what objects need to be drawn.
            foreach (uint goID in _obj_entitymanager.QueryRegion(bounds)) //Checks what objects are in the calculated boundry.
            {
                GameObjects go = _obj_entitymanager.GetObject(goID);

                if (go.Hidden == false)
                    go.Draw(spriteBatch); //Draws The Object.
            }

            player.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
