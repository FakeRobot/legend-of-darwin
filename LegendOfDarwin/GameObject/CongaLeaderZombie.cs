using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    class CongaLeaderZombie : Zombie
    {
        // darwin that is on the current game board
        protected Darwin darwin;

        // array and index for designating pts at the vertices of the zombie's patrol path
        protected Vector2[] pathList;
        protected int pathCount = 0;

        // set ranges to whole board
        public CongaLeaderZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, Vector2[] myPathList, Darwin mydarwin, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard) 
        {
            allowRangeDetection = false;
            allowVision = true;
            visionMaxX = 6;
            visionMaxY = 6;
            darwin = mydarwin;
            pathList = myPathList;
        }

        /*
         * makes the zombie follow allong the designated patrol path
         * */
        public void followPath() 
        {
            if (this.X == pathList[pathCount].X && this.Y == pathList[pathCount].Y) 
            {
                pathCount++;
            }

            // reset path if neccessary
            if (pathCount >= pathList.Length) 
            {
                pathCount = 0;
            }

            moveTowardsPoint((int)pathList[pathCount].X,(int)pathList[pathCount].Y);
            
        }

        /*
         * moves zombie towards a given point
         * Used for straight line portions of the patrol path
         * */
        public void moveTowardsPoint(int ptX, int ptY)
        {
            int changeX = 0;
            int changeY = 0;
            int intendedPathX = 0;
            int intendedPathY = 0;

            changeX = ptX - this.X;
            changeY = ptY - this.Y;

            if (Math.Abs(changeX) > Math.Abs(changeY))
            {
                //move in x direction
                if (ptX > this.X)
                {
                    //intend to move right
                    intendedPathX = this.X + 1;
                    intendedPathY = this.Y;

                }
                else if (ptX < this.X)
                {
                    //intend to move left
                    intendedPathX = this.X - 1;
                    intendedPathY = this.Y;
                }
            }
            else
            {
                //move in y direction
                if (ptY > this.Y)
                {
                    //intend to move down
                    intendedPathX = this.X;
                    intendedPathY = this.Y + 1;
                }
                else if (ptY < this.Y)
                {
                    //intend to move up
                    intendedPathX = this.X;
                    intendedPathY = this.Y - 1;
                }
            }

            if (isZombieInRange(intendedPathX, intendedPathY))
            {

                bool canMoveThere = false;
                

                if (darwin.X == intendedPathX && darwin.Y == intendedPathY && !darwin.isZombie())
                    canMoveThere = true;

                // checks for board position to be open
                if (board.isGridPositionOpen(intendedPathX, intendedPathY) || canMoveThere)
                {

                    if (intendedPathX == this.X + 1)
                        MoveRight();
                    else if (intendedPathX == this.X - 1)
                        MoveLeft();
                    else if (intendedPathY == this.Y + 1)
                        MoveDown();
                    else if (intendedPathY == this.Y - 1)
                        MoveUp();
                }
                else
                {
                    
                        RandomWalk();


                }
            }

        }

        public new void Update(GameTime gameTime, Darwin darwin)
        {
            eventLagMin++;
            if (eventLagMin > eventLagMax)
            {
                this.eventFlag = true;
            }

            if (movecounter > ZOMBIE_MOVE_RATE)
            {

                followPath();

                movecounter = 0;
            }
            movecounter++;

        }



    }
}
