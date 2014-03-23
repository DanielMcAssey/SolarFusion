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
            Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Objects/Static/energy_ball/sprite");
            this.animation = new AnimatedSprite(tmpTexture, 1, 1);
            this.animation.AddAnimation("idle", 1, 1, 1);
            this.animation.Position = new Vector2(position.X, position.Y - 15);
            this.animation.Origin = new Vector2(tmpTexture.Width / 2f, tmpTexture.Height / 2f);
            this.animation.Scale = 1f;
            this.animation.CurrentAnimation = "idle";
            this.Score = 1;
            this.type = PowerUpType.EnergyBall;
        }
    }
}
