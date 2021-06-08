using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public class Map //XMLSerializer
    {
        #region Parameters

        public char[][] location;
        public List<Box> boxes;
        public List<Bomb> bombs;
        public Player player;

        #endregion

        #region Constructors

        public Map()
        {

        }
        public Map(char[][] location, List<Box> boxes, Player player, List<Bomb> bombs)
        {
            this.location = location;
            this.boxes = boxes;
            this.player = player;
            this.bombs = bombs;
        }

        #endregion
    }
}
