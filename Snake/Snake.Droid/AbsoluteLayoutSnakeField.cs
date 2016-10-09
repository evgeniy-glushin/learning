using System;
using Android.Content;
using Android.Widget;
using Snake.Lib;

using Android.App;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.OS;
using Android.Util;

namespace Snake.Droid
{
    //public class AbsoluteLayoutSnakeField : ISnakeField
    //{
    //    private AbsoluteLayout _layout;
    //    private MainActivity _ctx;

    //    public AbsoluteLayoutSnakeField(AbsoluteLayout layout, MainActivity ctx)
    //    {
    //        _layout = layout;
    //        _ctx = ctx;
    //    }

    //    public Position Position { get; set; }
    //    public int Width { get; }
    //    public int Height { get; }
    //    public int Left { get; }
    //    public int Top { get; }
    //    public int Right { get; }
    //    public int Bottom { get; }
    //    public int W { get; set; }
    //    public int H { get; set; }
    //    public bool IsOutOfBounds(int l, int t, int r, int b)
    //    {


    //        throw new NotImplementedException();
    //    }

    //    public void Add(ISnakeComponent item, Position position, int size)
    //    {
    //        BtnSnakeComponent btnSnake = item as BtnSnakeComponent;
    //        if (btnSnake == null) throw new InvalidCastException($"Can't cast {item.GetType().Name} to {nameof(BtnSnakeComponent)}");

    //        _ctx.RunOnUiThread(() => _layout.AddView(btnSnake.CoreBtn, new AbsoluteLayout.LayoutParams(size, size, (int)position.X, (int)position.Y)));
    //        //_layout.AddView(btnSnake.CoreBtn, new AbsoluteLayout.LayoutParams(size, size, (int)position.X, (int)position.Y));
    //    }
    //}
}