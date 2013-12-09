using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public enum PowerUpType
    {
        None = 0,
        EnergyBall = 1,
        Crate = 2,
        Dynamite = 4,
        Crystal = 8,
        Default = 65536,
    }

    public abstract class PowerUp : GameObjects
    {
        public AnimatedSprite animation;
        public override Rectangle Bounds
        {
            get { return new Rectangle((int)(animation.Position.X - ((animation.AnimationWidth * animation.Scale) / 2f)), (int)(animation.Position.Y - ((animation.AnimationHeight * animation.Scale) / 2f)), (int)(animation.AnimationWidth * animation.Scale), (int)(animation.AnimationHeight * animation.Scale)); }
        }

        protected PowerUpType type;
        public PowerUpType Type
        {
            get { return type; }
        }
        public PowerUp(uint id) : base(id) { }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
