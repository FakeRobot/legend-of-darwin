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
        public static bool isZombie;

        //start darwin's humanity at 100
        public int humanityLevel = 100;

        // The frame or cell of the sprite to show
        private Rectangle source;

        // The location to draw the sprite on the screen.
        //public Rectangle destination;


        // The current position of Darwin on the floor
        // Might have to be changed to coordinates depending on the floor layout
        public Vector2 position = Vector2.Zero;


        // Textures 
        Texture2D darwinTex;
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

        public void LoadContent(GraphicsDevice newGraphics, Texture2D humanTex, Texture2D zombieTex)
        {
            graphics = newGraphics;
            darwinTex = humanTex;
            zombieDarwinTex = zombieTex;
        }

        public void Update(GameTime gameTime, KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            updateDarwinTransformState(ks);
            moveDarwin(ks, board, currentDarwinX, currentDarwinY);
            setPictureSize(board.getSquareWidth(), board.getSquareLength());

        }

        private void moveDarwin(KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                if (board.isGridPositionOpen(currentDarwinX + 1, currentDarwinY))
                {
                    this.MoveRight();
                }
                else
                {
                    //its a zombie or a wall
                }
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                if(board.isGridPositionOpen(currentDarwinX -1, currentDarwinY))
                {
                    this.MoveLeft();
                }
                else
                {
                    //its a zombie or a wall
                }
                
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                if(board.isGridPositionOpen(currentDarwinX, currentDarwinY - 1)){
                    this.MoveUp();
                }
                else
                {
                    //its a zombie or a wall
                }
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                if(board.isGridPositionOpen(currentDarwinX, currentDarwinY + 1))
                {
                    this.MoveDown();
                }
                else
                {
                    //its a zombie or a wall
                }
            }
        }

        private void updateDarwinTransformState(KeyboardState ks)
        {
            //need to add flag - buggy
            if (ks.IsKeyDown(Keys.Z))
            {
                if (isZombie == true)
                {
                    isZombie = false;
                }
                else
                {
                    isZombie = true;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isZombie == false)
            {
                spriteBatch.Draw(darwinTex, destination, source, Color.White);
            }
            else
            {
                spriteBatch.Draw(zombieDarwinTex, destination, source, Color.White);
            }
        }
    }
}
