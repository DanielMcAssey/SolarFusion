using System;
using System.Collections.Generic;
using System.Linq;

using GameData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DataPipeline
{
    [ContentSerializerRuntimeType("GameData.GameEntityGroup, GameData")]
    public struct PipelineGameEntity
    {
        public float ScrollOffset;
        public float ScrollSpeed;
        public float LayerDepth;
        public GameEntity[] GameEntityData;

        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties;
    }
}
