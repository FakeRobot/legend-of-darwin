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

        private int babyCount, babyCountTwo;
        private Rectangle[] babySource;

        public bool goingToExplode, exploding;
        private Texture2D explodeTex;
        private int explodeCount;
        private Rectangle[] explodeSource;
        public enum Direction { Up, Down, Left, Right };
        private Direction darwinDirection;

        public BabyZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) : 
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            darwin = dar;

            babyCount = 0;
            babyCountTwo = 0;
            babySource = new Rectangle[6];
            babySource[0] = new Rectangle(0, 0, 64, 64);
            babySource[1] = new Rectangle(65, 0, 64, 64);
            babySource[2] = new Rectangle(128, 0, 64, 64);
            babySource[3] = new Rectangle(192, 0, 64, 64);
            babySource[4] = new Rectangle(256, 0, 64, 64);
            babySource[5] = new Rectangle(320, 0, 64, 64);

            goingToExplode = false;
            exploding = false;
            explodeCount = 0;
            explodeSource = new Rectangle[3];
            explodeSource[0] = new Rectangle(0, 0, 75, 90);
            explodeSource[1] = new Rectangle(76, 0, 87, 90);
            explodeSource[2] = new Rectangle(169, 0, 101, 90);

            this.setEventLag(40);
        }

        public void reset()
        {
            this.setEventLag(40);
            babyCount = 0;
            babyCountTwo = 0;
            goingToExplode = false;
            exploding = false;
            explodeCount = 0;
            this.setZombieAlive(false);
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
                if (exploding)
                {
                    explodeCount++;
                    if (explodeCount == 3)
                    {
                        explodeCount = 0;
                        exploding = false;
                        setZombieAlive(false);
                        setEventLag(40);
                    }
                }
                else if (goingToExplode)
                {
                    babyCount++;
                    if (babyCount == 3)
                    {
                        babyCountTwo++;
                        babyCount = 0;
                        if (babyCountTwo == 5)
                        {
                            babyCount = 0;
                            babyCountTwo = 0;
                            exploding = true;
                            goingToExplode = false;
                        }
                    }
                }
                else if (isZombieAlive())
                {
                    this.moveTowardsDarwin(darwin);
                    updateFacingDarwin(darwin);
                    if (nearDarwin())
                    {
                        goingToExplode = true;
                        setEventLag(5);
                    }
                }
                this.setEventFalse();
            }
        }

        private void updateFacingDarwin(Darwin darwin)
        {
            if (darwin.X <= this.X)
            {
                darwinDirection = Direction.Left;
            }
            else
            {
                darwinDirection = Direction.Right;
            }

        }

        public new void Draw(SpriteBatch sp)
        {
            if (exploding)
            {
                sp.Draw(explodeTex, board.getPosition(this), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X, this.Y + 1), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X, this.Y - 1), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X + 1, this.Y), explodeSource[explodeCount], Color.White);
                sp.Draw(explodeTex, board.getPosition(this.X - 1, this.Y), explodeSource[explodeCount], Color.White);
            }
            else if (goingToExplode)
            {
                if (babyCount == 0)
                {
                    if (darwinDirection.Equals(Direction.Left))
                    {
                        sp.Draw(zombieTexture, destination, babySource[1], Color.White);
                    }
                    else
                    {
                        sp.Draw(zombieTexture, destination, babySource[4], Color.White);
                    }
                }
                else
                {
                    if (darwinDirection.Equals(Direction.Left))
                    {
                        sp.Draw(zombieTexture, destination, babySource[2], Color.White);
                    }
                    else
                    {
                        sp.Draw(zombieTexture, destination, babySource[5], Color.White);
                    }
                }
                
            }
            else if (this.isZombieAlive())
            {
                if (darwinDirection.Equals(Direction.Right))
                {
                    sp.Draw(zombieTexture, destination, babySource[3], Color.White);
                }
                else
                {
                    sp.Draw(zombieTexture, destination, babySource[0], Color.White);
                }
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

            if (xDist + yDist < 2)
            {
                return true;
            }
            return false;
        }
    }
}
