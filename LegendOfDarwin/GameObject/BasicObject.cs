using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    // Basic object which can take up a space on the game board
    public class BasicObject
    {
        // The x coordinate to be put on the GameBoard class
        public int X;
        // The y coordinate to be put on the GameBoard class
        public int Y;

        // used for actions that only need to be done every so many iterations
        protected int eventLagMin, eventLagMax;
        protected bool eventFlag;

        // mostly used for only basic objects that can get sucked into the vortex
        protected bool visible = true;

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

        // change destination rectangle
        public void setDestination(Rectangle rectangle)
        {
            destination = rectangle;
        }

        // can it be seen?
        public void setVisible(bool isVisible)
        {
            visible = isVisible;
        }

        // methods for moving a object on the game board
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

        // is this object on top of another game object?
        // only makes sense if one or both of the objects does not set the space it is on to occupied
        public bool isOnTop(BasicObject bo)
        {
            if (bo.X == this.X && bo.Y == this.Y)
            {
                return true;
            }
            return false;
        }

        public bool isOnTop(int x, int y)
        {
            if (x == this.X && y == this.Y)
            {
                return true;
            }
            return false;
        }

        // has enough time elapsed so that event flag has been thrown?
        public bool canEventHappen()
        {
            return this.eventFlag;
        }

        // reset event flag
        public void setEventFalse()
        {
            this.eventFlag = false;
            eventLagMin = 0;
        }

        // set a threshold for the event counter
        public void setEventLag(int lag)
        {
            this.eventLagMax = lag;
        }

        // increment event counter
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
