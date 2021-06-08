using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban.Interfaces
{
    interface IMoveable // IMoveable для Box и Player
    {
        bool Move(ref Map map, ConsoleKeyInfo cKeyInfo);
        void Refresh();
    }
}
