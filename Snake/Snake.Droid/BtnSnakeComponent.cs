using Android.Widget;
using Snake.Lib;

namespace Snake.Droid
{
    //public class BtnSnakeComponent : ISnakeComponent
    //{
    //    private MainActivity _ctx;

    //    private Button _btn;
    //    public Button CoreBtn
    //    {
    //        get { return _btn; }
    //    }

    //    public BtnSnakeComponent(Button btn, MainActivity ctx) //, int size, Position position
    //    {
    //        _btn = btn;
    //        _ctx = ctx;
    //        //_btn.LayoutParameters = new LayoutParams(size, size, (int)position.X, (int)position.Y);
    //    }

    //    //private Position _curPosition;
    //    public Position CurPosition
    //    {
    //        get
    //        {
    //            //TODO: Do not instaciate new object each time.
    //            //return new Position(_btn.GetX(), _btn.GetY());
    //            return null;
    //        }
    //        set
    //        {
    //            _ctx.RunOnUiThread(() => {
    //                _btn.SetX(value.X);
    //                _btn.SetY(value.Y);
    //            });
    //            //_btn.SetX(value.X);
    //            //_btn.SetY(value.Y);
    //        }
    //    }

    //    public Position PrevPosition { get; set; }
    //    public int Width { get; }
    //    public int Height { get; }
    //}
}