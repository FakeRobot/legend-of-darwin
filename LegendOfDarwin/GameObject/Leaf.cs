using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfDarwin.GameObject
{
    class Leaf : BasicObject
    {

        private Texture2D brokenLeaf;
        private Texture2D wholeLeaf;

        private Boolean leafBroken;
       
        public Leaf(int x, int y, GameBoard myboard)
            : base(myboard)
        {
            this.leafBroken = false;

        }

        /// <summary>
        /// Get whether or not this leaf is whole, or broken
        /// </summary>
        /// <returns>True for when the leaf is broken, and false for when it is whole.</returns>
        public Boolean getLeafState()
        {
            return leafBroken;
        }

        /// <summary>
        /// Once Darwin steps on the leaf, it is now broken
        /// </summary>
        public void breakLeaf()
        {
            leafBroken = true;
        }

    }
}
