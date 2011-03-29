using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfDarwin.GameObject
{
    class PyroZombie : Zombie
    {

        // directional stuffs
        protected enum Dir {Up, Down, Left, Right};
    
        // need directions to shoot flames and directions to walk
        protected Dir flameDir;
        protected Dir walkDir;

        protected Boolean isShootingFlames;

        public PyroZombie(int startX, int startY, int maxX, int minX, int maxY, int minY, GameBoard myboard)
            : base(startX, startY, maxX, minX, maxY, minY, myboard)
        {
            allowVision = true;
            visionMaxX = 7;
            visionMaxY = 7;
            this.isShootingFlames = false;
        }

        public void LoadContent(Texture2D pyroZombieTexture)
        {
            zombieTexture = pyroZombieTexture;
        }

        public void Update(Darwin darwin)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.isShootingFlames)
            {
                case (true):
                    break;
                case (false):
                    break;
                default:
                    throw new Exception("failed to draw pyro zombie");
            }
        }

    }
}
