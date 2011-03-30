using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    class BoxPattern
    {
        // the list of spots on the board that are a part of the pattern
        private BasicObject[] spots;

        // Optional: a texture can be printed on the board to help with debugging
        protected Texture2D SpotTexture;

        private int numberOfSpotsToCheck;

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

            //foreach (Box b in boxes)
            for (int j = 0; j < numberOfSpotsToCheck; j++)
            {
                for (int i = 0; i < numberOfSpotsToCheck; i++)
                {
                    if (boxes[j].X == spots[i].X && boxes[j].Y == spots[i].Y)
                    {
                        matchCount++;
                    }
                }
            }

            if (matchCount == numberOfSpotsToCheck)
            {
                return true;
            }

            return false;
        }

    }
}
