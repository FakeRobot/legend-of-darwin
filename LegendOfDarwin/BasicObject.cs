using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class BasicObject
    {
        // The x coordinate to be put on the GameBoard class
        public int X;
        // The y coordinate to be put on the GameBoard class
        public int Y;

        // The location to draw the sprite on the screen.
        public Rectangle destination;

        // the board that the zombie is moving on
        public GameBoard board;

        public BasicObject(GameBoard myBoard)
        {
            board = myBoard;
        }

        /**
         * sets the screen position of the object
         * int myX, int myY screen positions of the object
         * */
        public void setPosition(int myX, int myY)
        {
            // Update the destination
            destination.X = myX;
            destination.Y = myY;
        }

        public void MoveRight()
        {
            this.setGridPosition(this.X + 1, this.Y);
            board.setGridPositionOccupied(this.X, this.Y);
            board.setGridPositionOpen(this.X - 1, this.Y);
            this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
        }

        public void MoveLeft()
        {
            this.setGridPosition(this.X - 1, this.Y);
            board.setGridPositionOccupied(this.X, this.Y);
            board.setGridPositionOpen(this.X + 1, this.Y);
            this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
        }

        public void MoveDown()
        {
            this.setGridPosition(this.X, this.Y + 1);
            board.setGridPositionOccupied(this.X, this.Y);
            board.setGridPositionOpen(this.X, this.Y - 1);
            this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
        }

        public void MoveUp()
        {
            this.setGridPosition(this.X, this.Y - 1);
            board.setGridPositionOccupied(this.X, this.Y);
            board.setGridPositionOpen(this.X, this.Y + 1);
            this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
        }

        // Set their positions
        public void setGridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
