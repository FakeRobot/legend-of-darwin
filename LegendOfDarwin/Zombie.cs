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
	
	    //zombie range, designates an area that the zombie can be in
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

        public int testcounter;
	    
        /* constructor
		*  sets an initial area for the zombie to take up
		*  mymaxX, myminX are the max/min allowed horizontal range for the zombie
        *  mymaxY, myminY are the max/min allowed vertical range for the zombie
        *  Gameboard myboard -- the board which the zombie is moving on
		**/
        public Zombie(int startX,int startY,int mymaxX,int myminX,int mymaxY,int myminY,GameBoard myboard)
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
         * moves the zombie on position to the right on the game board
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

        public void MoveLeft()
        {
        }

        public void MoveDown()
        {
        }

        public void MoveUp()
        {
        }

		/*
		*  makes the zombie walk randomly within its specified range 
		*  does not allow zombie to leave range
		**/
		public void RandomWalk()
		{
            Random rand = new Random();
            int direction = rand.Next(4);

            Console.Write("{0} ",direction);
            if (direction == 0)
            {
                //move down
            }
            else if (direction==1)
            {
                //move up
            }
            else if (direction == 2)
            {
                //move left
            }
            else if (direction == 3)
            {
                //move right
            }
		
		}
		
		public void moveTowardsDarwin(Darwin darwin)
		{
            
		
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
			if (darwin.X<=maxX && darwin.X>=minX && darwin.Y>=minY && darwin.Y<=maxY)
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

        // Update
        public void Update(GameTime gameTime)
        {
            if (testcounter<300 && (testcounter % 50)==0)
            {
                this.MoveRight();
                
            }

            if (testcounter<300)
              testcounter++;

        }
		
		// Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(zombieTexture, this.destination, this.source, Color.White);
        }
	
	}
	
}