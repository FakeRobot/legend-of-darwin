﻿using System;
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




        public CannibalZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard):
            base(startX,startY,mymaxX,myminX, mymaxY, myminY, myboard)
            {
                allowRangeDetection = false;
                allowVision = true;
                visionMaxX = 5;
                visionMaxY = 5;

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
                    ;
            }


        }


        public new void Update(GameTime gameTime, Darwin darwin, Brain brain) 
        {
            eventLagMin++;
            if (eventLagMin > eventLagMax)
            {
                this.eventFlag = true;
            }

            if (this.isOnTop(darwin) && darwin.isZombie())
            {
                darwin.setAbsoluteDestination(2, 2);
            }

            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                if (isVisionAllowed() && isBrainInRange(brain))
                    moveTowardsBrain(brain, darwin);
                else if (isVisionAllowed() && isDarwinInRange(darwin) && darwin.isZombie())
                    moveTowardsDarwin(darwin);
                else
                    this.RandomWalk();


                movecounter = 0;
            }
            movecounter++;

        }

    }
}