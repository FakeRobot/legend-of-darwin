using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    class Zombie : BasicObject
    {
        // Zombie's height in pixels
        public const int ZOMBIE_HEIGHT = 64;
        // Zombie's width in pixels
        public const int ZOMBIE_WIDTH = 64;

        // The frame or cell of the sprite to show
        protected Rectangle source;

        // The location to draw the sprite on the screen.
        protected Rectangle destination;

        protected Texture2D zombieTexture;

        //zombie range, designates an area that the zombie can be in, done in game board tiles
        protected int maxX;
        protected int minX;
        protected int maxY;
        protected int minY;

        // zombie vision range, optional, does not have to be used
        protected bool allowVision;

        // max horizontal distance from zombie that darwin can be seen at
        // vision acts like a square centered at the zombie
        protected int visionMaxX;
        protected int visionMaxY;

        // sets whether zombie will go after Darwin if he is in the zombie's movement range at all
        protected bool allowRangeDetection;

        // the board that the zombie is moving on
        protected GameBoard board;

        // counter for pacing zombie's movements
        public int movecounter=0;

        //rate that the zombie moves at, increase to slow down, decrease to speed up
        public const int ZOMBIE_MOVE_RATE=50;

        /* constructor
        *  sets an initial area for the zombie to take up
        *  mymaxX, myminX are the max/min allowed horizontal range for the zombie
        *  mymaxY, myminY are the max/min allowed vertical range for the zombie
        *  Gameboard myboard -- the board which the zombie is moving on
        **/
        public Zombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard)
        {
            maxX = mymaxX;
            maxY = mymaxY;
            minX = myminX;
            minY = myminY;

            // start with no zombie vision
            allowVision = false;
            visionMaxX = 0;
            visionMaxY = 0;

            allowRangeDetection = true;
            board = myboard;

            this.X = startX;
            this.Y = startY;

            board.isGridPositionOpen(this);

            destination = new Rectangle(0, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT);
            this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            source = new Rectangle(0, 0, ZOMBIE_WIDTH, ZOMBIE_HEIGHT);
        }

        // Load the content
        public void LoadContent(Texture2D myZombieTexture)
        {
            zombieTexture = myZombieTexture;
        }

        /**
         * sets the position of the zombie
         * int myX, int myY screen positions of the zombie
         * */
        public void setPosition(int myX, int myY)
        {

            // Update the destination
            //destination.Height = ZOMBIE_HEIGHT;
            //destination.Width = ZOMBIE_WIDTH;
            destination.X = myX;
            destination.Y = myY;
        }

        public void setPictureSize(int width, int height)
        {
            destination.Width = width;
            destination.Height = height;
        }

        /*
         * moves the zombie to the right of current position on the game board
         * */
        public void MoveRight()
        {
            this.setGridPosition(this.X + 1, this.Y);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X - 1, this.Y);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X - 1, this.Y);
            }
        }

        /*
         * moves the zombie to the left of current position on the game board
         * */
        public void MoveLeft()
        {
            this.setGridPosition(this.X - 1, this.Y);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X + 1, this.Y);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X + 1, this.Y);
            }
        }

        /*
         * moves the zombie down one square on the game board
         * */
        public void MoveDown()
        {
            this.setGridPosition(this.X, this.Y + 1);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X, this.Y - 1);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X, this.Y - 1);
            }
        }

        /*
         * moves the zombie one up on the game board
         * */
        public void MoveUp()
        {
            this.setGridPosition(this.X, this.Y - 1);
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOpen(this.X, this.Y + 1);
                this.setPosition(board.getPosition(this).X, board.getPosition(this).Y);
            }
            else
            {
                this.setGridPosition(this.X, this.Y + 1);
            }
        }

        /*
        *  makes the zombie walk randomly within its specified range 
        *  does not allow zombie to leave range
        **/
        public void RandomWalk()
        {
            Random rand = new Random();
            int direction = 0;
            bool hasPicked = false; // has a direction been chosen

            while (!(hasPicked))
            {
                direction = rand.Next(4);
                if (direction == 0)
                {
                    //move down
                    if (isZombieInRange(this.X, this.Y + 1))
                    {
                        MoveDown();
                        hasPicked = true;
                    }
                }
                else if (direction == 1)
                {
                    //move up
                    if (isZombieInRange(this.X, this.Y - 1))
                    {
                        MoveUp();
                        hasPicked = true;
                    }
                }
                else if (direction == 2)
                {
                    //move left
                    if (isZombieInRange(this.X - 1, this.Y))
                    {
                        MoveLeft();
                        hasPicked = true;
                    }
                }
                else if (direction == 3)
                {
                    //move right
                    if (isZombieInRange(this.X + 1, this.Y))
                    {
                        MoveRight();
                        hasPicked = true;
                    }
                }
            }

        }

        /**
         * moves zombie in the direction that darwin currently is
         * */
        public void moveTowardsDarwin(Darwin darwin)
        {

            int changeX = 0;
            int changeY = 0;

            changeX = darwin.X - this.X;
            changeY = darwin.Y - this.Y;

            if (Math.Abs(changeX) > Math.Abs(changeY))
            {
                //move in x direction
                if (darwin.X > this.X)
                {
                    //move right
                    if (isZombieInRange(this.X + 1, this.Y))
                    {
                        MoveRight();
                    }
                }
                else if (darwin.X < this.X)
                {
                    //move left
                    if (isZombieInRange(this.X - 1, this.Y))
                    {
                        MoveLeft();
                    }
                }
            }
            else 
            {
                //move in y direction
                if (darwin.Y > this.Y)
                {
                    //move down
                    if (isZombieInRange(this.X, this.Y + 1))
                    {
                        MoveDown();
                    }
                }
                else if (darwin.Y < this.Y)
                {
                    //move down
                    if (isZombieInRange(this.X, this.Y - 1))
                    {
                        MoveUp();
                    }
                }
            }

        }

        /**
         *  checks whether zombie is in range or not
         *  return True if it is, false if not
         */
        public bool isZombieInRange()
        {
            if (this.X <= maxX && this.X >= minX && this.Y >= minY && this.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         *  checks whether zombie is in range or not
         *  return True if it is, false if not
         *  myx - current x position, myy - current y position
         */
        public bool isZombieInRange(int myx, int myy)
        {
            if (myx <= maxX && myx >= minX && myy >= minY && myy <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         *  checks whether or not darwin is in the zombies range
         *  returns true if he is, false otherwise
         */
        public bool isDarwinInRange(Darwin darwin)
        {
            if (darwin.X <= maxX && darwin.X >= minX && darwin.Y >= minY && darwin.Y <= maxY)
                return true;
            else
                return false;
        }

        /**
         * is vision enabled for zombie
         * return true if so, false otherwise
         * */
        public bool isVisionAllowed()
        {
            return allowVision;
        }

        /**
         * sets the allowance of vision on the zombie
         * Vision determines if the zombie can see darwin if he comes into a specified range
         * vision range defaults to zero
         * myVision - true to enable vision, false to disable it
         * */
        public void setVisionAllowed(bool myVision)
        {
            allowVision = myVision;
        }

        /*
         * sets vision range to a designated range
         * range is centered at zombie
         * int mymaxX - max x distance zombie can see
         * int mymaxY - max y distance zombie can see
         * */
        public void setVisionRange(int mymaxX, int mymaxY)
        {
            if (mymaxX >= 0 && mymaxY >= 0)
            {
                visionMaxX = mymaxX;
                visionMaxY = mymaxY;
            }
            else
            {
                visionMaxX = 0;
                visionMaxY = 0;
            }
        }

        /*
         * tells whether zombie can detect Darwin if he is in the zombie's movement range
         * */
        public bool isRangeDetectionAllowed()
        {
            return allowRangeDetection;
        }

        /*
         * sets whether range detection is allowed or not
         * that is, will the zombie go after darwin if he is in the zombie's movement range
         * */
        public void setIsRangeDetectionAllowed(bool myAllow)
        {
            allowRangeDetection = myAllow;
        }

        public void testRun()
        {
            if (movecounter < 300 && (movecounter % 50) == 0)
            {
                this.MoveRight();
            }
            else if (movecounter < 600 && movecounter > 300 && (movecounter % 50) == 0)
            {
                this.MoveDown();
            }
            else if (movecounter > 600 && movecounter < 900 && (movecounter % 50) == 0)
            {
                this.MoveLeft();
            }
            else if (movecounter < 1200 && movecounter > 900 && (movecounter % 50) == 0)
            {
                this.MoveUp();
            }

            if (movecounter < 1200)
                movecounter++;
        }

        // Update
        public void Update(GameTime gameTime,Darwin darwin)
        {
            //testRun();

            
            //Random rand1 = new Random();     
            //Random rand2 = new Random();

            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                if (isRangeDetectionAllowed() && isDarwinInRange(darwin))
                    moveTowardsDarwin(darwin);
                else
                    this.RandomWalk();


                movecounter = 0;
            }
            movecounter++;

        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(zombieTexture, this.destination, this.source, Color.White);
        }

    }

}