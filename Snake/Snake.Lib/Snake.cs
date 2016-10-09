using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;
using static System.Diagnostics.Debug;

namespace Snake.Lib
{
    public class Snake //: ISnakeComponent
    {
        private const double TOLERANCE = 0.00001;
        private const int SNAKE_MIN_ITEMS = 3;

        private readonly IBodyFactory _bodyFactory;
        private IFoodFactory _foodFactory;
        private List<ISnakeComponent> _snakeBodyWithHead;
        
        public Snake(ISnakeField field, IBodyFactory bodyFactory, IFoodFactory foodFactory, int step)
        {
          
            Step = step;
            Field = field;
            _bodyFactory = bodyFactory;
            _foodFactory = foodFactory;
        }

        public void InitSnake(Position startPosition, int snakeLength)
        {
            if (snakeLength < SNAKE_MIN_ITEMS) throw new ArgumentException($"The min length for the snake is {SNAKE_MIN_ITEMS}");
            
            if (SnakeBodyWithHead.Any()) SnakeBodyWithHead.Clear();
            if (IsCrashed) IsCrashed = false;
         
            Direction = Direction.Right;
            Add(_bodyFactory.Create(startPosition, GameComponentType.Head, Direction, Step, Step));
            for (int i = 1; i < snakeLength - 1; i++) //snakeLength - 1 - this is the snake's body length without tail
                Add(_bodyFactory.Create(new Position(startPosition.X - i * Step, startPosition.Y), GameComponentType.Body, Direction, Step, Step));
            Add(_bodyFactory.Create(new Position(startPosition.X - (snakeLength - 1) * Step, startPosition.Y), GameComponentType.Tail, Direction, Step, Step));

            _body = null;
            //foreach (var food in SnakeBodyWithHead) newBodyPart.Direction = Direction;
        }

        public event Action OnCrashed;
        public event Action<IFoodComponent> OnEat;

        /// <summary>
        /// Gets whether the snake was crashed or not.
        /// </summary>
        public bool IsCrashed { get; private set; }

        public int Step { get; set; }

        /// <summary>
        /// The snake's head.
        /// </summary>
        public ISnakeComponent Head => SnakeBodyWithHead.Count == 0 ? null : SnakeBodyWithHead.First();

        /// <summary>
        /// The snake's tail.
        /// </summary>
        public ISnakeComponent Tail => SnakeBodyWithHead.Count == 0 ? null : SnakeBodyWithHead.Last();

        //It's required to recreate body after each change of snake's size.
        //For this purpose it just need to set the _body field to NULL.
        private List<ISnakeComponent> _body;

        /// <summary>
        /// The snake's body without head and tail. 
        /// </summary>
        public List<ISnakeComponent> Body => _body ?? (_body = SnakeBodyWithHead.Skip(1)
            .Take(SnakeBodyWithHead.Count - 2)
            .ToList());

        private ISnakeComponent BodyFirst => SnakeBodyWithHead[1];
        private ISnakeComponent BodyLast => SnakeBodyWithHead[SnakeBodyWithHead.Count - 2];
        
        /// <summary>
        /// Gets snake body with head.
        /// </summary>
        public List<ISnakeComponent> SnakeBodyWithHead => 
            _snakeBodyWithHead ?? (_snakeBodyWithHead = new List<ISnakeComponent>());

        /// <summary>
        /// Gets the snakes field.
        /// </summary>
        public ISnakeField Field { private get; set; }

        /// <summary>
        /// Grows up the snake.
        /// </summary>
        /// <param name="food">The food.</param>
        public void Eat(IFoodComponent food)
        {
            //put the new body part on the tail's place
            var newBodyPart = _bodyFactory.Create(null, GameComponentType.Body, Direction, Step, Step);
            newBodyPart.CurPosition = Tail.CurPosition;
            newBodyPart.PrevPosition = Tail.PrevPosition;

            //step back tail 
            Tail.CurPosition = Tail.PrevPosition;
            Tail.Direction = GetDirestion(newBodyPart.CurPosition, Tail.CurPosition);

            //set direction for new food
            newBodyPart.Direction = Direction;

            //grow the snake
            SnakeBodyWithHead.Insert(SnakeBodyWithHead.Count - 1, newBodyPart);
            _body = null; // it will cause to recreate 

            Field.PutOn(newBodyPart);
            OnEat?.Invoke(food);
        }

