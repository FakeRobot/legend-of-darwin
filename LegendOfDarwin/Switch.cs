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
        BasicObject[] walls;

        // The frame or cell of the sprite to show
        private Rectangle switchSource;

        GameBoard board;

        /* posX is the X coordinate of the switch
         * posy is the Y coordinate of the switch
         * myboard is the gameboard
         * the bos array is an array of positions that the switch changed from occupied to open when switched
         **/
        public Switch(BasicObject switchSquare, GameBoard myboard, BasicObject[] walls) : base(myboard)
        {
            //switch inherits an X and Y from the basic object
            this.X = switchSquare.X;
            this.Y = switchSquare.Y;

            this.walls = walls;

            this.board = myboard;

            // initialize the square that the switch is on to be occupied
            if (board.isGridPositionOpen(this.X, this.Y))
            {
                board.setGridPositionOccupied(this.X, this.Y);
            }

            // initialize all of the walls associated with this switch to be occupied
            foreach (BasicObject bo in walls)
            {
                // if it is open, fill it.
                if (board.isGridPositionOpen(bo))
                {
                    board.setGridPositionOccupied(this.X, this.Y);
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
            foreach (BasicObject bo in walls)
            {
                Rectangle source = new Rectangle(0, 0, board.getSquareLength(), board.getSquareLength());
                spriteBatch.Draw(wallTex, board.getPosition(bo), source, Color.White);
            }
            
            switchSource = new Rectangle(0, 0, board.getSquareLength(), board.getSquareLength());

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.setPosition(this.X, this.Y);

            spriteBatch.Draw(wallTex, board.getPosition(switchSquare), switchSource, Color.White);
        }
    }
}
