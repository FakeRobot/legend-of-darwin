using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class BabyZombie : Zombie
    {
        private Darwin darwin;

        public BabyZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) : 
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            darwin = dar;
        }

        public void Update(GameTime gameTime)
        {
            this.RandomWalk();
        }
    }
}
