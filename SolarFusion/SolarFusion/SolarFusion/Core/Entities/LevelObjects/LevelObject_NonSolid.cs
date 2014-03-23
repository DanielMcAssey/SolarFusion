using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class LevelObject_NonSolid : LevelObject
    {
        public LevelObject_NonSolid(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
        }
    }
}
