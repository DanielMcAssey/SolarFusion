using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class PowerUp_Crystal : PowerUp
    {
        public PowerUp_Crystal(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
            Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Objects/Animated/crystal/spritesheet");
            this.animation = new AnimatedSprite(tmpTexture, 5, 1);
            this.animation.AddAnimation("idle", 1, 5, 6);
            this.animation.Position = new Vector2(position.X, position.Y - 15);
            this.animation.Scale = 1f;
            this.animation.CurrentAnimation = "idle";
            this.type = PowerUpType.Crystal;
            this.animation.Loop = true;
        }
    }
}
