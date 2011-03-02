using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class BasicObject
    {
        // The x coordinate to be put on the GameBoard class
        public int X;
        // The y coordinate to be put on the GameBoard class
        public int Y;

        public BasicObject(int x, int y)
        {
            setGridPosition(x, y);
        }

        // Set their positions
        public void setGridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }


    }
}
