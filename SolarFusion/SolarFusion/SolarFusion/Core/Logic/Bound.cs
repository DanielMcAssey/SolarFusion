using System;

namespace SolarFusion.Core
{
    public enum BoundType
    {
        Min,
        Max,
    }

    public class Bound : IComparable<Bound>
    {
        public BoundingBoxes Box;
        public float Value;
        public BoundType Type;

        public Bound(BoundingBoxes box, float value, BoundType type)
        {
            this.Box = box;
            this.Value = value;
            this.Type = type;
        }

        public int CompareTo(Bound otherBound)
        {
            int relationship = this.Value.CompareTo(otherBound.Value);
            if (relationship == 0)
                relationship += this.Type.CompareTo(otherBound.Type);
            return relationship;
        }
    }
}
