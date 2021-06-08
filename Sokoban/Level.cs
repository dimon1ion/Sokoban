using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public class Level
    {
        #region Parameters

        public delegate void Del();
        public event Del RefreshAll;
        public Map map;
        public int numLevel { get; set; }

        #endregion

        #region Constructor

        public Level(Map map, int numLevel)
        {
            this.map = map;
            this.numLevel = numLevel;
            foreach (var box in map.boxes)
            {
                RefreshAll += box.Refresh;
            }
            RefreshAll += map.player.Refresh;
        }

        #endregion

        #region Play&PrintMap&Check


        public bool PlayLevel()
        {
            RefreshAll();
            bool print = true;
            bool promt = true;
            while (true)
            {
                if (print)
                {
                    PrintMap(ref promt);
                }
                print = true;
                ConsoleKeyInfo cKeyInfo = Console.ReadKey();
                switch (cKeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.DownArrow:
                        if (!(map.player.Move(ref map, cKeyInfo)))
                        {
                            print = false;
                            continue;
                        }
                        Console.Beep(450, 40);
                        break;
                    case ConsoleKey.Tab:
                        RefreshAll();
                        continue;
                    case ConsoleKey.Backspace:
                        return false;
                    default:
                        print = false;
                        continue;
                }
                if (Check())
                {
                    break;
                }
            }
            return true;
        }
        private void PrintMap(ref bool promt) //Отображение локации
        {
            Console.SetCursorPosition(2, 2);
            bool isBomb;
            int length2;
            for (int i = 0; i < map.location.GetLength(0); i++)
            {
                Console.SetCursorPosition(2, 2+i);
                length2 = map.location[i].GetLength(0);
                for (int j = 0; j < length2; j++)
                {
                    isBomb = false;
                    if (map.location[i][j] == '1')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(map.location[i][j]);
                        if (j != length2 - 1)
                        {
                            Console.Write(' ');
                        }
                    }
                    else if (map.location[i][j] == '#')
                    {
                        Console.Write(map.location[i][j]);
                        if (j != length2 - 1)
                        {
                            Console.Write(' ');
                        }
                    }
                    else
                    {
                        if (map.bombs.Where(x => (x.y == i) && (x.x == j)).ToList().Count != 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            isBomb = true;
                        }
                        if (map.boxes.Where(x => (x.y == i) && (x.x == j)).ToList().Count != 0)
                        {
                            if (!isBomb)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write('X');
                            if (j != length2 - 1)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write(' ');
                            }
                        }
                        else if (map.player.y == i && map.player.x == j)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('O');
                            if (j != length2 - 1)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write(' ');
                            }
                        }
                        else
                        {
                            if (!isBomb)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            Console.Write(' ');
                            if (j != length2 - 1)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write(' ');
                            }
                        }
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            if (promt)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nUp Arrow - \tmove up");
                Console.WriteLine("Down Arrow - \tmove down");
                Console.WriteLine("Left Arrow - \tmove left");
                Console.WriteLine("Right Arrow - \tmove right");
                Console.WriteLine("Key Tab - \trestart level");
                Console.WriteLine("Key Backspace - Exit and save");
                Console.ForegroundColor = ConsoleColor.White;
                promt = false;
            }
        }
        public bool Check()
        {
            int boxOnBomb = 0;
            foreach (var bomb in map.bombs)
            {
                foreach (var box in map.boxes.Where(x => x.x == bomb.x && x.y == bomb.y))
                {
                    boxOnBomb++;
                    break;
                }
            }
            if (map.bombs.Count == boxOnBomb)
            {
                return true;
            }
            return false;
        }
        
        #endregion
    }
    
}
