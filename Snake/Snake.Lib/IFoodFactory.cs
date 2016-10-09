namespace Snake.Lib
{
    public interface IFoodFactory
    {
        IFoodComponent Create(Position position, int width, int height);
    }
}
