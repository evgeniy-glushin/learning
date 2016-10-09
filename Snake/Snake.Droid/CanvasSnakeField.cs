using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Snake.Lib;

namespace Snake.Droid
{
    public class CanvasSnakeField : SurfaceView, ISurfaceHolderCallback, ISnakeField
    {
        private DrawThread _drawThread;
        private List<IGameComponent> _drawableObjs = new List<IGameComponent>();

        #region ISnakeField Impl

        public Position Position { get; private set; }
        public new int Top => 0;
        public new int Left => 0;
        public new int Bottom => Height;
        public new int Right => Width;

        private RelativeLayout.LayoutParams _fieldParams;
        private RelativeLayout.LayoutParams FieldParams
            => _fieldParams ?? (_fieldParams = (RelativeLayout.LayoutParams)LayoutParameters);

        int ISnakeField.Width
        {
            get { return Width; }
            set
            {
                FieldParams.Width = value;
            }
        }

        int ISnakeField.Height
        {
            get { return Height; }

            set
            {
                FieldParams.Height = value;
            }
        }

        public int LeftMargin
        {
            get { return FieldParams.LeftMargin; }
            set { FieldParams.LeftMargin = value; }
        }

        public int RightMargin
        {
            get { return FieldParams.RightMargin; }
            set { FieldParams.RightMargin = value; }
        }

        public int TopMargin
        {
            get { return FieldParams.TopMargin; }
            set { FieldParams.TopMargin = value; }
        }
        public int BottomMargin
        {
            get { return FieldParams.BottomMargin; }
            set { FieldParams.BottomMargin = value; }
        }

        public bool IsOutOfBounds(Position position, int step)
        {
            return position.X < Left ||
                   position.Y < Top ||
                   position.X + step > Right ||
                   position.Y + step > Bottom;
        }

        public void Clear()
        {
            _drawableObjs.Clear();
            Apples.Clear();
        }

        public void Draw(bool isDrawing)
        {
            _drawThread?.SetDrawing(isDrawing);
        }

        private List<IFoodComponent> _apples;
        public List<IFoodComponent> Apples => _apples ?? (_apples = new List<IFoodComponent>());

        private Lib.Snake _snake;
        public Lib.Snake Snake
        {
            get { return _snake; }
            set
            {
                //remove old snake if exists
                //var body = _snake?.SnakeBodyWithHead.Select(b => (IGameComponent) b);
                //if (body != null && _drawableObjs.Exists(l => l == body))
                //    _drawableObjs.Remove(_snake.SnakeBodyWithHead);

                //_snake?.SnakeBodyWithHead.ForEach(x => _drawableObjs.Remove(x));
                _snake = value;
                //_drawableObjs.Add(_snake.SnakeBodyWithHead);
            }
        }


        public void PutOn(IGameComponent item)
        {
            if (item.Type == GameComponentType.Apple)
                Apples.Add((IFoodComponent)item);

            _drawableObjs.Add(item);
        }

        public void PickUp(IGameComponent item)
        {
            if (item.Type == GameComponentType.Apple)
                Apples.Remove((IFoodComponent)item);

            _drawableObjs.Remove(item);
        }


        #endregion

        #region SurfaceView Impl
        public CanvasSnakeField(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Holder.AddCallback(this);
        }

        public CanvasSnakeField(Context context) : base(context)
        {
            Holder.AddCallback(this);
        }

        public CanvasSnakeField(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Holder.AddCallback(this);
        }

        public CanvasSnakeField(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Holder.AddCallback(this);
        }

        public CanvasSnakeField(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Holder.AddCallback(this);
        }
        #endregion

        #region ISurfaceHolderCallback Impl
        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
            Position = new Position(Left, Top);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            var img = Resources.GetDrawable(Resource.Drawable.field_768_442);
            Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
            var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, Width, Height, true);

