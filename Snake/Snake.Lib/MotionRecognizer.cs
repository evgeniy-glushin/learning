using static System.Math;

namespace Snake.Lib
{
    public class MotionRecognizer
    {
        public static Direction Recognize(Position down, Position up)
        {
            var xOffset = down.X - up.X;
            var yOffset = down.Y - up.Y;

            if (Abs(xOffset) > Abs(yOffset))
            {
                return xOffset < 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return yOffset > 0 ? Direction.Up : Direction.Down;
            }
        }
    }
}
