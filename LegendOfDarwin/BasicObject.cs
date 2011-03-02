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

        /*
        public BasicObject(int x, int y)
        {
            setGridPosition(x, y);
        }

        public BasicObject()
        {
            X = 0;
            Y = 0;
        }
        */

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

        /*
         * moves the object to the right of current position on the game board
         * */
        public void MoveRight()
        {
            this.setGridPosition(this.X + 1, this.Y);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X - 1, this.Y);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X - 1, this.Y);
            }
        }

        /*
         * moves the object to the left of current position on the game board
         * */
        public void MoveLeft()
        {
            this.setGridPosition(this.X - 1, this.Y);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X + 1, this.Y);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X + 1, this.Y);
            }
        }

        /*
         * moves the object down one square on the game board
         * */
        public void MoveDown()
        {
            this.setGridPosition(this.X, this.Y + 1);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X, this.Y - 1);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X, this.Y - 1);
            }
        }

        /*
         * moves the object one up on the game board
         * */
        public void MoveUp()
        {
            this.setGridPosition(this.X, this.Y - 1);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X, this.Y + 1);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X, this.Y + 1);
            }
        }

        // Set their positions
        public void setGridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }




    }
}
