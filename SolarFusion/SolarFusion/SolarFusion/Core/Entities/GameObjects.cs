using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public enum ObjectType
    {
        Player = 0,
        Enemy = 1,
        PowerUp = 2,
        LevelObject = 3,
        Other = 4,
    }

    public enum MoveDirection
    {
        Idle = 0,
        Left = 1,
        Right = 2,
        Jump = 3,
    }

    public abstract class GameObjects
    {
        public readonly uint ID;
        public ObjectType ObjectType = ObjectType.Other;
        public float LayerDepth;
        public int Score = 1;
        public abstract Rectangle Bounds { get; }
        public bool Hidden = false;
        public Rectangle defaultBounds;

        public GameObjects(uint id)
        {
            this.ID = id;
        }

        public float ScrollSpeed { get; set; }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
