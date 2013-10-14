using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameData
{
    public class LevelTilemap
    {
        public string tmName;
        public int tmWidth;
        public int tmHeight;
        public int tmTileWidth;
        public int tmTileHeight;
        public string[] tmImagePaths;

        [ContentSerializerIgnore]
        public Texture2D[] tmTextures;
        public int tmTileCount;
        public LevelTile[] tmTiles;
        public int tmLayerCount;
        public LevelTilemapData[] tmLayers;
        public int tmGameEntityGroupCount;
        public GameEntityGroup[] tmGameEntityGroups;
        public Vector2 tmPlayerStart;
        public int tmPlayerLayer;
        public string tmMusic;

        public void LoadContent(ContentManager contentManager)
        {
            tmTextures = new Texture2D[tmImagePaths.Length];

            for (int i = 0; i < tmImagePaths.Length; i++)
            {
                tmTextures[i] = contentManager.Load<Texture2D>(tmImagePaths[i]);
            }
        }
    }
}
