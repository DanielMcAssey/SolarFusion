using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public enum LevelObjectType
    {
        None = 0,
        Solid = 1,
        NonSolid = 2,
        Crate = 4,
        Default = 65536,
    }

    public abstract class LevelObject : GameObjects
    {
        protected Rectangle bounds;
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)this.defaultBounds.X, (int)this.defaultBounds.Y, this.defaultBounds.Width, this.defaultBounds.Height); }
        }

        protected LevelObjectType type;
        public LevelObjectType Type
        {
            get { return type; }
        }

        public LevelObject(uint id) : base(id) { }
        public override void Update(GameTime gameTime) { }
        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
