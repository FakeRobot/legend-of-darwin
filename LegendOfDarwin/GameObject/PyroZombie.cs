using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class PyroZombie : Zombie
    {

        // directional stuffs
        protected enum Dir { Up, Down, Left, Right };

        // need directions to shoot flames and directions to walk
        protected Dir flameDir;
        protected Dir walkDir;

        protected Boolean isShootingFlames;
        protected Boolean patrolling;

        protected Texture2D flame;

        public PyroZombie(int startX, int startY, int maxX, int minX, int maxY, int minY, GameBoard myboard)
            : base(startX, startY, maxX, minX, maxY, minY, myboard)
        {
            allowVision = true;
            visionMaxX = 7;
            visionMaxY = 7;
            this.isShootingFlames = false;
            this.patrolling = true;
        }

        public void LoadContent(Texture2D pyroZombieTexture, Texture2D flameTexture)
        {
            zombieTexture = pyroZombieTexture;
            this.flame = flameTexture;
        }

        public void setDirection(int dir)
        {
            switch (dir)
            {
                case(3):
                    this.walkDir = Dir.Down;
                    break;
                case(1):
                    this.walkDir = Dir.Up;
                    break;
                case(4):
                    this.walkDir = Dir.Left;
                    break;
                case(2):
                    this.walkDir = Dir.Right;
                    break;
                default:
                    throw new Exception("Cant set walk direction for shit");
            }
        }

        public void Update(Darwin darwin)
        {
            if (!this.patrolling)
            {
                switch (this.walkDir)
                {
                    case (Dir.Down):
                        this.MoveDown();
                        break;
                    case (Dir.Up):
                        this.MoveUp();
                        break;
                    case (Dir.Left):
                        this.MoveLeft();
                        break;
                    case (Dir.Right):
                        this.MoveRight();
                        break;
                    default:
                        throw new Exception("Cant walk for shit");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.isShootingFlames)
            {
                case (true):
                    break;
                case (false):
                    spriteBatch.Draw(zombieTexture, destination, source, Color.White);
                    break;
                default:
                    throw new Exception("failed to draw pyro zombie");
            }
        }

    }
}
