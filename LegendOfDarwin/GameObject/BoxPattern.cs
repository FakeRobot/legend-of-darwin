using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    // used to detect if boxes in a level comply to a set pattern
    // used to tell if player has completed a puzzle
    class BoxPattern
    {
        // the list of spots on the board that are a part of the pattern
        private BasicObject[] spots;

        // Optional: a texture can be printed on the board to help with debugging
        protected Texture2D SpotTexture;

        private int numberOfSpotsToCheck;

        // for sparkle animation
        bool showSparkles;
        int sparkleCount = 0;

        // takes in a board and an array of basic objects representing squares that are part of the pattern
        public BoxPattern(GameBoard board, BasicObject[] mySpots)
        {
            int i = 0;
            foreach (BasicObject s in mySpots)
            {
                i++;
            }

            numberOfSpotsToCheck = i;
            spots = new BasicObject[numberOfSpotsToCheck];

            for (i = 0; i < numberOfSpotsToCheck; i++)
            {
                this.spots[i] = new BasicObject(board);
                this.spots[i].setGridPosition(mySpots[i].X, mySpots[i].Y);
            }
        }

        public void LoadContent(Texture2D SpotTex)
        {
            SpotTexture = SpotTex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BasicObject s in spots)
            {
                spriteBatch.Draw(SpotTexture, s.destination, Color.White);
            }
        }

        // check the list of boxes to see if they are on every spot in the pattern
        public bool isComplete(GameBoard board, Box[] boxes)
        {
            // store the number of boxes we match
            int matchCount = 0;

            foreach (Box b in boxes)
            //for (int j = 0; j < numberOfSpotsToCheck; j++)
            {
                for (int i = 0; i < numberOfSpotsToCheck; i++)
                {
                    //if (boxes[j].X == spots[i].X && boxes[j].Y == spots[i].Y)
                    if (b.X == spots[i].X && b.Y == spots[i].Y)
                    {
                        matchCount++;
                    }
                }
            }

            if (matchCount == numberOfSpotsToCheck)
            {
                if (sparkleCount < 75)
                {
                    showSparkles = true;
                    sparkleCount++;
                }
                else if (sparkleCount >= 75)
                {
                    showSparkles = false;
                }
                return true;
            }
            else
            {
                showSparkles = false;
                sparkleCount = 0;
            }
            return false;
        }

        // accessors for managing the sparkle animation
        public bool shouldSparkle()
        {
            return showSparkles;
        }

        public int getSparkleCount()
        {
            return sparkleCount;
        }

    }
}
