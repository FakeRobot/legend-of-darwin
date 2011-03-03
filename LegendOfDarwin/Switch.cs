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
        // the textures to draw on the wall squares and the switch square
        Texture2D wallTex;
        Texture2D switchTex;

        // squares that will be in the wall
        BasicObject[] walls;

        // The frame or cell of the sprite to show
        private Rectangle switchSource;

        // The dimensions of the switch image in pixels
        private const int SWITCH_WIDTH = 250;
        private const int SWITCH_HEIGHT = 250;

        private bool isOn;

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

            if (board.isGridPositionOpen(this.X, this.Y))
            {
                board.setGridPositionOccupied(this.X, this.Y);
            }
            else
            {
                // you are putting a switch on a block that is already occupied. That isn't good.
                // Throw an error or something
            }

            turnOn();

        }

        // toggle the walls associated with this switch to be un-walkable
        public void turnOn()
        {
            // initialize all of the walls associated with this switch to be occupied
            foreach (BasicObject bo in walls)
            {
                if (board.isGridPositionOpen(bo))
                {
                    board.setGridPositionOccupied(bo);
                }
            }

            isOn = true;
        }

        // toggle the walls associated with this switch to be walkable
        public void turnOff()
        {
            foreach (BasicObject bo in walls)
            {
                if (!board.isGridPositionOpen(bo))
                {
                    board.setGridPositionOpen(bo);
                }
            }

            isOn = false;
        }

        public void LoadContent(Texture2D myWallTex, Texture2D mySwitchTex)
        {
            wallTex = myWallTex;
            switchTex = mySwitchTex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isOn)
            {
                foreach (BasicObject bo in walls)
                {
                    // Draw the walls visible
                    Rectangle source = new Rectangle(0, 0, board.getSquareLength(), board.getSquareLength());
                    spriteBatch.Draw(wallTex, board.getPosition(bo), source, Color.White);
                }

                // Draw the Switch in the on position
                switchSource = new Rectangle(0, 0, SWITCH_WIDTH, SWITCH_HEIGHT);
                spriteBatch.Draw(switchTex, board.getPosition(this.X, this.Y), switchSource, Color.White);
            }
            else
            {
                // Draw the switch in the off position
                switchSource = new Rectangle(SWITCH_WIDTH, 0, SWITCH_WIDTH, SWITCH_HEIGHT);
                spriteBatch.Draw(switchTex, board.getPosition(this.X, this.Y), switchSource, Color.White);
            }
        }

        public void Update(KeyboardState ks, Darwin darwin)
        {
            // if we be a zombie, we cant use switches
            if (darwin.isZombie())
            {
                // grab us the current
                LegendOfDarwin.Darwin.Dir facing = darwin.facing;

                // check switch position in relation to darwin's position + facing direction 
                switch (facing)
                {
                    case (LegendOfDarwin.Darwin.Dir.Left):
                        if (((this.X + 1) == darwin.X) && (this.Y == darwin.Y))
                            toggleSwitch(ks);
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Right):
                        if (((this.X - 1) == darwin.X) && (this.Y == darwin.Y))
                            toggleSwitch(ks);
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Up):
                        if ((this.X == darwin.X) && ((this.Y + 1) == darwin.Y))
                            toggleSwitch(ks);
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Down):
                        if ((this.X == darwin.X) && ((this.Y - 1) == darwin.Y))
                            toggleSwitch(ks);
                        break;
                }
            }
        }

        private void toggleSwitch(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.A))
            {
                if (isOn)
                {
                    turnOff();
                }
                else
                {
                    turnOn();
                }
            }
        }

    }
}
