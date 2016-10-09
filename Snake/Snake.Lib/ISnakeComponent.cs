using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Lib
{
    public interface ISnakeComponent : IGameComponent
    {
        Position PrevPosition { get; set; }

        Direction Direction { get; set; }
    }
}
