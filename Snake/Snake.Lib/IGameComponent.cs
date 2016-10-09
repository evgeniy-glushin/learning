using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake.Lib
{
    public interface IGameComponent
    {
        Position CurPosition { get; set; }
        GameComponentType Type { get; }
        int Width { get; }
        int Height { get; }
    }
}
