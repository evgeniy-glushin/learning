using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Snake.Lib;
using static Snake.Droid.Resource.Drawable;

namespace Snake.Droid
{
    class FoodFactory : IFoodFactory
    {
        private readonly Resources _resources;

        public FoodFactory(Resources resources)
        {
            _resources = resources;
        }

        public IFoodComponent Create(Position position, int width, int height)
        {
            return new FoodComponent(position, GetBitmap(width, height), 10, GameComponentType.Apple);
        }

        private Bitmap GetBitmap(int width, int height)
        {
            var img = _resources.GetDrawable(apple1_50_50);
            Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
            Bitmap scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            return scaledBitmap;
        }
    }
}