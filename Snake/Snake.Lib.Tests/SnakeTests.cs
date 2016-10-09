using NSubstitute;
using NUnit.Framework;

namespace Snake.Lib.Tests
{
    [TestFixture]
    public class SnakeTests
    {
        private int _step = 15;
        private Snake _snake;

        [SetUp]
        public void Init()
        {
            _snake = new Snake(null, null, null, _step);
        }

        public void GoRight_left_to_right_direction()
        {
            //Arrange
            GroveSnake(new Position(30, 30), new Position(30, 15));
            GroveSnake(new Position(30, 15), null);
            GroveSnake(new Position(30, 0), null);

            //Act
            _snake.GoRight();

            //Assert
            Assert.IsTrue(SnakeOffset(_snake, new Position(30, 45)));
        }

        //[Test]
        //public void GoStraight_left_to_right_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 15));
        //    GroveSnake(new Position(30, 15), null);
        //    GroveSnake(new Position(30, 0), null);

        //    //Act
        //    _snake.GoStraight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 45)));
        //}

        //[Test]
        //public void GoStraight_right_to_left_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 45));
        //    GroveSnake(new Position(30, 45), null);
        //    GroveSnake(new Position(30, 60), null);

        //    //Act
        //    _snake.GoStraight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 15)));
        //}

        //[Test]
        //public void GoStraight_top_to_bottom_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(15, 30));
        //    GroveSnake(new Position(15, 30), null);
        //    GroveSnake(new Position(0, 30), null);

        //    //Act
        //    _snake.GoStraight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(45, 30)));
        //}

        //[Test]
        //public void GoStraight_bottom_to_top_direction()
        //{
        //    //Arrange         
        //    GroveSnake(new Position(30, 30), new Position(45, 30));
        //    GroveSnake(new Position(45, 30), null);
        //    GroveSnake(new Position(60, 30), null);

        //    //Act
        //    _snake.GoStraight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(15, 30)));
        //}



        //[Test]
        //public void TurnRight_left_to_right_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 15));
        //    GroveSnake(new Position(30, 15), null);
        //    GroveSnake(new Position(30, 0), null);

        //    //Act
        //    _snake.TurnRight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(45, 30)));
        //}

        //[Test]
        //public void TurnRight_right_to_left_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 45));
        //    GroveSnake(new Position(30, 45), null);
        //    GroveSnake(new Position(30, 60), null);

        //    //Act
        //    _snake.TurnRight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(15, 30)));
        //}

        //[Test]
        //public void TurnRight_top_to_bottom_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(15, 30));
        //    GroveSnake(new Position(15, 30), null);
        //    GroveSnake(new Position(0, 30), null);

        //    //Act
        //    _snake.TurnRight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 15)));
        //}

        //[Test]
        //public void TurnRight_bottom_to_top_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(45, 30));
        //    GroveSnake(new Position(45, 30), null);
        //    GroveSnake(new Position(60, 30), null);

        //    //Act
        //    _snake.TurnRight();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 45)));
        //}



        //[Test]
        //public void TurnLeft_left_to_right_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 15));
        //    GroveSnake(new Position(30, 15), null);
        //    GroveSnake(new Position(30, 0), null);

        //    //Act
        //    _snake.TurnLeft();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(15, 30)));
        //}

        //[Test]
        //public void TurnLeft_right_to_left_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 45));
        //    GroveSnake(new Position(30, 45), null);
        //    GroveSnake(new Position(30, 60), null);

        //    //Act
        //    _snake.TurnLeft();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(45, 30)));
        //}

        //[Test]
        //public void TurnLeft_top_to_bottom_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(15, 30));
        //    GroveSnake(new Position(15, 30), null);
        //    GroveSnake(new Position(0, 30), null);

        //    //Act
        //    _snake.TurnLeft();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 45)));
        //}

        //[Test]
        //public void TurnLeft_bottom_to_top_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(45, 30));
        //    GroveSnake(new Position(45, 30), null);
        //    GroveSnake(new Position(60, 30), null);

        //    //Act
        //    _snake.TurnLeft();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 15)));
        //}



        //[Test]
        //public void TurnAround_left_to_right_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 15));
        //    GroveSnake(new Position(30, 15), null);
        //    GroveSnake(new Position(30, 0), null);

        //    //Act
        //    _snake.TurnAround();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, -15)));
        //}

        //[Test]
        //public void TurnAround_right_to_left_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(30, 45));
        //    GroveSnake(new Position(30, 45), null);
        //    GroveSnake(new Position(30, 60), null);

        //    //Act
        //    _snake.TurnAround();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(30, 75)));
        //}

        //[Test]
        //public void TurnAround_top_to_bottom_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(15, 30));
        //    GroveSnake(new Position(15, 30), null);
        //    GroveSnake(new Position(0, 30), null);

        //    //Act
        //    _snake.TurnAround();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(-15, 30)));
        //}

        //[Test]
        //public void TurnAround_bottom_to_top_direction()
        //{
        //    //Arrange
        //    GroveSnake(new Position(30, 30), new Position(45, 30));
        //    GroveSnake(new Position(45, 30), null);
        //    GroveSnake(new Position(60, 30), null);

        //    //Act
        //    _snake.TurnAround();

        //    //Assert
        //    Assert.IsTrue(SnakeOffset(_snake, new Position(75, 30)));
        //}


        private void GroveSnake(Position curPos, Position prevPos)
        {
            //var item = Substitute.For<IFoodComponent>();
            //item.CurPosition.Returns(h => curPos);
            //if (prevPos != null) item.PrevPosition.Returns(h => prevPos);
            //_snake.Eat(item);
        }

        private bool SnakeOffset(Snake snake, Position expectedPosition)
        {
            if (!snake.Head.CurPosition.Equals(expectedPosition))
                throw new AssertionException($"Snake current position is {snake.Head.CurPosition} but expected {expectedPosition}");

            for (int i = 0; i < snake.Body.Count; i++)
            {
                var next = i == 0 ? snake.Head : snake.Body[i - 1];
                var current = snake.Body[i];

                if (!current.CurPosition.Equals(next.PrevPosition))
                {
                    throw new AssertionException($"Body item [{i}] position is ({current.CurPosition}) but expected ({next.PrevPosition})");
                }
            }

            return true;
        }
    }
}
