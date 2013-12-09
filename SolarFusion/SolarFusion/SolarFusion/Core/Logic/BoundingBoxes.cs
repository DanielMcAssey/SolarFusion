using System;
using Microsoft.Xna.Framework;

namespace SolarFusion.Core
{
    public class BoundingBoxes
    {
        public Bound Bottom;
        public Bound Top;
        public Bound Left;
        public Bound Right;
        public uint GameObjectID;

        public BoundingBoxes(uint gameObjectID, Rectangle rectangle)
        {
            this.GameObjectID = gameObjectID;
            this.Top = new Bound(this, rectangle.Top, BoundType.Min);
            this.Left = new Bound(this, rectangle.Left, BoundType.Min);
            this.Bottom = new Bound(this, rectangle.Bottom, BoundType.Max);
            this.Right = new Bound(this, rectangle.Right, BoundType.Max);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle()
            {
                Y = (int)this.Top.Value,
                X = (int)this.Left.Value,
                Width = (int)(this.Right.Value - this.Left.Value),
                Height = (int)(this.Bottom.Value - this.Top.Value),
            };
        }
    }
}
