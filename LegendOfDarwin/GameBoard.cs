using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class GameBoard
    {
        private Rectangle[][] grid;
        private Boolean[][] hasObject;

        public GameBoard(Vector2 boardSize, Vector2 windowSize)
        {
            int length = (int)(windowSize.Y / boardSize.Y);
            int width = (int)(windowSize.X / boardSize.X);

            for (int i = 0; i < boardSize.X; i++)
            {
                for (int j = 0; j < boardSize.Y; j++)
                {
                    grid[i][j] = new Rectangle(i * width, j * length, width, length);
                    hasObject[i][j] = false;
                }
            }
        }

        public Boolean isTaken(int x, int y)
        {
            return hasObject[x][y];
        }

        public Boolean setPosition(BasicObject bo)
        {
            if (isTaken(bo.X, bo.Y))
            {
                return false;
            }

            hasObject[bo.X][bo.Y] = true;

            return true;
        }

        public void freePosition(int x, int y)
        {
            hasObject[x][y] = false;
        }

    }
}
