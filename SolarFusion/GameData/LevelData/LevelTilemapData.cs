using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameData
{
    public struct LevelTilemapData
    {
        public float ScrollOffset;

        [ContentSerializerIgnore]
        public float CurrentScrollOffset;
        public float ScrollSpeed;
        public float LayerDepth;
        public LevelTileData[] TileData;


    }
}
