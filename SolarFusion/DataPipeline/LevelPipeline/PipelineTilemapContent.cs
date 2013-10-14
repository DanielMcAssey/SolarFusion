using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DataPipeline.LevelPipeline
{
    [ContentSerializerRuntimeType("GameData.LevelTilemap, GameData")]
    public class PipelineTilemapContent
    {
        public string tmName;
        public int tmWidth;
        public int tmHeight;
        public int tmTileWidth;
        public int tmTileHeight;
        public string[] tmImagePaths;
        public int tmTileCount;
        public LevelTile[] tmTiles;
        public int tmLayerCount;
        public PipelineTilemapData[] tmLayers;
        public int tmGameEntityGroupCount;
        public PipelineGameEntity[] tmGameEntityGroups;
        public Vector2 tmPlayerStart;
        public int tmPlayerLayer;
        public string tmMusic;

        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }
}
