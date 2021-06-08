using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sokoban
{
    public class Game
    {
        #region Parameter

        private List<Level> levels;

        #endregion

        #region Constructor

        public Game(List<Level> levels)
        {
            this.levels = levels;
        }

        #endregion

        #region Play

        public int Play(int numLevel)
        {
            bool isEnd = true;
            int currentLvl = 1;
            foreach (var level in levels.Where(x => x.numLevel >= numLevel))
            {
                Console.Clear();
                currentLvl = level.numLevel;
                Console.Write($"Level: {level.numLevel}");
                if (!level.PlayLevel())
                {
                    isEnd = false;
                    break;
                }
            }
            Console.Clear();
            if (isEnd)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Congratulations, you completed the game!!!\n");
                MessageBox.Show("Congratulations, you completed the game!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.Write("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
            return currentLvl;
        }

        #endregion
    }
}
