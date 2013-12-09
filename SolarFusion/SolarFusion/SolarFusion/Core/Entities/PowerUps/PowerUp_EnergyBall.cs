using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class PowerUp_EnergyBall : PowerUp
    {
        public PowerUp_EnergyBall(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
            Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Objects/Animated/powerup_energyball");
            this.animation = new AnimatedSprite(tmpTexture, 12, 1);

            this.animation.AddAnimation("idle", 1, 12, 15);
            this.animation.Position = position;
            this.animation.Origin = new Vector2((tmpTexture.Width / 12f) / 2f, tmpTexture.Height / 2f);
            this.animation.Scale = 0.5f;
            this.animation.CurrentAnimation = "idle";
            this.Score = 10;
            this.type = PowerUpType.EnergyBall;
        }
    }
}
