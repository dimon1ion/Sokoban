using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public abstract class Coord // Coord для Box, Bomb, Player
    {
        #region Parameters

        public int x { get; set; }
        public int y { get; set; }

        #endregion

        #region Constructor

        protected Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion
    }
}
