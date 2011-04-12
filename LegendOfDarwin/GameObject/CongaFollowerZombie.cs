using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    // basic path following zombie used on level 4
    class CongaFollowerZombie : Zombie
    {
        // darwin that is on the current game board
        protected Darwin darwin;

        // used to delay zombie attack when spotted by CongaLeader
        protected bool hasBeenSeen = false;
        protected int hasBeenSeenCounter = 0;

        // array and index for designating pts at the vertices of the zombie's patrol path
        protected Vector2[] pathList;
        protected int pathCount = 0;

        //amt that sprite should be shifted up in order to look natural
        protected int amtShiftUp = 0;

        //init posit of zombie, used for resetting
        protected Vector2 startPosit;

        // for deciding when zombie darwin should be attacked or not
        protected bool killMode = false;
        protected bool preKillMode = false;

        // the leader in the current level
        protected CongaLeaderZombie leaderZombie;

        // set ranges to whole board
        public CongaFollowerZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, Vector2[] myPathList,CongaLeaderZombie myLeader, Darwin mydarwin, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard) 
        {
            allowRangeDetection = false;
            allowVision = true;
            visionMaxX = 4;
            visionMaxY = 4;
            darwin = mydarwin;
            pathList = myPathList;
            ZOMBIE_MOVE_RATE = 20;
            startPosit = new Vector2(startX,startY);
            leaderZombie = myLeader;
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

            moveTowardsPoint((int)pathList[pathCount].X, (int)pathList[pathCount].Y);

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


                if (darwin.X == intendedPathX && darwin.Y == intendedPathY && (!darwin.isZombie() || killMode))
                    canMoveThere = true;
                else if (darwin.X == intendedPathX && darwin.Y == intendedPathY && darwin.isZombie() && !killMode)
                    preKillMode = true;

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
                    if (!preKillMode)
                        RandomWalk();

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
            // check for actual min.max points
            Vector2 minPt = getMinPtOnPath();
            Vector2 maxPt = getMaxPtOnPath();

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
            Vector2 minPt = getMinPtOnPath();
            Vector2 maxPt = getMaxPtOnPath();

            if ((myDarwin.X >= minPt.X && myDarwin.X <= maxPt.X && (myDarwin.Y == minPt.Y || myDarwin.Y == maxPt.Y))
                || ((myDarwin.X == minPt.X || myDarwin.X == maxPt.X) && myDarwin.Y >= minPt.Y && myDarwin.Y <= maxPt.Y))
                return true;
            else
                return false;
        }

        /* 
         * accesses the pt on the zombies patrol path corresponding to the upper left corner
         **/
        public Vector2 getMinPtOnPath() 
        {
            Vector2 minPt = new Vector2(50, 50);
            for (int i = 0; i < pathList.Length; i++)
            {
                if (pathList[i].X <= minPt.X && pathList[i].Y <= minPt.Y)
                {
                    minPt.X = pathList[i].X;
                    minPt.Y = pathList[i].Y;
                }
            }

            return minPt;
        }

        /* 
         * accesses the pt on the zombies patrol path corresponding to the lower right corner
         **/
        public Vector2 getMaxPtOnPath()
        {
            Vector2 maxPt = new Vector2(0, 0);
            for (int i = 0; i < pathList.Length; i++)
            {
                if (pathList[i].X >= maxPt.X && pathList[i].Y >= maxPt.Y)
                {
                    maxPt.X = pathList[i].X;
                    maxPt.Y = pathList[i].Y;
                }
            }

            return maxPt;
        }

        /**
         * resets follower zombie to initial position on game board
         * */
        public void reset() 
        {
            board.setGridPositionOpen(this.X, this.Y);
            this.setGridPosition((int)startPosit.X,(int) startPosit.Y);
            board.setGridPositionOccupied(this.X, this.Y);
            this.setZombieAlive(true);
            killMode = false;
            preKillMode = false;

            this.pathCount = 0;
        }

        // ATTACK!!!
        public void activateKillMode() 
        {
            killMode = true;
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

                if (preKillMode)
                {
                    // zombie's groove has been thrown off, alert leader
                    source.X = 128;
                    killMode = true;
                    preKillMode = false;
                    leaderZombie.activateKillMode();
                }
                else if(killMode)
                {
                    // just kill darwin
                    ZOMBIE_MOVE_RATE = 10;
                    this.enemyAlert = true;
                    source.X = 64;
                    moveTowardsPoint(darwin.X, darwin.Y);
                }
                else if (this.isPointInVision(darwin.X, darwin.Y) && !darwin.isZombie() && isDarwinOnFloor(darwin))
                {
                        // darwin is a human on the dance floow he must die
                        ZOMBIE_MOVE_RATE = 10;
                        this.enemyAlert = true;
                        source.X = 64;
                        moveTowardsPoint(darwin.X, darwin.Y);
                }
                else if ((isDarwinOnFloor(darwin) && !darwin.isZombie())
                    || (darwin.isZombie() && isDarwinOnFloor(darwin) && !isDarwinOnPath(darwin)))
                {
                        if (!hasBeenSeen)
                        {
                            // get ready to attack
                            ZOMBIE_MOVE_RATE = 20;
                            hasBeenSeenCounter++;
                            if (hasBeenSeenCounter > 3)
                                hasBeenSeen = true;
                            followPath();
                        }
                        else
                        {
                            // someone saw darwin, attack
                            ZOMBIE_MOVE_RATE = 10;
                            this.enemyAlert = true;
                            source.X = 64;
                            moveTowardsPoint(darwin.X, darwin.Y);
                        }

                 }
                else
                {
                        // no threat at the moment, just dance, watch out for alarm from other zombies
                        ZOMBIE_MOVE_RATE = 20;
                        hasBeenSeen = false;
                        hasBeenSeenCounter = 0;

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
