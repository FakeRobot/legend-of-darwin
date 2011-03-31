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

        private bool explode;
        private Texture2D explodeTex;
        private int explodeCount;
        private Rectangle[] explodeSource;

        public BabyZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) : 
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            darwin = dar;

            explode = false;
            explodeCount = 0;
            explodeSource = new Rectangle[3];
            explodeSource[0] = new Rectangle(0, 0, 75, 90);
            explodeSource[1] = new Rectangle(75, 0, 75, 90);
            explodeSource[2] = new Rectangle(150, 0, 75, 90);

            this.setEventLag(40);
        }

        public void LoadContent(Texture2D babyIn, Texture2D splodeIn)
        {
            base.LoadContent(babyIn);
            explodeTex = splodeIn;
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (canEventHappen())
            {
                if (explode)
                {
                    explodeCount++;


                    if (explodeCount == 3)
                    {
                        explodeCount = 0;
                        explode = false;
                    }
                    else
                    {
                        this.setZombieAlive(false);
                    }
                }
                else if (isZombieAlive())
                {
                    this.moveTowardsDarwin(darwin);
                    if (nearDarwin())
                    {
                        explode = true;
                    }
                }
                this.setEventFalse();
            }
        }

        public new void Draw(SpriteBatch sp)
        {
            if (explode)
            {
                sp.Draw(explodeTex, board.getPosition(this), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X, this.Y + 1), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X, this.Y - 1), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X + 1, this.Y), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X - 1, this.Y), explodeSource[explodeCount], Color.White);
            }
            if (this.isZombieAlive())
            {
                sp.Draw(this.zombieTexture, this.destination, Color.White);
            }
        }

        public bool nearDarwin()
        {
            int xDist, yDist;
            if (darwin.X > this.X)
            {
                xDist = darwin.X - this.X;
            }
            else
            {
                xDist = this.X - darwin.X;
            }

            if (darwin.Y > this.Y)
            {
                yDist = darwin.Y - this.Y;
            }
            else
            {
                yDist = this.Y - darwin.Y;
            }

            if (xDist + yDist == 1)
            {
                return true;
            }
            return false;
        }
    }
}
