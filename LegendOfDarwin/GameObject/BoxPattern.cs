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

        public BoxPattern(BasicObject[] mySpots)
        {
            int i = 0;
            foreach (BasicObject s in mySpots)
            {
                spots[i] = s;
                i++;
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
            // copy the box positions (basic objects) into an array
            BasicObject[] boxSpots;
            int i = 0;

            foreach (Box b in boxes)
            {
                //boxSpots[i] = b;
                //boxSpots[i].setPosition(b.X, b.Y);
                i++;
            }

            return false;
        }

    }
}
