using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class GameBoard
    {
        // Length and width of each square/rectangle
        private int squareLength, squareWidth;

        // 2d array representation of the game floor. 
        // Each rectangle upon each grid position contains the necessary information for each tile to be placed on
        private Rectangle[,] grid;

        // A flag of each grid to see if an object occupies it
        private Boolean[,] hasObject;

        // Number of squares going down
        private int gridLength;

        // Number of squares going right
        private int gridWidth;

        /*
         * boardSize contains the x and y length and width of the whole gameboard.
         * boardSize.X = 25 and boardSize.Y = 25 would translate to a 25 X 25 board
         * windowSize contains the x and y length and width of the game's window's size.
         * windowSize.X = 900 and windowSize.Y = 600 would translate to a 900 X 600 window size in pixels
         */
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


        /*
         * x is the width or 'going right' position on the grid
         * y is the length or 'going down' position on the grid
         * x = 3, y = 5 would translate to 3 to the right and 5 down
         * Both start at 0
         */ 
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

        /*
         * bo contains the coordinates on the grid to be checked to see if its opened or not
         * returns true on open, false on not
         */ 
        public Boolean isGridPositionOpen(BasicObject bo)
        {
            if (isTaken(bo.X, bo.Y))
            {
                return false;
            }

            hasObject[bo.X,bo.Y] = true;

            return true;
        }

        /*
         * PreCon: The bo's coordinates have the actually position using the function isGridPositionOpen
         * Gets the rectangle based on the bo's coordinates on the grid
         */ 
        public Rectangle getPosition(BasicObject bo)
        {
            return grid[bo.X,bo.Y];
        }

        /*
         * Frees the coordinates on the grid based on bo's coordinates
         */ 
        public void freePosition(BasicObject bo)
        {
            hasObject[bo.X,bo.Y] = false;
        }

        /*
         * Does the same as freePosition
         */ 
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

        /*
         * Exactly what it says
         */ 
        public int getSquareLength()
        {
            return this.squareLength;
        }

        /*
         * Exactly what it says
         */ 
        public int getSquareWidth()
        {
            return this.squareWidth;
        }
    }
}
