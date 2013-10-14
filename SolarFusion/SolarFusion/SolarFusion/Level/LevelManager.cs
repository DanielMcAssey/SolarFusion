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

namespace SolarFusion.Level
{
    public class LevelManager
    {
        private ContentManager _obj_contentmanager = null;
        private LevelTilemap _obj_currentmap = null;
        private uint _current_level_id = 0;

        // Properties
        #region "Properties"
        public uint CurrentGameLevelID
        {
            get { return this._current_level_id; }
        }

        public LevelTilemap CurrentGameMap
        {
            get { return this._obj_currentmap; }
        }
        #endregion
        // !Properties

        public LevelManager(ContentManager mContent)
        {
            this._obj_contentmanager = mContent;
        }

        public void LoadLevel(uint mLEVELID)
        {
            _obj_currentmap = this._obj_contentmanager.Load<LevelTilemap>("Levels/level_" + mLEVELID.ToString() + "/data"); //Load level data from selected level.
            _obj_currentmap.LoadContent(this._obj_contentmanager);
            this._current_level_id = mLEVELID;

            for (int i = 0; i < _obj_currentmap.tmGameEntityGroupCount; i++) //Loop over the amount of objects in the level and load them.
            {
                for (int j = 0; j < _obj_currentmap.tmGameEntityGroups[i].GameEntityData.Count(); j++)
                {
                    GameEntity goData = _obj_currentmap.tmGameEntityGroups[i].GameEntityData[j];
                    Vector2 position = new Vector2(goData.entPosition.Center.X, goData.entPosition.Center.Y);
                    GameObjects go;

                    switch (goData.entCategory) //Swtich by object category.
                    {
                        case "PlayerStart":
                            Vector2 newPos = new Vector2(position.X, position.Y + ((player.Height / 2) / 2));
                            player.Position = newPos;
                            player.floorHeight = position.Y + ((player.Height / 2) / 2);
                            player.isSingleplayer = true;
                            player.LayerDepth = CurrentMap.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "Powerup":
                            go = mObjectManager.CreatePowerup((PowerupType)Enum.Parse(typeof(PowerupType), goData.entType), position);
                            _obj_currentmap.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            go.LayerDepth = _obj_currentmap.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "Enemy":
                            go = mObjectManager.CreateEnemy((EnemyType)Enum.Parse(typeof(EnemyType), goData.entType), position);
                            _obj_currentmap.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            go.LayerDepth = _obj_currentmap.tmGameEntityGroups[i].LayerDepth;
                            break;
                        case "StartScroll":
                            startScroll.Add(new Rectangle((int)goData.entPosition.X, goData.entPosition.Y, goData.entPosition.Width, goData.entPosition.Height));
                            break;
                        case "EndScroll":
                            endScroll.Add(new Rectangle((int)goData.entPosition.X, goData.entPosition.Y, goData.entPosition.Width, goData.entPosition.Height));
                            break;
                        case "End_Area":
                            endAreas.Add(new Rectangle((int)goData.entPosition.X, goData.entPosition.Y, goData.entPosition.Width, goData.entPosition.Height));
                            break;
                        case "Wall":
                            wallAreas.Add(new Rectangle((int)goData.entPosition.X, goData.entPosition.Y, goData.entPosition.Width, goData.entPosition.Height));
                            break;
                        case "LevelObject":
                            go = mObjectManager.CreateLevelObject((LevelObjectType)Enum.Parse(typeof(LevelObjectType), goData.entType), position);
                            _obj_currentmap.tmGameEntityGroups[i].GameEntityData[j].entID = go.ID;
                            go.defaultBounds = goData.entPosition;
                            go.LayerDepth = _obj_currentmap.tmGameEntityGroups[i].LayerDepth;
                            break;
                    }
                }
            }


            //Other
        }


        public void UnloadLevel()
        {
            this._obj_currentmap = null;
        }
    }
}
