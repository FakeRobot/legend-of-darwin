using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    public class CannibalZombie : Zombie
    {

        //flag for if zombie is in 'bug2' mode
        protected bool goAroundMode;

        protected Vector2[] path;
        protected int pathCount;
        protected int pathLimit;

        public CannibalZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard):
            base(startX,startY,mymaxX,myminX, mymaxY, myminY, myboard)
            {
                allowRangeDetection = false;
                allowVision = true;
                goAroundMode = false;
                visionMaxX = 5;
                visionMaxY = 5;
                pathCount = 0;
                pathLimit = 0;
            }

        // checks if a given game board point is in the zombie's vision
        public bool isPointInVision(int myX, int myY) 
        {
            if (myX <= this.X+visionMaxX && myX>=this.X-visionMaxX && myY <= this.Y+visionMaxY && myY>= this.Y-visionMaxY)
                return true;
            else
                return false;
        }

        /**
         * moves cannibal towards a specified point within the cannibals range
         * will go around stuff to get there
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
                    intendedPathY = this.Y+1;
                }
                else if (ptY < this.Y)
                {
                    //intend to move up
                    intendedPathX = this.X;
                    intendedPathY = this.Y-1;
                }
            }

            if (isZombieInRange(intendedPathX, intendedPathY))
            {
                // checks for board position to be open
                if (board.isGridPositionOpen(intendedPathX, intendedPathY))
                {
                    if (intendedPathX == this.X + 1)
                        MoveRight();
                    else if (intendedPathX == this.X - 1)
                        MoveLeft();
                    else if (intendedPathY == this.Y + 1)
                        MoveDown();
                    else if (intendedPathX == this.Y - 1)
                        MoveUp();
                }
                else
                {
                    //implement some sort of bug2 here
                    goAroundMode = true;
                    pathCount = 0;
                    Search mysearch = new Search(board);
                    path = mysearch.aStar(this.X,this.Y,ptX,ptY);
                    pathLimit = mysearch.getLength();
                    
                    if (!mysearch.isSolution()) 
                    {
                        //if there is no path, just randomwalk to change things up
                        goAroundMode = false;
                        RandomWalk();
                    }
                
                }
            }
        }

        // used to pick which way around a obstacle zombie should go
        // zombie should be right next to an obstacle when this is called
        public void findBestDir(int ptX, int ptY)
        {
            

        }

        public void goAroundObstacle(int ptX, int ptY)
        {
            if (goAroundMode) 
            {

                if (pathCount < pathLimit)
                {
                    Vector2 nextPoint = path[pathCount];

                    this.setGridPosition((int)nextPoint.X, (int)nextPoint.Y);
                    board.setGridPositionOccupied((int)nextPoint.X, (int)nextPoint.Y);
                    board.setGridPositionOpen((int)nextPoint.X, (int)nextPoint.Y);

                    pathCount++;
                }
                else 
                {
                    goAroundMode = false;
 
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

            if (this.isOnTop(darwin) && darwin.isZombie())
            {
                darwin.setGridPosition(2, 2);
            }

            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                
                if (isVisionAllowed() && isDarwinInRange(darwin) && darwin.isZombie())
                    this.moveTowardsPoint(darwin.X,darwin.Y);
                else
                    this.RandomWalk();


                movecounter = 0;
            }
            movecounter++;

        }

    }
}
