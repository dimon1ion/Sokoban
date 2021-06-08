using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public class Bomb : Coord
    {
        #region Constructors

        public Bomb() : base(0, 0)
        {
        }
        public Bomb(int x, int y) : base(x, y)
        {
        }

        #endregion
    }
}
