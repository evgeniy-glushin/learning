using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;

namespace Snake.Lib
{
    public class SnakeGame
    {
        private readonly ISnakeField _field;
        private readonly IBodyFactory _bodyFactory;
        private IFoodFactory _foodFactory;
        private readonly int _countOfItemsInRow;
        private int _countOfItemsInColumn;
        private int _snakeSpeed;
        int STEP;


        public SnakeGame(ISnakeField field, IBodyFactory bodyFactory, IFoodFactory foodFactory, int countOfItemsInRow, int snakeSpeed)
        {
            _field = field;
            _bodyFactory = bodyFactory;
            _countOfItemsInRow = countOfItemsInRow;
            _snakeSpeed = snakeSpeed;
            _foodFactory = foodFactory;
            _snakeSpeed = snakeSpeed;

            InitGame(field, bodyFactory, foodFactory);
        }

        private void InitGame(ISnakeField snakeField, IBodyFactory componentFactory, IFoodFactory foodFactory)
        {
            STEP = AdjustGameField();
            InitSnake(snakeField, componentFactory, foodFactory);
        }

        private void InitSnake(ISnakeField snakeField, IBodyFactory bodyFactory, IFoodFactory foodFactory)
        {
            Snake = new Snake(snakeField, bodyFactory, foodFactory, STEP);
            snakeField.Clear();
            _field.Snake = Snake;
            Snake.OnCrashed += GameOver;
            Snake.OnEat += SnakeOnOnEat;

            Snake.InitSnake(new Position(5 * STEP, 5 * STEP), 3);
        }

        private void SnakeOnOnEat(IFoodComponent food)
        {
            Score += food.Points;
        }


        private Position _touchDownPosition;
        public void TouchDown(Position position)
        {
            if (Status == GameStatus.None || Status == GameStatus.Pause || Status == GameStatus.Restart) Play();
            //else if (Snake.Speed < _snakeSpeed) Snake.Speed = _snakeSpeed;
            else  if (_touchDownPosition == null) _touchDownPosition = position;
        }

        public void TouchUp(Position position)
        {
            if (_touchDownPosition != null)
            {
                var direction = RecognizeDirection(_touchDownPosition, position);
                if (direction == Snake.Direction)
                {
                    Snake.Speed = _snakeSpeed / 2;
                }
                else
                {
                    Snake.Speed = _snakeSpeed;
                    switch (direction)
                    {
                        case Direction.Left:
                            Left();
                            break;
                        case Direction.Right:
                            Right();
                            break;
                        case Direction.Up:
                            Up();
                            break;
                        case Direction.Down:
                            Down();
                            break;
                    }
                }

                _touchDownPosition = null;
            }
        }

        public static Direction RecognizeDirection(Position down, Position up)
        {
            var xOffset = down.X - up.X;
            var yOffset = down.Y - up.Y;

            if (Abs(xOffset) > Abs(yOffset))
                return xOffset < 0 ? Direction.Right : Direction.Left;
            else
                return yOffset > 0 ? Direction.Up : Direction.Down;
        }

        internal Snake Snake { get; set; }

        public event Action OnGameOver;
        public event Action OnScoreChanged;

        public void Left() => Snake.GoLeft();
        public void Right() => Snake.GoRight();
        public void Up() => Snake.GoUp();
        public void Down() => Snake.GoDown();

        private int _score;

        public int Score
        {
            get { return _score; }
            private set
            {
                _score = value;
                OnScoreChanged?.Invoke();
            }
        }

        public GameStatus Status { get; set; } = GameStatus.None;

        public void Play()
        {
            if (Status == GameStatus.None || Status == GameStatus.Pause || Status == GameStatus.Restart)
            {
                Status = GameStatus.Running;
                //_field.Draw(true);
                Snake.Run(true);
                StartFoodGenerator();
            }
        }

        public void Pause()
        {
            if (Status == GameStatus.Running)
            {
                Status = GameStatus.Pause;
                //_field.Draw(false);
                Snake.Run(false);
            }
        }

        public void Stop()
        {
            Status = GameStatus.Stoped;
            Snake.Run(false);
        }

        public void Restart()
        {
            Stop();
            Status = GameStatus.Restart;
            InitSnake(_field, _bodyFactory, _foodFactory);
            _field.Apples.Clear();
            Score = 0;
            //Play();
        }

        private void GameOver()
        {
            Status = GameStatus.GameOver;
            Snake.Run(false);
            OnGameOver?.Invoke();
        }

        /// <summary>
        /// Gets size of one item in grid.
        /// </summary>
        private int AdjustGameField()
        {
            int itemWidthHeight = _field.Width / _countOfItemsInRow;

            //adjust field's width
            var amountOfItemsInRowAdjusted = (int)Math.Floor(_field.Width / itemWidthHeight);
            var widthAdjusted = amountOfItemsInRowAdjusted * itemWidthHeight;
            var widthDif = _field.Width - widthAdjusted;
            _field.Width = widthAdjusted;

            //adjust field's height
            var amountOfItemsInColumnAdjusted = (int)Math.Floor(_field.Height / itemWidthHeight);
            var heightAdjusted = amountOfItemsInColumnAdjusted * itemWidthHeight;
            var heightDif = _field.Height - heightAdjusted;
            _field.Height = heightAdjusted;
            _countOfItemsInColumn = amountOfItemsInColumnAdjusted;

            //_field.LeftMargin = widthDif / 2;
            //_field.RightMargin = heightDif - _field.LeftMargin;

            //_field.TopMargin = heightDif / 2;
            //_field.BottomMargin = heightDif - _field.TopMargin;

            return itemWidthHeight;
        }



        #region FoodGenerator

        readonly Random _random = new Random();

        private Guid _foodGeneratorId;
        async void StartFoodGenerator()
        {
            _foodGeneratorId = Guid.NewGuid();
            var token = _foodGeneratorId;
            while (Status == GameStatus.Running && token == _foodGeneratorId)
            {
                PutOnApple();
                await TaskEx.Delay(3000);
            }
        }

        private void PutOnApple()
        {
            var rowNum = _random.Next(_countOfItemsInRow);
            var colNum = _random.Next(_countOfItemsInColumn);
            Position applePosition = new Position(rowNum * STEP, colNum * STEP);

            if (_field.Apples.All(a => !a.CurPosition.Equals(applePosition)) &&
                Snake.SnakeBodyWithHead.All(b => !b.CurPosition.Equals(applePosition)))
            {
                var food = _foodFactory.Create(applePosition, STEP, STEP);
                _field.PutOn(food);
            }
            else
            {
                PutOnApple();
            }
        }

        #endregion
    }

    public enum GameStatus { None, Running, Pause, Stoped, Restart, GameOver }
}
