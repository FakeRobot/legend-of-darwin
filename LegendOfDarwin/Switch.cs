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
        // the texture to draw on the wall squares
        Texture2D wallTex;

        // squares that will be in the wall
        BasicObject[] bos;

        // The frame or cell of the sprite to show
        private Rectangle source;

        GameBoard board;

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

            this.bos = bos;

            this.board = myboard;

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
                if (board.isGridPositionOpen(bo))
                {
                    //if its free it will be set by the function call in the if
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
            foreach (BasicObject bo in bos)
            {
                source = new Rectangle(0, 0, board.getSquareLength(), board.getSquareLength());
                spriteBatch.Draw(wallTex, board.getPosition(bo), source, Color.White);
            }
        }
    }
}