        public void Add(ISnakeComponent item)
        {
            item.Direction = Direction;
            SnakeBodyWithHead.Add(item);
            Field.PutOn(item);
        }

        public int Speed { get; internal set; } = 200;
        private bool _isRuning;

        public async void Run(bool isRuning)
        {
            _isRuning = isRuning;
            while (_isRuning && !IsCrashed)
            {
                try
                {
                    Direction = DirectionQueue.Dequeue(Direction);
                    Move(Direction);

                    await TaskEx.Delay(Speed);
                }
                catch (Exception e)
                {
                    throw new Exception($"Something wrong in Run method of Snake class. {e.StackTrace}");
                }
            }
        }

        public Direction Direction { get; private set; }
        
  
        public void GoRight()
        {
            ThrowIfCanNotMove();
            var direction = Direction.Right;
            if (IsLeftMoving(Head.CurPosition, BodyFirst.CurPosition))
                direction = TurnBack();
            DirectionQueue.Enqueue(direction);
        }

        public void GoLeft()
        {
            ThrowIfCanNotMove();
            var direction = Direction.Left;
            if (IsRightMoving(Head.CurPosition, BodyFirst.CurPosition))
                direction = TurnBack();
            DirectionQueue.Enqueue(direction);
        }

        public void GoDown()
        {
            ThrowIfCanNotMove();
            var direction = Direction.Down;
            if (IsUpMoving(Head.CurPosition, BodyFirst.CurPosition))
                direction = TurnBack();
            DirectionQueue.Enqueue(direction);
        }

        public void GoUp()
        {
            ThrowIfCanNotMove();
            var direction = Direction.Up;
            if (IsDownMoving(Head.CurPosition, BodyFirst.CurPosition))
                direction = TurnBack();
            DirectionQueue.Enqueue(direction);
        }

        private void ThrowIfCanNotMove()
        {
            if (SnakeBodyWithHead.Count < SNAKE_MIN_ITEMS)
                throw new Exception("The snake must have haed and at least one food in body!");
        }

        /// <summary>
        /// Moves the snake.
        /// </summary>
        /// <param name="state">The moving direction.</param>
        private void Move(Direction state)
        {
            if (state == Direction.Right)
                Move(xOffset: Step, yOffset: 0);
            else if (state == Direction.Left)
                Move(xOffset: -Step, yOffset: 0);
            else if (state == Direction.Down)
                Move(xOffset: 0, yOffset: Step);
            else if (state == Direction.Up)
                Move(xOffset: 0, yOffset: -Step);
        }

        /// <summary>
        /// Moves the snake.
        /// </summary>
        /// <param name="xOffset">The offset of X axis.</param>
        /// <param name="yOffset">The offset of Y axis.</param>
        private void Move(int xOffset, int yOffset)
        {
            //the snake can't move if crashed
            if (IsCrashed) return;

            //determines the next position of the head
            Position nextHeadPosition = new Position(Head.CurPosition.X + xOffset, Head.CurPosition.Y + yOffset);
        
            //the snake out of bounds
            if (Field.IsOutOfBounds(nextHeadPosition, Step))
            {
                //move the head to oposite side of field
                if (IsLeftMoving(nextHeadPosition, Head.CurPosition))
                    nextHeadPosition = new Position(Field.Right + xOffset, Head.CurPosition.Y + yOffset);
                else if (IsRightMoving(nextHeadPosition, Head.CurPosition))
                    nextHeadPosition = new Position(Field.Left, Head.CurPosition.Y + yOffset);
                else if (IsUpMoving(nextHeadPosition, Head.CurPosition))
                    nextHeadPosition = new Position(Head.CurPosition.X + xOffset, Field.Bottom + yOffset);
                else if (IsDownMoving(nextHeadPosition, Head.CurPosition))
                    nextHeadPosition = new Position(Head.CurPosition.X + xOffset, Field.Top);
            }

            //stop go ahead if the snake crashed
            if (CrashTest(nextHeadPosition)) return;

            //the head do step
            Head.PrevPosition = Head.CurPosition;
            Head.CurPosition = nextHeadPosition;
            Head.Direction = Direction;

            //the body do step
            for (int i = 0; i < Body.Count; i++)
            {
                var next = i == 0 ? Head : Body[i - 1];
                var current = Body[i];

                current.PrevPosition = current.CurPosition;
                current.CurPosition = next.PrevPosition;
                current.Direction = Direction;
            }

            //the tail do step
            Tail.PrevPosition = Tail.CurPosition;
            Tail.CurPosition = BodyLast.PrevPosition;
            Tail.Direction = GetDirestion(BodyLast.CurPosition, Tail.CurPosition);
            
            DetectAndEatFood();
        }

