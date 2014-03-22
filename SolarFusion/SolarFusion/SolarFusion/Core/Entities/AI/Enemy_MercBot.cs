using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SolarFusion.Core
{
    public class Enemy_MercBot : AI
    {
        public Enemy_MercBot(uint id, ContentManager virtualContent, Vector2 position)
            : base(id)
        {
            Texture2D tmpTexture = virtualContent.Load<Texture2D>("Sprites/Enemies/mercbot/spritesheet");
            this.animation = new AnimatedSprite(tmpTexture, 3, 3);

            this.animation.AddAnimation("idle", 1, 3, 3);
            this.animation.AddAnimation("left", 2, 3, 3);
            this.animation.AddAnimation("right", 3, 3, 3);

            this.animation.Position = position;
            this.animation.Scale = 1.5f;
            this.animation.CurrentAnimation = "idle";
            this.animation.Loop = true;

            this.Health = 100;
            this.Speed = 1f;
        }
    }
}
