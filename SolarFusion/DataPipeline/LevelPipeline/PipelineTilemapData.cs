using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DataPipeline
{
    [ContentSerializerRuntimeType("GameData.LevelTilemapData, GameData")]
    public struct PipelineTilemapData
    {
        public float ScrollOffset;
        public float ScrollSpeed;
        public float LayerDepth;
        public LevelTileData[] TileData;

        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties;
    }
}