        /// <summary>
        /// Calcs whether the snake crashed or not.
        /// </summary>
        /// <param name="position">The test position.</param>
        /// <returns></returns>
        private bool CrashTest(Position position)
        {
            IsCrashed = Body.Any(x => x.CurPosition.Equals(position));
            if (IsCrashed) OnCrashed?.Invoke();
            return IsCrashed;
        }

        private void DetectAndEatFood()
        {
            var food = Field.Apples.FirstOrDefault(a => a.CurPosition.Equals(Head.CurPosition));
            if (food != null)
            {
                Field.PickUp(food);
                Eat(food);
            }
        }

        /// <summary>
        /// Get's direction by positions.
        /// </summary>
        /// <param name="forward">The forward position.</param>
        /// <param name="rear">The rear position.</param>
        /// <returns></returns>
        private Direction GetDirestion(Position forward, Position rear)
        {
            if (IsRightMoving(forward, rear))
                return Direction.Right;
            else if (IsLeftMoving(forward, rear))
                return Direction.Left;
            else if (IsUpMoving(forward, rear))
                return Direction.Up;
            else if (IsDownMoving(forward, rear))
                return Direction.Down;
            else throw new ArgumentException("Can't find out direction.");
        }

        /// <summary>
        /// Turns the snake to oposite side.
        /// </summary>
        private Direction TurnBack()
        {
            //the snake can't move if crashed
            if (IsCrashed) return Direction.None;

            int curSnakePosition = 0;
            var reversePosition = SnakeBodyWithHead.Count - 1;
            while (curSnakePosition < reversePosition)
            {
                var curPosition = SnakeBodyWithHead[curSnakePosition].CurPosition;

                SnakeBodyWithHead[curSnakePosition].CurPosition = SnakeBodyWithHead[reversePosition].CurPosition;
                SnakeBodyWithHead[curSnakePosition].PrevPosition = null;

                SnakeBodyWithHead[reversePosition].CurPosition = curPosition;
                SnakeBodyWithHead[reversePosition].PrevPosition = null;

                ++curSnakePosition;
                reversePosition = SnakeBodyWithHead.Count - curSnakePosition - 1;
            }

            Direction direction = GetDirestion(Head.CurPosition, BodyFirst.CurPosition);
            Head.Direction = direction;
            Tail.Direction = direction;
            WriteLine($"TurnBack: direction - {direction}");

            DirectionQueue.Dequeue();

            return direction;
        }

        /// <summary>
        /// Returns True if the snake moves from right to left.
        /// </summary>
        /// <returns></returns>
        private bool IsLeftMoving(Position head, Position bodyFirst)
        {
            return (Abs(head.X - bodyFirst.X + Step) < TOLERANCE || IsForwardOnRightAndRearOnLeftSide(head, bodyFirst)) &&
                   Abs(head.Y - bodyFirst.Y) < TOLERANCE;
        }

        /// <summary>
        /// Returns True if the snake moves from left to right.
        /// </summary>
        /// <returns></returns>
        private bool IsRightMoving(Position head, Position bodyFirst)
        {
            return (Abs(head.X - bodyFirst.X - Step) < TOLERANCE || IsForwardOnLeftAndRearOnRightSide(head, bodyFirst)) &&
                   Abs(head.Y - bodyFirst.Y) < TOLERANCE;
        }

