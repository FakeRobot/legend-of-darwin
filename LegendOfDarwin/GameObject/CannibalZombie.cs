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
        protected Darwin darwin;
        protected List<Zombie> zombies;

        public CannibalZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY,List<Zombie> myListZombies,Darwin mydarwin,GameBoard myboard):
            base(startX,startY,mymaxX,myminX, mymaxY, myminY, myboard)
            {
                allowRangeDetection = false;
                allowVision = true;
                goAroundMode = false;
                visionMaxX = 8;
                visionMaxY = 8;
                pathCount = 0;
                pathLimit = 0;
                darwin = mydarwin;
                zombies = myListZombies;
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

                bool canMoveThere = false;
                foreach (Zombie zombieToEat in zombies) 
                {
                    if (zombieToEat.X == intendedPathX && zombieToEat.Y == intendedPathY) 
                    {
                        canMoveThere = true;
                    }
 
                }

                if (darwin.X == intendedPathX && darwin.Y == intendedPathY && darwin.isZombie())
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

        // used when zombie is running towards something and an obstacle is in the way
        public void goAroundObstacle()
        {
            if (goAroundMode) 
            {

                if (pathCount < pathLimit)
                {
                    Vector2 nextPoint = path[pathCount];

                    
                    board.setGridPositionOccupied((int)nextPoint.X, (int)nextPoint.Y);
                    board.setGridPositionOpen((int)this.X, (int)this.Y);
                    this.setGridPosition((int)nextPoint.X, (int)nextPoint.Y);

                    pathCount++;
                }
                else 
                {
                    goAroundMode = false;
 
                }

            }

        }

        // lets the cannibal know which zombies are still on the level
        public void updateListOfZombies(List<Zombie> myZombieList) 
        {
            zombies = myZombieList;
        }

        public void CollisionWithZombie(Zombie zombie)
        {
            if (this.isOnTop(zombie))
            {
                zombie.setZombieAlive(false);
            }
        }

        public void Update(GameTime gameTime, Darwin darwin) 
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
                foreach (Zombie myzombie in zombies)
                    CollisionWithZombie(myzombie);
                //these loops are seperate because the top loop could potentially remove zombies from the list

                int intendedptX = 0;
                int intendedptY = 0;
                bool hasZombieDest = false;
                bool goForDarwin = true;
                int closestZombieDist = 10000;

                foreach (Zombie myzombie in zombies) 
                {
                    if (isVisionAllowed() && isPointInVision(myzombie.X, myzombie.Y) && myzombie.isZombieAlive()) 
                    {
                        int dist = Math.Abs(this.X - myzombie.X) + Math.Abs(this.Y - myzombie.Y);
                        if (dist < closestZombieDist)
                        {
                            closestZombieDist = dist;
                            intendedptX = myzombie.X;
                            intendedptY = myzombie.Y;
                            hasZombieDest = true;
                            goForDarwin = false;
                        }
 
                    }
                }

                if (hasZombieDest)
                {
                    if (Math.Abs(this.X - darwin.X) + Math.Abs(this.Y - darwin.Y) <= closestZombieDist)
                    {
                        goForDarwin = true;
                        hasZombieDest = false;
                    }
                }

                if (goAroundMode)
                    goAroundObstacle();
                else if (isVisionAllowed() && isPointInVision(darwin.X, darwin.Y) && darwin.isZombie() && goForDarwin)
                    this.moveTowardsPoint(darwin.X, darwin.Y);
                else if (hasZombieDest)
                    this.moveTowardsPoint(intendedptX,intendedptY);
                else
                    this.RandomWalk();


                movecounter = 0;
            }
            movecounter++;

        }

    }
}
