using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    public class BasicObject
    {
        // The x coordinate to be put on the GameBoard class
        public int X;
        // The y coordinate to be put on the GameBoard class
        public int Y;

        protected int eventLagMin, eventLagMax;

        protected bool eventFlag;

        // The location to draw the sprite on the screen.
        public Rectangle destination;

        // the board that the zombie is moving on
        public GameBoard board;

        public BasicObject(GameBoard myBoard)
        {
            board = myBoard;

            eventLagMax = eventLagMin = 5;
            eventFlag = true;
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

        public void setDestination(Rectangle rectangle)
        {
            destination = rectangle;
        }

        public void MoveRight()
        {
                this.setGridPosition(this.X + 1, this.Y);
                board.setGridPositionOccupied(this.X, this.Y);
                board.setGridPositionOpen(this.X - 1, this.Y);
        }

        public void MoveLeft()
        {
                this.setGridPosition(this.X - 1, this.Y);
                board.setGridPositionOccupied(this.X, this.Y);
                board.setGridPositionOpen(this.X + 1, this.Y);
        }

        public void MoveDown()
        {
                this.setGridPosition(this.X, this.Y + 1);
                board.setGridPositionOccupied(this.X, this.Y);
                board.setGridPositionOpen(this.X, this.Y - 1);
        }

        public void MoveUp()
        {
                this.setGridPosition(this.X, this.Y - 1);
                board.setGridPositionOccupied(this.X, this.Y);
                board.setGridPositionOpen(this.X, this.Y + 1);
        }

        // Set their positions
        public void setGridPosition(int x, int y)
        {
            X = x;
            Y = y;

            setDestination(board.getPosition(X, Y));
        }

        public bool isOnTop(BasicObject bo)
        {
            if (bo.X == this.X && bo.Y == this.Y)
            {
                return true;
            }
            return false;
        }

        public bool canEventHappen()
        {
            return this.eventFlag;
        }

        public void setEventFalse()
        {
            this.eventFlag = false;
            eventLagMin = 0;
        }

        public void setEventLag(int lag)
        {
            this.eventLagMax = lag;
        }

        public void Update(GameTime gameTime)
        {
            eventLagMin++;
            if (eventLagMin > eventLagMax)
            {
                this.eventFlag = true;
            }
        }
    }
}