        /// <summary>
        /// Returns True if the snake moves from top to bottom.
        /// </summary>
        /// <returns></returns>
        private bool IsDownMoving(Position head, Position bodyFirst)
        {
            return Abs(head.X - bodyFirst.X) < TOLERANCE &&
                   (Abs(head.Y - bodyFirst.Y - Step) < TOLERANCE || IsForwardOnTopAndRearOnBottomSide(head, bodyFirst));
            //Head.CurPosition.Y > Body.First().CurPosition.Y;
        }

        /// <summary>
        /// Returns True if the snake moves from bottom to top.
        /// </summary>
        /// <returns></returns>
        private bool IsUpMoving(Position head, Position bodyFirst)
        {
            return Abs(head.X - bodyFirst.X) < TOLERANCE &&
                   (Abs(head.Y - bodyFirst.Y + Step) < TOLERANCE || IsForwardOnBottomAndRearOnTopSide(head, bodyFirst));
            // Head.CurPosition.Y < Body.First().CurPosition.Y;
        }

        /// <summary>
        /// Gets true if the forward and the rear splited and forward on the right side of the field and rear on the left side.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="rear"></param>
        private bool IsForwardOnRightAndRearOnLeftSide(Position forward, Position rear) =>
            Abs(forward.X - Field.Right + Step) < TOLERANCE &&
            Abs(rear.X - Field.Left) < TOLERANCE;

        /// <summary>
        /// Gets true if the forward and the rear splited and forward on the left side of the field and rear on the right side.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="rear"></param>
        private bool IsForwardOnLeftAndRearOnRightSide(Position forward, Position rear) =>
            Abs(forward.X - Field.Left) < TOLERANCE &&
            Abs(rear.X - Field.Right + Step) < TOLERANCE;

        /// <summary>
        /// Gets true if the forward and the rear splited and forward on the top side of the field and rear on the bottom side.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="rear"></param>
        private bool IsForwardOnTopAndRearOnBottomSide(Position forward, Position rear) =>
            Abs(forward.Y - Field.Top) < TOLERANCE &&
            Abs(rear.Y - Field.Bottom + Step) < TOLERANCE;

        /// <summary>
        /// Gets true if the forward and the rear splited and forward on the bottom side of the field and rear on the top side.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="rear"></param>
        private bool IsForwardOnBottomAndRearOnTopSide(Position forward, Position rear) =>
            Abs(forward.Y - Field.Bottom + Step) < TOLERANCE &&
            Abs(rear.Y - Field.Top) < TOLERANCE;


        private static class DirectionQueue
        {
            private static readonly Queue<Direction> DirectionsQueue = new Queue<Direction>(2);

            public static void Enqueue(Direction d)
            {
                if (DirectionsQueue.Count == 2) return;
                DirectionsQueue.Enqueue(d);
            }
            public static Direction Dequeue(Direction defaultDirect = Direction.None)
            {
                if (DirectionsQueue.Count == 0) return defaultDirect;
                return DirectionsQueue.Dequeue();
            }
        }

    }



    //private Direction GetDirestion(Direction desirableDirection)
    //{
    //    if (desirableDirection == Direction.Right)
    //        if (IsLeftMoving(Head.CurPosition, BodyFirst.CurPosition) && IsRightMoving(BodyLast.CurPosition, Tail.CurPosition))
    //            return Direction.Left;
    //        else
    //            return Direction.Right;

    //    else if (desirableDirection == Direction.Left)
    //        if (IsRightMoving(Head.CurPosition, BodyFirst.CurPosition) &&
    //            IsLeftMoving(BodyLast.CurPosition, Tail.CurPosition))
    //            return Direction.Right;
    //        else
    //            return Direction.Left;

    //    else if (desirableDirection == Direction.Up)
    //        if (IsDownMoving(Head.CurPosition, BodyFirst.CurPosition) &&
    //            IsUpMoving(BodyLast.CurPosition, Tail.CurPosition))
    //            return Direction.Down;
    //        else
    //            return Direction.Up;

    //    else if (desirableDirection == Direction.Down)
    //        if (IsUpMoving(Head.CurPosition, BodyFirst.CurPosition) &&
    //            IsDownMoving(BodyLast.CurPosition, Tail.CurPosition))
    //            return Direction.Up;
    //        else
    //            return Direction.Down;

    //    else throw new NotSupportedException($"There is no logic for {desirableDirection}");
    //}

}
