using Android.Graphics;
using Snake.Lib;

namespace Snake.Droid
{
    class FoodComponent : IFoodComponent, IDrawableComponent
    {
        public FoodComponent(Position curPosition, Bitmap bitmap, int points, GameComponentType type)
        {
            CurPosition = curPosition;
            Points = points;
            Type = type;
            Bitmap = bitmap;
        }

        public Position CurPosition { get; set; }
        public GameComponentType Type { get; }
        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public int Points { get; }
        public Bitmap Bitmap { get; }
    }
}