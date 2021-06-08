using System;
using System.Linq;
using System.Text;
using Sokoban.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public class Player : Coord, IMoveable
    {
        #region Parameters

        public int X_Default { get; set; }
        public int Y_Default { get; set; }

        #endregion

        #region Constructors

        public Player() : base(0, 0)
        {

        }
        public Player(int x, int y) : base(x, y)
        {
            X_Default = x;
            Y_Default = y;
        }

        #endregion

        #region Refresh&Move

        public void Refresh()
        {
            x = X_Default;
            y = Y_Default;
        }

        public bool Move(ref Map map, ConsoleKeyInfo cKeyInfo)
        {
            int newX;
            int newY;
            switch (cKeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newX = x;
                    newY = y - 1;
                    break;
                case ConsoleKey.LeftArrow:
                    newX = x - 1;
                    newY = y;
                    break;
                case ConsoleKey.RightArrow:
                    newX = x + 1;
                    newY = y;
                    break;
                case ConsoleKey.DownArrow:
                    newX = x;
                    newY = y + 1;
                    break;
                default:
                    return false;
            }
            if (0 <= newX && 0 <= newY && newY < map.location.GetLength(0))
            {
                if (newX < map.location[newY].GetLength(0))
                {
                    if (map.location[newY][newX] == ' ')
                    {
                        foreach (var box in map.boxes.Where(x => x.y == newY && x.x == newX))
                        {
                            if (!box.Move(ref map, cKeyInfo))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                    x = newX;
                    y = newY;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
