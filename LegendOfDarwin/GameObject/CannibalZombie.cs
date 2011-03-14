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

        public void Update(GameTime gameTime, Darwin darwin, Brain brain) 
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
