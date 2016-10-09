using System;
using Android.Graphics;
using Snake.Lib;

namespace Snake.Droid
{
    //public class RectSnakeComponent : ISnakeComponent
    //{
    //    private Rect _rect;
    //    private MainActivity _ctx;
    //    private Canvas _canvas;
    //    private Paint _paint;

    //    public Canvas Canvas { set { _canvas = value; } }
    //    public Rect Rect => _rect;


    //    public RectSnakeComponent(Rect rect, MainActivity ctx)
    //    {
    //        _rect = rect;
    //        _ctx = ctx;
            
    //        _paint = new Paint();
    //        _paint.SetARGB(255, 200, 255, 0);
    //        _paint.SetStyle(Paint.Style.Stroke);
    //        _paint.StrokeWidth = 4;
    //    }
        
    //    public Position CurPosition
    //    {
    //        get { return new Position(_rect.Left, _rect.Top); }
    //        set
    //        {
    //            if (_canvas == null) throw new NullReferenceException("The Canvas must be established!");

    //            _rect.Set((int)value.X, (int)value.Y, (int)value.X + _rect.Width(), (int)value.Y + _rect.Height());
    //            _ctx.RunOnUiThread(() => { _canvas.DrawRect(_rect, _paint); });
    //        }
    //    }

    //    public Position PrevPosition { get; set; }
    //    public int Width { get; }
    //    public int Height { get; }
    //}
}