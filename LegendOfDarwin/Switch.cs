using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfDarwin
{
    class Switch : BasicObject
    {
        Texture2D WallTex;

        /* posX is the X coordinate of the switch
         * posy is the Y coordinate of the switch
         * myboard is the gameboard
         * the bos array is an array of positions that the switch changed from occupied to open when switched
         **/
        public Switch(int posX, int posY, GameBoard myboard, BasicObject[] bos)
        {
            //switch inherits an X and Y from the basic object
            this.X = posX;
            this.Y = posY;

            // check that you aren't putting a switch on a spot that is already taken
            /*
            if (!myboard.isGridPositionOpen(this))
            {
            }
            */

            // initialize all 
            foreach (BasicObject bo in bos)
            {
                // if it is open, fill it.
                if (myboard.isGridPositionOpen(bo))
                {
                    bo.setGridPosition(bo.X, bo.Y);
                }


            }



            
        }
    }
}
