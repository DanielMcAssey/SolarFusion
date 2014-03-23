using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class LevelObject_Solid : LevelObject
    {
        public LevelObject_Solid(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
        }
    }
}
