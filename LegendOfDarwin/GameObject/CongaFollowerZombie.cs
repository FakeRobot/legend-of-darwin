using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    class CongaFollowerZombie : Zombie
    {
        // darwin that is on the current game board
        protected Darwin darwin;

        // array and index for designating pts at the vertices of the zombie's patrol path
        protected Vector2[] pathList;
        protected int pathCount = 0;

        //amt that sprite should be shifted up in order to look natural
        protected int amtShiftUp = 0;

        // set ranges to whole board
        public CongaFollowerZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, Vector2[] myPathList, Darwin mydarwin, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard) 
        {
            allowRangeDetection = false;
            allowVision = true;
            visionMaxX = 4;
            visionMaxY = 4;
            darwin = mydarwin;
            pathList = myPathList;
        }


    }
}
