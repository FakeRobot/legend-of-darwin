using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class GameBoard
    {
        private int squareLength, squareWidth;
        private Rectangle[,] grid;
        private Boolean[,] hasObject;
        private int gridLength;
        private int gridWidth;

       
        public GameBoard(Vector2 boardSize, Vector2 windowSize)
        {
            squareLength = (int)(windowSize.Y / boardSize.Y);
            squareWidth = (int)(windowSize.X / boardSize.X);

            gridLength = (int)boardSize.Y;
            gridWidth = (int)boardSize.X;

            grid = new Rectangle[(int)boardSize.X, (int)boardSize.Y];
            hasObject = new Boolean[(int)boardSize.X, (int)boardSize.Y];

            for (int i = 0; i < boardSize.X; i++)
            {
                for (int j = 0; j < boardSize.Y; j++)
                {
                    grid[i,j] = new Rectangle(i * squareWidth, j * squareLength, squareWidth, squareLength);
                    hasObject[i,j] = false;
                }
            }
        }

        public Boolean isTaken(int x, int y)
        {
            if (x < 0 || x > gridWidth)
            {
                return true;
            }
            if (y < 0 || y > gridLength)
            {
                return true;
            }
            return hasObject[x,y];
        }

        public Boolean isGridPositionOpen(BasicObject bo)
        {
            if (isTaken(bo.X, bo.Y))
            {
                return false;
            }

            hasObject[bo.X,bo.Y] = true;

            return true;
        }

        public Rectangle getPosition(BasicObject bo)
        {
            return grid[bo.X,bo.Y];
        }

        public void freePosition(BasicObject bo)
        {
            hasObject[bo.X,bo.Y] = false;
        }

        public void setGridPositionOpen(int x, int y)
        {
            if (x < 0 || x > gridWidth)
            {
            }
            else if (y < 0 || y > gridLength)
            {
            }
            else
            {
                hasObject[x, y] = false;
            }
        }

        public int getSquareLength()
        {
            return this.squareLength;
        }

        public int getSquareWidth()
        {
            return this.squareWidth;
        }
    }
}
