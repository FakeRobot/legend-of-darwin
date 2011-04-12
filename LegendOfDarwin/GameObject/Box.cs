using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;


namespace LegendOfDarwin.GameObject
{

    // Boxes to be pushed around by darwin
    public class Box : BasicObject
    {
        public const int BOX_HEIGHT = 64;
        public const int BOX_WIDTH = 64;

        protected Texture2D BoxTexture;

        public SoundEffect boxSound;

        // sets a new box on the game board
        // takes in board, and a starting position
        public Box(GameBoard myboard, int startX, int startY) : base(myboard)
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

        public void LoadContent(Texture2D BoxTex, SoundEffect bSound)
        {
            BoxTexture = BoxTex;
            boxSound = bSound;
        }

        public void Update(GameTime gameTime, KeyboardState ks, Darwin darwin)
        {
            base.Update(gameTime);
            // if darwin is a zombie, he cant push boxes
            if (!darwin.isZombie() && this.canEventHappen() && ks.IsKeyDown(Keys.A))
            {
                this.setEventFalse();

                // get Darwin's current facing direction
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
                                boxSound.Play();
                            }
                        }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Right):
                        if (((this.X - 1) == darwin.X) && (this.Y == darwin.Y))
                            if (board.isGridPositionOpen(this.X + 1, this.Y))
                            {
                                this.MoveRight();
                                boxSound.Play();
                            }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Up):
                        if ((this.X == darwin.X) && ((this.Y + 1) == darwin.Y))
                            if (board.isGridPositionOpen(this.X, this.Y - 1))
                            {
                                this.MoveUp();
                                boxSound.Play();
                            }
                        break;
                    case (LegendOfDarwin.Darwin.Dir.Down):
                        if ((this.X == darwin.X) && ((this.Y - 1) == darwin.Y))
                            if (board.isGridPositionOpen(this.X, this.Y + 1))
                            {
                                this.MoveDown();
                                boxSound.Play();
                            }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                spriteBatch.Draw(BoxTexture, this.destination, Color.White);
            }
        }

    }
}
