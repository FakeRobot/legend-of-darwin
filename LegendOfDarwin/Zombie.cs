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
	
	    //zombie range, designates an area that the zombie can be in
		int maxX;
		int minX;
		int maxY;
		int minY;
		
		
	    /* constructor
		*  sets an initial area for the zombie to take up
		*  mymaxX, myminX are the max/min allowed horizontal range for the zombie
        *  mymaxY, myminY are the max/min allowed vertical range for the zombie		
		**/
        public Zombie(int mymaxX,int myminX,int mymaxY,int myminY)
        {
		  maxX = mymaxX;
		  maxY = mymaxY;
		  minX = myminX;
		  minY = myminY;
		}
		
		// Load the content
        public void LoadContent()
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

        // Update
        public void Update(GameTime gameTime)
        {

        }
		
		// Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
	
	}
	
}