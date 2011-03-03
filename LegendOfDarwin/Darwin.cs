using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    class Darwin : BasicObject
    {
        GraphicsDevice graphics;

        // an enum type for the direction 
        public enum Dir { Up, Down, Left, Right };

        //Darwin's height in pixels
        public const int DARWIN_HEIGHT = 64;
        //Darwin's width in pixels
        public const int DARWIN_WIDTH = 64;


        //Which direction is Darwin facing
        public Dir facing = Dir.Down;

        //flag to show whether darwin is a zombie or not
        private bool zombieFlag;

        //start darwin's humanity at 100
        public int humanityLevel = 100;

        // The frame or cell of the sprite to show
        private Rectangle source;

        // The location to draw the sprite on the screen.
        //public Rectangle destination;

        public bool collision = false;


        // The current position of Darwin on the floor
        // Might have to be changed to coordinates depending on the floor layout
        public Vector2 position = Vector2.Zero;


        // Textures 
        Texture2D darwinUpTex;
        Texture2D darwinDownTex;
        Texture2D darwinRightTex;
        Texture2D darwinLeftTex;
        Texture2D zombieDarwinTex;

        //BasicObject potentialGridPosition;

        //constructor
        public Darwin(GameBoard myboard) : base(myboard)
        {
            // Init the frame or cell of the animation that will be shown. 
            source = new Rectangle();
            source.Width = DARWIN_HEIGHT;
            source.Height = DARWIN_WIDTH;
            source.X = 0;
            source.Y = 0;

            //potentialGridPosition.setGridPosition(5, 5);

            board = myboard;

            this.setGridPosition(5, 5);
        }

        public void setSource(Rectangle rec)
        {
            source.Width = rec.Width;
            source.Height = rec.Height;
            source.X = rec.X;
            source.Y = rec.Y;
        }

        public void setPictureSize(int width, int height)
        {
            destination.Width = width;
            destination.Height = height;
        }

        /*
        public void setPosition(int startX, int startY)
        {
            // Set the initial position of Darwin
            position.X = (float)startX;
            position.Y = (float)startY;

            // Update the destination
            destination = new Rectangle();
            destination.Height = DARWIN_HEIGHT;
            destination.Width = DARWIN_WIDTH;
            destination.X = (int)Math.Round(position.X);
            destination.Y = (int)Math.Round(position.Y);
        }
        */

        public void LoadContent(GraphicsDevice newGraphics, Texture2D humanUp, Texture2D humanDown, Texture2D humanRight, Texture2D humanLeft, Texture2D zombieTex)
        {
            graphics = newGraphics;
            darwinUpTex = humanUp;
            darwinDownTex = humanDown;
            darwinRightTex = humanRight;
            darwinLeftTex = humanLeft;
            zombieDarwinTex = zombieTex;
        }

        public void Update(GameTime gameTime, KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            base.Update(gameTime);
            if (this.canEventHappen())
            {
                updateDarwinTransformState(ks);
                this.setEventFalse();
            }

            moveDarwin(ks, board, currentDarwinX, currentDarwinY);
            setPictureSize(board.getSquareWidth(), board.getSquareLength());

        }

        private void moveDarwin(KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            // has a direction been picked to go in, used to get rid of diagonal movement
            bool hasPicked = false;

            if (ks.IsKeyDown(Keys.Right))
            {
                facing = Dir.Right;
                if (board.isGridPositionOpen(currentDarwinX + 1, currentDarwinY) && !hasPicked)
                {
                    hasPicked = true;
                    this.MoveRight();
                }
                /*else
                {
                    collision = true;
                }*/
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                facing = Dir.Left;
                if (board.isGridPositionOpen(currentDarwinX - 1, currentDarwinY) && !hasPicked)
                {
                    hasPicked = true;
                    this.MoveLeft();
                }
                /*else
                {
                    collision = true;
                }*/
                
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                facing = Dir.Up;
                if (board.isGridPositionOpen(currentDarwinX, currentDarwinY - 1) && !hasPicked)
                {
                    hasPicked = true;
                    this.MoveUp();
                }
                /*else
                {
                    collision = true;
                }*/
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                facing = Dir.Down;
                if(board.isGridPositionOpen(currentDarwinX, currentDarwinY + 1) && !hasPicked)
                {
                    hasPicked = true;
                    this.MoveDown();
                }
                /*else
                {
                    collision = true;
                }*/
            }
        }

        private void updateDarwinTransformState(KeyboardState ks)
        {
            
            if (ks.IsKeyDown(Keys.Z))
            {
                if (zombieFlag == true)
                {
                    zombieFlag = false;
                }
                else
                {
                    zombieFlag = true;
                }
            }
        }

        // Check if Darwin is intersecting something
        public bool Intersects(/*Zombie enemy*/)
        {
            /*
            if Darwin intersects with a zombie
                if he's human
                    return true
                else
                    return false

             otherwise if there is no intersection
                return false
            */
             
            return false;
        }

        // Collisions
        public bool Collision(/*Zombie enemy*/)
        {
            /*
            if (Intersects(enemy))
            {
                Collision Logic
                return true;
            }
            else
            {
                return false;
            }*/
            return false;
        }

        public bool isZombie()
        {
            return this.zombieFlag;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (zombieFlag == false)
            {
                if (facing == Dir.Up)
                {
                    spriteBatch.Draw(darwinUpTex, destination, source, Color.White);
                }
                else if (facing == Dir.Down)
                {
                    spriteBatch.Draw(darwinDownTex, destination, source, Color.White);
                }
                else if (facing == Dir.Left)
                {
                    spriteBatch.Draw(darwinLeftTex, destination, source, Color.White);
                }
                else
                {
                    spriteBatch.Draw(darwinRightTex, destination, source, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(zombieDarwinTex, destination, source, Color.White);
            }
        }
    }
}
