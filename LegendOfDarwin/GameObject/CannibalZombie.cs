using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LegendOfDarwin.GameObject
{
    /**
     * Cannibal's eat other zombies
     * attack/seek out zombie Darwin or other zombies when they get in a certain range, other wise just shift around
     */
    public class CannibalZombie : Zombie
    {

        //flag for if zombie is in 'bug2' mode
        protected bool goAroundMode;

        // more overhead for nieve path planning
        protected Vector2[] path;
        protected int pathCount;
        protected int pathLimit;

        protected Darwin darwin;

        // list of zombies on same stage as cannibal
        protected List<Zombie> zombies;
        public SoundEffect eatSound;

        /**
         * Initalizes a cannibal on the game board
         * 
         * Arguments
         * int startX, startY -- start position of cannibal
         * int mymaxX, mymaxY, myminX, myminY -- range that cannibal can walk around in
         * List myListZombies -- list of the other non-cannibal zombies on the board
         * Darwin mydarwin -- reference to Darwin
         * Gameboard myboard -- board that cannibal is to be played on
         */
        public CannibalZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY,List<Zombie> myListZombies,Darwin mydarwin,GameBoard myboard):
            base(startX,startY,mymaxX,myminX, mymaxY, myminY, myboard)
            {
                allowRangeDetection = false;
                allowVision = true;
                goAroundMode = false;
                visionMaxX = 6;
                visionMaxY = 6;
                pathCount = 0;
                pathLimit = 0;
                darwin = mydarwin;
                zombies = myListZombies;
                ZOMBIE_MOVE_RATE=40;
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
                    else if (intendedPathY == this.Y - 1)
                        MoveUp();
                }
                else
                {
                    //attempt to find way around obstacle
                    //FindWhichWay(intendedPathX, intendedPathY);

                    //if (!goAroundMode)
                    //{
                        RandomWalk();
                    //}
                    //else
                    //   goAroundObstacle();
                    
                
                }
            }
        }

        // Used to help cannibal shift around small obstalces
        public void FindWhichWay(int intendedptX, int intendedptY) 
        { 
            Vector2 firstMove = new Vector2();
            Vector2 secondMove = new Vector2();

            // there is an obstacle either up or down so we should check left or right, check left first by default
            if (intendedptX==this.X && (intendedptY==this.Y-1 || intendedptY==this.Y+1))
            {
                firstMove.X = this.X-1;
                firstMove.Y = this.Y;

                secondMove.X = this.X-1;
                secondMove.Y = intendedptY;
                
            }
            else 
            {
                //assume obstacle is to left or right, so check up or down, down first by default
                firstMove.X = this.X;
                firstMove.Y = this.Y+1;

                secondMove.X = intendedptX;
                secondMove.Y = this.Y+1;
            }


            if (!(canMoveToPoint(firstMove) && canMoveToPoint(secondMove))) 
            {
                // there is an obstacle either up or down so we should check left or right, check right second by default
                if (intendedptX == this.X && (intendedptY == this.Y - 1 || intendedptY == this.Y + 1))
                {
                    firstMove.X = this.X + 1;
                    firstMove.Y = this.Y;

                    secondMove.X = this.X + 1;
                    secondMove.Y = intendedptY;

                }
                else
                {
                    //assume obstacle is to left or right, so check up or down, up second by default
                    firstMove.X = this.X;
                    firstMove.Y = this.Y - 1;

                    secondMove.X = intendedptX;
                    secondMove.Y = this.Y - 1;
                }

            }

            if (canMoveToPoint(firstMove) && canMoveToPoint(secondMove)) 
            {
                //implement some sort of bug2 here
                goAroundMode = true;
                pathCount = 0;
                
                Vector2[] newPath = new Vector2[2];
                newPath[0] = firstMove;
                newPath[1] = secondMove;
                path = newPath;
                pathLimit = 2; 
 
            }
            else
                    goAroundMode = false;
               
        }

        /*
         * checks if given point is either free, has a zombie, or has zombie darwin
         * */
        public bool canMoveToPoint(Vector2 movePt) 
        {
            bool canMove = false;

            foreach (Zombie zombieToEat in zombies)
            {
                if (zombieToEat.X == movePt.X && zombieToEat.Y == movePt.Y)
                {
                    canMove = true;
                }

            }

            if ((darwin.X == movePt.X && darwin.Y == movePt.Y && darwin.isZombie())
                || board.isGridPositionOpen((int)movePt.X, (int)movePt.Y))
                canMove = true;

            return canMove;
        }

        // like normal zombie's random walk but happens less frequently
        new public void RandomWalk()
        {
            Random rand1 = new Random();

            if (rand1.Next(7) == 2)
                base.RandomWalk();

        }

        // checks if a zombie is one square away or not
        public bool isZombieOneSquareAway(Zombie someZombie)
        {
            return (someZombie.X + 1 == this.X || someZombie.X - 1 == this.X || someZombie.Y + 1 == this.Y || someZombie.Y - 1 == this.Y);

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
                    if (pathCount >= pathLimit)
                        goAroundMode = false;
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

        // used to kill other zombies
        public void CollisionWithZombie(Zombie zombie)
        {
            if (this.isOnTop(zombie) && zombie.isZombieAlive())
            {
                zombie.setZombieAlive(false);
                eatSound.Play();
            }
        }

        // takes in list of zombies, returns new list with dead zombies removed
        public List<Zombie> removeDeadZombies(List<Zombie> myZList) 
        {
            List<Zombie> newList = new List<Zombie>();

            foreach (Zombie deadZombie in myZList) 
            {
                if (deadZombie.isZombieAlive())
                    newList.Add(deadZombie);
            }

            return newList;
        }
        
        public new void LoadContent(Texture2D fastZombieTexture, SoundEffect eSound)
        {
            zombieTexture = fastZombieTexture;
            eatSound = eSound;
        }

        public new void Update(GameTime gameTime, Darwin darwin) 
        {
            eventLagMin++;
            if (eventLagMin > eventLagMax)
            {
                this.eventFlag = true;
            }
            foreach (Zombie myzombie in zombies)
                CollisionWithZombie(myzombie);

            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                
                //these loops are seperate because the top loop could potentially remove zombies from the list
                this.updateListOfZombies(this.removeDeadZombies(zombies));

                int intendedptX = 0;
                int intendedptY = 0;
                bool hasZombieDest = false;
                bool goForDarwin = true;
                int closestZombieDist = 10000;

                // checks where other zombies are, and where zombie darwin is, finds closest one that is in range
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
                    if (Math.Abs(this.X - darwin.X) + Math.Abs(this.Y - darwin.Y) <= closestZombieDist && darwin.isZombie())
                    {
                        goForDarwin = true;
                        hasZombieDest = false;
                    }
                }

                if (goAroundMode)
                    goAroundObstacle();
                else if (isVisionAllowed() && isPointInVision(darwin.X, darwin.Y) && darwin.isZombie() && goForDarwin)
                {
                    // either go for darwin or a zombie, use exclamation mark
                    source.X = 64;
                    this.enemyAlert = true;
                    this.moveTowardsPoint(darwin.X, darwin.Y);
                }
                else if (hasZombieDest)
                {
                    source.X = 64;
                    this.enemyAlert = true;
                    this.moveTowardsPoint(intendedptX, intendedptY);
                }
                else
                {
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

                    this.RandomWalk();
                }

                movecounter = 0;
            }
            movecounter++;

        }

    }
}
