namespace Snake.Lib
{
    public interface IBodyFactory
    {
        ISnakeComponent Create(Position position, GameComponentType type, Direction direction, int width, int height);
    }
}
