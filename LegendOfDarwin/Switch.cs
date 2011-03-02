using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    class Switch : BasicObject
    {
        Texture2D wallTex;

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

        // Load the content
        public void LoadContent(Texture2D myWallTex)
        {
            wallTex = myWallTex;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //foreach (BasicObject bo in bos)
            //{
            //    spriteBatch.Draw(background[i, j], grid[i, j], Color.White);
            //}
        }
    }
}
