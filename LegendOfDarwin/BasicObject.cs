using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class BasicObject
    {
        public Rectangle source;
        public int X;
        public int Y;
        
        public BasicObject(Rectangle sourceIn)
        {
            source = new Rectangle(sourceIn.X, sourceIn.Y, sourceIn.Width, sourceIn.Height);
        }

        public void setGridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
