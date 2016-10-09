using Android.Graphics;
using Snake.Lib;

namespace Snake.Droid
{
    public class ImgComponent : ISnakeComponent, IDrawableComponent
    {
        public Bitmap Bitmap { get; private set; }
        internal Bitmap LeftBitmap { get; }
        internal Bitmap RightBitmap { get; }
        internal Bitmap UpBitmap { get; }
        internal Bitmap DownBitmap { get; }



        public Position CurPosition { get; set; }
        public Position PrevPosition { get; set; }
        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public GameComponentType Type { get; }

        private Direction _direction;
        public Direction Direction
        {
            get { return _direction; }
            set
            {
                if (_direction == value) return;
                _direction = value;
                switch (_direction)
                {
                    case Direction.Left:
                        Bitmap = LeftBitmap;
                        break;
                    case Direction.Right:
                        Bitmap = RightBitmap;
                        break;
                    case Direction.Up:
                        Bitmap = UpBitmap;
                        break;
                    case Direction.Down:
                        Bitmap = DownBitmap;
                        break;
                }
            }
        }

        public ImgComponent(Position curPosition, GameComponentType type, Bitmap leftBitmap, Bitmap rightBitmap, Bitmap upBitmap, Bitmap downBitmap)
        {
            CurPosition = curPosition;
            Type = type;
            LeftBitmap = leftBitmap;
            RightBitmap = rightBitmap;
            UpBitmap = upBitmap;
            DownBitmap = downBitmap;
        }




        //public ImgComponent(Bitmap leftBitmap, Bitmap rightBitmap, Bitmap upBitmap, Bitmap downBitmap)
        //{
        //    LeftBitmap = leftBitmap;
        //    RightBitmap = rightBitmap;
        //    UpBitmap = upBitmap;
        //    DownBitmap = downBitmap;
        //}
    }
}