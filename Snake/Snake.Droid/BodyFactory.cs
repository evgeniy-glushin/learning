using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Snake.Lib;
using static Snake.Droid.Resource.Drawable;

namespace Snake.Droid
{
    public class BodyFactory : IBodyFactory
    {
        private readonly Resources _resources;
        private Dictionary<ResType, int> _table;

        public BodyFactory(Resources resources)
        {
            _resources = resources;
            _table = new Dictionary<ResType, int>()
            {
                [new ResType { Type = GameComponentType.Head, Direction = Direction.Right }] = right_snake_head1_50_50,
                [new ResType { Type = GameComponentType.Head, Direction = Direction.Left }] = left_snake_head1_50_50,
                [new ResType { Type = GameComponentType.Head, Direction = Direction.Up }] = up_snake_head1_50_50,
                [new ResType { Type = GameComponentType.Head, Direction = Direction.Down }] = down_snake_head1_50_50,

                [new ResType { Type = GameComponentType.Body, Direction = Direction.Right }] = snake_body_50_50,
                [new ResType { Type = GameComponentType.Body, Direction = Direction.Left }] = snake_body_50_50,
                [new ResType { Type = GameComponentType.Body, Direction = Direction.Up }] = snake_body_50_50,
                [new ResType { Type = GameComponentType.Body, Direction = Direction.Down }] = snake_body_50_50,

                [new ResType { Type = GameComponentType.Tail, Direction = Direction.Right }] = right_snake_tail_50_50,
                [new ResType { Type = GameComponentType.Tail, Direction = Direction.Left }] = left_snake_tail_50_50,
                [new ResType { Type = GameComponentType.Tail, Direction = Direction.Up }] = up_snake_tail_50_50,
                [new ResType { Type = GameComponentType.Tail, Direction = Direction.Down }] = down_snake_tail_50_50,

                [new ResType { Type = GameComponentType.Apple, Direction = Direction.Right }] = apple1_50_50,
                [new ResType { Type = GameComponentType.Apple, Direction = Direction.Left }] = apple1_50_50,
                [new ResType { Type = GameComponentType.Apple, Direction = Direction.Up }] = apple1_50_50,
                [new ResType { Type = GameComponentType.Apple, Direction = Direction.Down }] = apple1_50_50,
            };
        }

        public ISnakeComponent Create(Position position, GameComponentType type, Direction direction, int width, int height)
        {
            //System.Diagnostics.Debug.WriteLine(type);
            //var img = _resources.ObtainTypedArray(GetResourceId(type)).GetDrawable(GetResourceId(type));

            var imgSnakeComponent = new ImgComponent(position, type,
                GetBitmap(type, Direction.Left, width, height),
                GetBitmap(type, Direction.Right, width, height),
                GetBitmap(type, Direction.Up, width, height),
                GetBitmap(type, Direction.Down, width, height));
            imgSnakeComponent.Direction = direction;

            return imgSnakeComponent;
        }

        private Bitmap GetBitmap(GameComponentType type, Direction direction, int width, int height)
        {
            var img = _resources.GetDrawable(GetResourceId(type, direction));
            Bitmap bitmap = ((BitmapDrawable) img).Bitmap;
            Bitmap scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            return scaledBitmap;
        }

        private int GetResourceId(GameComponentType type, Direction direction)
        {
            return _table[new ResType(type, direction)];
        }

        //private int GetResourceId(GameComponentType type)
        //{
        //    switch (type)
        //    {
        //        case GameComponentType.Body:
        //            return Resource.Drawable.snake_body_50_50;
        //        case GameComponentType.Apple:
        //            return Resource.Drawable.apple1_50_50;
        //        case GameComponentType.Head:
        //            return Resource.Drawable.right_snake_head1_50_50;
        //        case GameComponentType.Tail:
        //            return Resource.Drawable.right_snake_tail_50_50;

        //        default: throw new ApplicationException($"There isn't resource for {type}");
        //    }
        //}

        private struct ResType : IEquatable<ResType>
        {
            public GameComponentType Type { get; set; }
            public Direction Direction { get; set; }

            public ResType(GameComponentType type, Direction direction)
            {
                Type = type;
                Direction = direction;
            }
            
            public bool Equals(ResType other)
            {
                return Type == other.Type && Direction == other.Direction;
            }
        }
    }
}