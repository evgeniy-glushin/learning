using System;

namespace Snake.Lib
{
    public class Position : IEquatable<Position>
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString()
        {
            return $"X: {X}; Y: {Y};";
        }

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
