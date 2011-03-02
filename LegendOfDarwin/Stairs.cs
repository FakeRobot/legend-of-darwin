using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{

    class Stairs : BasicObject
    {
        private Texture2D stairTex;

        private Rectangle destination;

        public void LoadContent(Texture2D content)
        {
            stairTex = content;
        }

        public void setDestination(Rectangle rectangle)
        {
            destination = rectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(stairTex, destination, Color.White);
        }
    }
}
