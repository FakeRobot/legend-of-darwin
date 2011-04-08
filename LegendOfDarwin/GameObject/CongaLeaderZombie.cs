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

        //amt that sprite should be shifted up in order to look natural
        protected int amtShiftUp = 0;

        // used to see when an all out atk should be done
        protected bool killMode = false;

        // list of all follower zombies on level
        protected List<CongaFollowerZombie> followerZombies;

        // you should set ranges to whole board
        public CongaLeaderZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, Vector2[] myPathList, 
            Darwin mydarwin, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard) 
        {
            allowRangeDetection = false;
            allowVision = true;
            visionMaxX = 6;
            visionMaxY = 6;
            darwin = mydarwin;
            pathList = myPathList;
            ZOMBIE_MOVE_RATE = 20;
            followerZombies = new List<CongaFollowerZombie>();
        }

        // loads in sprite as well as shifts sprite to look natural 
        public new void LoadContent(Texture2D myZombieTexture) 
        {
            base.LoadContent(myZombieTexture, myZombieTexture);

            // 100 is height of sprite on sheet, the destination is shifted proportionally to the gameboard
            source.Height=100;
            destination.Height = (100 / 64) * board.getSquareWidth() + 10;
            amtShiftUp = (1 / 2) * board.getSquareWidth() + 10;
            destination.Y -= amtShiftUp;
        }

        /*
         * resets conga leaders position
         * myx, myy are restart posit for conga zombie on gameboard
         */
        public void Reset(int myx, int myy) 
        {
            board.setGridPositionOpen(this.X, this.Y);
            this.setGridPosition(myx, myy);
            board.setGridPositionOccupied(this.X, this.Y);
            this.setZombieAlive(true);
            killMode = false;

            this.pathCount = 0;
            //fix sprite
            destination.Height = (100 / 64) * board.getSquareWidth() + 10;
            destination.Y -= amtShiftUp;
        }

        // ATTACK!!!
        public void activateKillMode()
        {
            killMode = true;
        }

        // checks if zombies are in kill mode or not
        public bool isKillMode() 
        {
            return killMode;
        }

        // set what follower zombies there are on the level
        public void setFollowers(List<CongaFollowerZombie> myFollowers) 
        {
            followerZombies = myFollowers;
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
                

                if (darwin.X == intendedPathX && darwin.Y == intendedPathY)
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
                        destination.Height = (100 / 64) * board.getSquareWidth() + 10;
                        destination.Y -= amtShiftUp;

                }
            }

        }

        /*
         * checks if darwin is on the dance floor or not
         * that is, is darwin inside the patrol path
         * Point must be set up so top left pt is first,
         * bottom right pt is 3rd
         * */
        public bool isDarwinOnFloor(Darwin myDarwin) 
        {
            // 
            Vector2 minPt = pathList[0];
            Vector2 maxPt = pathList[2];

            if (myDarwin.X >= minPt.X && myDarwin.X <= maxPt.X && myDarwin.Y >= minPt.Y && myDarwin.Y <= maxPt.Y)
                return true;
            else
                return false;
        }

        /*
         * checks if darwin is on the conga line path or not
         * that is, is darwin on the patrol path
         * Point must be set up so top left pt is first,
         * bottom right pt is 3rd
         * */
        public bool isDarwinOnPath(Darwin myDarwin)
        {
            // 
            Vector2 minPt = pathList[0];
            Vector2 maxPt = pathList[2];

            if ((myDarwin.X >= minPt.X && myDarwin.X <= maxPt.X && (myDarwin.Y == minPt.Y || myDarwin.Y == maxPt.Y))
                || ((myDarwin.X == minPt.X || myDarwin.X == maxPt.X) && myDarwin.Y >= minPt.Y && myDarwin.Y <= maxPt.Y))
                return true;
            else
                return false;
        }


        /*
         * These overides of the move functions are meant to refix the 
         * sprite when a movement is made
         * This keeps the sprite looking natural, since it is too tall for the square
         * */
        public new void MoveRight()
        {
            base.MoveRight();
            destination.Height = (100/64)*board.getSquareWidth()+10;
            destination.Y -= amtShiftUp;
        }

        public new void MoveLeft()
        {
            base.MoveLeft();
            destination.Height = (100 / 64) * board.getSquareWidth()+10;
            destination.Y -= amtShiftUp;
        }

        public new void MoveDown()
        {
            base.MoveDown();
            destination.Height = (100 / 64) * board.getSquareWidth()+10;
            destination.Y -= amtShiftUp;
        }

        public new void MoveUp()
        {
            base.MoveUp();
            destination.Height = (100 / 64) * board.getSquareWidth()+10;
            destination.Y -= amtShiftUp;
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
                if (killMode) 
                {
                    ZOMBIE_MOVE_RATE = 10;
                    this.enemyAlert = true;
                    source.X = 64;
                    moveTowardsPoint(darwin.X, darwin.Y);

                    foreach (CongaFollowerZombie follower in followerZombies)
                        follower.activateKillMode();
                }
                else if (isDarwinOnFloor(darwin) && !darwin.isZombie())
                {
                    ZOMBIE_MOVE_RATE = 10;
                    this.enemyAlert = true;
                    source.X = 64;
                    moveTowardsPoint(darwin.X,darwin.Y);
                }
                else if (isDarwinOnFloor(darwin) && !isDarwinOnPath(darwin)) 
                {
                    ZOMBIE_MOVE_RATE = 10;
                    // case where darwin is a zombie not in the conga line
                    this.enemyAlert = true;
                    source.X = 64;
                    moveTowardsPoint(darwin.X, darwin.Y);
                }
                else
                {
                    ZOMBIE_MOVE_RATE = 20;
                    if (enemyAlert)
                    {
                        source.X = 128;
                        enemyAlertCount++;
                        if (enemyAlertCount > 2)
                        {
                            enemyAlert = false;
                            enemyAlertCount = 0;
                        }
                    }
                    else
                        this.source.X = 0;

                    followPath();
                }

                movecounter = 0;
            }
            movecounter++;
            
        }



    }
}
