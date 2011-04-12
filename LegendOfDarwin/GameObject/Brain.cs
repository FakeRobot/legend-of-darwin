using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace LegendOfDarwin.GameObject
{

    // brain that can be pushed around by zombie darwin
    public class Brain: BasicObject
    {
        public const int BRAIN_HEIGHT = 64;
        public const int BRAIN_WIDTH = 64;

        protected Texture2D brainTexture;

        // takes in board and starting positions on initialization
        public Brain(GameBoard myboard, int startX, int startY) : base(myboard)
        {
            this.X = startX;
            this.Y = startY;

            this.setGridPosition(startX, startY);

            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOccupied(this.X, this.Y);
            }

            destination = board.getPosition(startX, startY);
        }

        public void LoadContent(Texture2D brainTex)
        {
            brainTexture = brainTex;
        }

        public void Update(GameTime gameTime, KeyboardState ks, Darwin darwin)
        {
            base.Update(gameTime);
            // Darwin only pushes brains as a zombie
            if (darwin.isZombie() && this.canEventHappen() && ks.IsKeyDown(Keys.A))
            {
                this.setEventFalse();

                // grab us the current
                LegendOfDarwin.Darwin.Dir facing = darwin.facing;

                // check switch position in relation to darwin's position + facing direction 
                switch (facing)
                {
                    case (LegendOfDarwin.Darwin.Dir.Left):
                        if (((this.X + 1) == darwin.X) && (this.Y == darwin.Y))
                        {
                            if (board.isGridPositionOpen(this.X - 1, this.Y))
                            {
                                this.MoveLeft();
                            }
                        }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Right):
                        if (((this.X - 1) == darwin.X) && (this.Y == darwin.Y))
                            if (board.isGridPositionOpen(this.X + 1, this.Y))
                            {
                                this.MoveRight();
                            }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Up):
                        if ((this.X == darwin.X) && ((this.Y + 1) == darwin.Y))
                            if (board.isGridPositionOpen(this.X, this.Y - 1))
                            {
                                this.MoveUp();
                            }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Down):
                        if ((this.X == darwin.X) && ((this.Y - 1) == darwin.Y))
                            if (board.isGridPositionOpen(this.X, this.Y + 1))
                            {
                                this.MoveDown();
                            }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                spriteBatch.Draw(brainTexture, this.destination, Color.White);
            }
        }

    }
}
