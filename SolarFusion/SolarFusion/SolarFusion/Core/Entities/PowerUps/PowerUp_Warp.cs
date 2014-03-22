using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class PowerUp_Warp : PowerUp
    {
        public PowerUp_Warp(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
            Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Objects/Animated/warp/spritesheet");
            this.animation = new AnimatedSprite(tmpTexture, 8, 3);

            this.animation.AddAnimation("idle", 1, 8, 15);
            this.animation.AddAnimation("end", 2, 7, 3);
            this.animation.AddAnimation("start", 3, 4, 3);
            this.animation.Position = position;
            this.animation.Scale = 1f;
            this.animation.Rotation = 1.5f;
            this.animation.CurrentAnimation = "idle";
            this.type = PowerUpType.Warp;
            this.animation.Loop = true;
        }
    }
}
