using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class LevelObject_Crate : LevelObject
    {
        public LevelObject_Crate(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
            //Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Objects/crate/sprite");
            //this.type = LevelObjectType.Crate;
        }
    }
}
