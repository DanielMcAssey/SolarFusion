using System;

namespace SolarFusion.Core
{
    public struct CollisionPair : IComparable<CollisionPair>
    {
        public readonly uint A;
        public readonly uint B;

        public CollisionPair(uint A, uint B)
        {
            if (A < B)
            {
                this.A = A;
                this.B = B;
            }
            else
            {
                this.A = B;
                this.B = A;
            }
        }

        public int CompareTo(CollisionPair pair)
        {
            return this.A.CompareTo(pair.A) + this.B.CompareTo(pair.B);
        }
    }
}
