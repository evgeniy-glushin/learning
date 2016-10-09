using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Lib
{
    public interface ISnakeField
    {
        Position Position { get; }

        int Width { get; set; }
        int Height { get; set; }

        int Left { get; }
        int Top { get; }
        int Right { get; }
        int Bottom { get; }

        int LeftMargin { get; set; }
        int RightMargin { get; set; }
        int TopMargin { get; set; }
        int BottomMargin { get; set; }


        bool IsOutOfBounds(Position position, int step);

        void Clear();

        void PutOn(IGameComponent item);
        void PickUp(IGameComponent item);

        Snake Snake { get; set; }
        List<IFoodComponent> Apples { get; }

        void Draw(bool isDrawing);
    }
}
