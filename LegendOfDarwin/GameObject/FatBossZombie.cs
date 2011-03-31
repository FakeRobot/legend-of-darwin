using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class FatBossZombie : Zombie
    {
        Darwin darwin;

        private int health;

        private LinkedList<BabyZombie> babies;

        public FatBossZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) :
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            health = 4;

            darwin = dar;

            destination.Height = board.getSquareLength() * 2;
            destination.Width = board.getSquareWidth() * 2;
        }

        public void LoadContent(Texture2D texIn)
        {
            zombieTexture = texIn;
        }

        public void Update(GameTime gameTime)
        { 
        
        }

        public void Draw(SpriteBatch sb)
        { 
            
        }


    }
}
