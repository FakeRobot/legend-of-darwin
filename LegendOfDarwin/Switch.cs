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
         * walls is an array basic objects with the positions on the grid that the switch will control
         **/
        public Switch(BasicObject switchSquare, GameBoard myboard, BasicObject[] walls) : base(myboard)
        {
            //switch inherits an X and Y from the basic object
            this.X = switchSquare.X;
            this.Y = switchSquare.Y;

            this.walls = walls;

            this.board = myboard;

            if (board.isGridPositionOpen(this.X, this.Y))
            {
                board.setGridPositionOccupied(this.X, this.Y);
            }
            else
            {
                // you are putting a switch on a block that is already occupied. That isn't good.
            }

            // initialize all of the walls associated with this switch to be occupied
            foreach (BasicObject bo in walls)
            {
                if (board.isGridPositionOpen(bo))
                {
                    board.setGridPositionOccupied(bo.X, bo.Y);
                }
                else
                {
                    // you are putting a wall on a block that is already occupied. That isn't good.
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
            spriteBatch.Draw(wallTex, board.getPosition(this.X, this.Y), switchSource, Color.White);
        }
    }
}