            _drawThread = new DrawThread(Holder, scaledBitmap, _drawableObjs);
            _drawThread.SetRunning(true);
            _drawThread.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            bool retry = true;
            _drawThread.SetRunning(false);
            while (retry)
            {
                try
                {
                    _drawThread.Join();
                    retry = false;
                }
                catch (InterruptedException e)
                {
                }
            }
        }
        #endregion

        private class DrawThread : Thread
        {
            private bool _running = true;
            private bool _drawing = true;
            private readonly ISurfaceHolder _surfaceHolder;
            private readonly Bitmap _fieldBitmap;
            private List<IGameComponent> _drawableObjs;

            public DrawThread(ISurfaceHolder surfaceHolder, Bitmap fieldBitmap, List<IGameComponent> drawableObjs)
            {
                _surfaceHolder = surfaceHolder;
                _fieldBitmap = fieldBitmap;
                _drawableObjs = drawableObjs;
            }

            public void SetRunning(bool running) => _running = running;
            public void SetDrawing(bool isDrawing) => _drawing = isDrawing;

            private IGameComponent _drawItem;
            public override void Run()
            {
                Canvas canvas = null;
                while (_running)
                {
                    //if (_drawing)
                    //{
                    try
                    {
                        canvas = _surfaceHolder.LockCanvas(null);
                        if (canvas == null) continue;

                        canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                        canvas.DrawBitmap(_fieldBitmap, 0, 0, null);
                        for (int idx = 0; idx < _drawableObjs.Count; idx++)
                        {
                            _drawItem = _drawableObjs[idx];
                            if (_drawItem.CurPosition == null) continue;
                            IDrawableComponent drawable = _drawItem as IDrawableComponent;
                            if (drawable == null) throw new InvalidCastException($"The drawable obj must be inherited from {nameof(IDrawableComponent)}");
                            canvas.DrawBitmap(drawable.Bitmap, _drawItem.CurPosition.X, _drawItem.CurPosition.Y, null);

                            //var list = _drawableObjs[listIdx];
                            //for (int drawObjIdx = 0; drawObjIdx < list.Count; drawObjIdx++)
                            //{
                            //    _drawItem = list[drawObjIdx];
                            //    if (_drawItem.CurPosition == null) continue;
                            //    IDrawableComponent drawable = _drawItem as IDrawableComponent;
                            //    if (drawable == null)
                            //        throw new InvalidCastException(
                            //            $"The drawable obj must be inherited from {nameof(IDrawableComponent)}");
                            //    canvas.DrawBitmap(drawable.Bitmap, _drawItem.CurPosition.X, _drawItem.CurPosition.Y,
                            //        null);
                            //}
                        }
                    }
                    catch (System.Exception e)
                    {
                        throw new System.Exception($"There is exception in Canvas field. {e}");
                    }
                    finally
                    {
                        if (canvas != null)
                        {
                            _surfaceHolder.UnlockCanvasAndPost(canvas);
                        }
                    }
                    //}
                }
            }
        }
    }
}





//for (int i = 0; i < _snake.SnakeBodyWithHead.Count; i++)
//{
//    dot = _snake.SnakeBodyWithHead[i];
//    if(dot.CurPosition == null) continue;
//    var imgSnakeComponent = dot as ImgComponent;
//    canvas.DrawBitmap(imgSnakeComponent.Bitmap, dot.CurPosition.X, _drawItem.CurPosition.Y, null);
//}

//for (int i = 0; i < _apples.Count; i++)
//{
//    dot = _apples[i];

//    var imgSnakeComponent = dot as ImgComponent;
//    canvas.DrawBitmap(imgSnakeComponent.Bitmap, dot.CurPosition.X, _drawItem.CurPosition.Y, null);
//}

#if DEBUG
//var step = _snake.Step;
//for (int i = 0; i <= bottom/step + 3; i++)
//    canvas.DrawLine(left, top + step*i, right, top + step*i, _debugGridPaint);

//for (int i = 0; i <= right/step; i++)
//    canvas.DrawLine(left + step*i, top, left + step*i, bottom, _debugGridPaint);
#endif