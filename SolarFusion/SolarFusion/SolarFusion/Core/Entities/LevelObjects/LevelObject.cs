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
        public AnimatedSprite animation;
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(animation.Position.X - ((animation.AnimationWidth * animation.Scale) / 2f)), (int)(animation.Position.Y - ((animation.AnimationHeight * animation.Scale) / 2f)), (int)(animation.AnimationWidth * animation.Scale), (int)(animation.AnimationHeight * animation.Scale)); }
        }

        protected LevelObjectType type;
        public LevelObjectType Type
        {
            get { return type; }
        }
        public LevelObject(uint id) : base(id) { }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, 1f);
        }
    }
}
