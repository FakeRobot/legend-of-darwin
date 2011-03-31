using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfDarwin.GameObject
{
    class BabyZombie : Zombie
    {
        private Darwin darwin;

        public BabyZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) : 
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            darwin = dar;
            this.setEventLag(30);
        }


        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.isZombieAlive() && canEventHappen())
            {
                this.moveTowardsDarwin(darwin);
                this.setEventFalse();
            }
        }

        public new void Draw(SpriteBatch sp)
        {
            if (this.isZombieAlive())
            {
                sp.Draw(this.zombieTexture, this.destination, Color.White);
            }
        }
    }
}
