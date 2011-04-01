using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    public class Snake: Zombie
    {
        protected Texture2D snakeTexture;
        private int snakeDelayCounter = 0;
        public bool delaySnakeCounter = false;
        public bool allowedToWalk= false;
        public bool lineOfSight { get; set; }
        public int spriteStripCounter = 0;

        public enum Direction {Up, Down, Left, Right};
        public Direction lineOfSightDirection;

        public Snake(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard)
            : base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard)
        {
            ZOMBIE_MOVE_RATE = 20;
            source.X = 0;
            isAlive = true;
        }

        public new void LoadContent(Texture2D snakeTex)
        {
            snakeTexture = snakeTex;
        }

        public new void Update(GameTime gameTime, Darwin darwin, LinkedList<Flame> flames)
        {
            //base.Update(gameTime);
            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                allowedToWalk = true;
                this.checkForFieryDeath(flames);

                if (snakeDelayCounter > (ZOMBIE_MOVE_RATE * 5))
                {
                    snakeDelayCounter = 0;
                    delaySnakeCounter = false;
                }

                if (!delaySnakeCounter && (isDarwinAboveSnakeSomewhere(darwin) || isDarwinBelowSnakeSomewhere(darwin) || isDarwinRightOfSnakeSomewhere(darwin) || isDarwinLeftOfSnakeSomewhere(darwin)))
                {
                    lineOfSight = true;

                    if (isDarwinAboveSnakeSomewhere(darwin) ){

                        lineOfSightDirection = Direction.Up;
                    }

                    if (isDarwinBelowSnakeSomewhere(darwin)){

                        lineOfSightDirection = Direction.Down;
                    }

                    if (isDarwinRightOfSnakeSomewhere(darwin))
                    {
                        lineOfSightDirection = Direction.Right;
                    }

                    if (isDarwinLeftOfSnakeSomewhere(darwin))
                    {
                        lineOfSightDirection = Direction.Left;   
                    }
                }
                else
                {
                    lineOfSight = false;
                    this.RandomWalk();
                }
                movecounter = 0;
            }
            else
            {
                allowedToWalk = false;
            }

            movecounter++;
            snakeDelayCounter++;
        }

        public void checkForFieryDeath(LinkedList<Flame> flames)
        {
            foreach (Flame flame in flames)
            {
                if (this.isOnTop(flame))
                {
                    this.isAlive = false;
                }
            }
        }

        public void pushDarwinUp(Darwin darwin)
        {
                darwin.MoveUp();

                if (board.isGridPositionOpen(this.X, this.Y - 1))
                {
                    this.MoveUp();
                }
        }

        public void pushDarwinDown(Darwin darwin)
        {
            darwin.MoveDown();

            if (board.isGridPositionOpen(this.X, this.Y + 1))
            {
                this.MoveDown();
            }
        }

        public void pushDarwinRight(Darwin darwin)
        {
            darwin.MoveRight();

            if (board.isGridPositionOpen(this.X + 1, this.Y))
            {
                this.MoveRight();
            }
        }

        public void pushDarwinLeft(Darwin darwin)
        {
            darwin.MoveLeft();

            if (board.isGridPositionOpen(this.X - 1, this.Y))
            {
                this.MoveLeft();
            }
        }

        public void backOffDown()
        {
            this.lineOfSight = false;
            this.delaySnakeCounter = true;

            if(board.isGridPositionOpen(this.X, this.Y + 1))
            {
                this.MoveDown();
            }

            if (board.isGridPositionOpen(this.X, this.Y + 1))
            {
                this.MoveDown();
            }
        }

        public void backOffUp()
        {
            this.lineOfSight = false;
            this.delaySnakeCounter = true;

            if(board.isGridPositionOpen(this.X, this.Y - 1)){
                this.MoveUp();
            }

            if (board.isGridPositionOpen(this.X, this.Y - 1))
            {
                this.MoveUp();
            }
        }

        public void backOffRight()
        {
            this.lineOfSight = false;
            this.delaySnakeCounter = true;

            if (board.isGridPositionOpen(this.X + 1, this.Y))
            {
                this.MoveRight();
            }

            if (board.isGridPositionOpen(this.X + 1, this.Y))
            {
                this.MoveRight();
            }
        }

        public void backOffLeft()
        {
            this.lineOfSight = false;
            this.delaySnakeCounter = true;

            if (board.isGridPositionOpen(this.X - 1, this.Y))
            {
                this.MoveLeft();
            }

            if (board.isGridPositionOpen(this.X - 1, this.Y))
            {
                this.MoveLeft();
            }
        }

        public bool isDarwinAboveSnakeSomewhere(Darwin darwin)
        {
            if(darwin.X == this.X && darwin.Y < this.Y){
                return true;
            }
            return false;
        }

        public bool isDarwinBelowSnakeSomewhere(Darwin darwin)
        {
            if (darwin.X == this.X && darwin.Y > this.Y)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinRightOfSnakeSomewhere(Darwin darwin)
        {
            if (darwin.Y == this.Y && darwin.X > this.X)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinLeftOfSnakeSomewhere(Darwin darwin)
        {
            if (darwin.Y == this.Y && darwin.X < this.X)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinDirectlyAboveSnake(Darwin darwin)
        {
            if ((this.Y - 1 == darwin.Y) && darwin.X == this.X)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinDirectlyBelowSnake(Darwin darwin)
        {
            if ((this.Y + 1 == darwin.Y) && darwin.X == this.X)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinDirectlyRightOfSnake(Darwin darwin)
        {
            if ((this.X + 1 == darwin.X) && darwin.Y == this.Y)
            {
                return true;
            }
            return false;
        }

        public bool isDarwinDirectlyLeftOfSnake(Darwin darwin)
        {
            if ((this.X - 1 == darwin.X) && darwin.Y == this.Y)
            {
                return true;
            }
            return false;
        }

        public bool isSnakeInPit()
        {
            if (this.X <= 4 || this.X >= 28 || this.Y <= 4 || this.Y >= 19)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (this.isAlive)
            {
                if (allowedToWalk)
                {
                    if (spriteStripCounter == 1)
                    {
                        spriteStripCounter = 0;
                        if (this.lineOfSightDirection.Equals(Direction.Right) || this.lineOfSightDirection.Equals(Direction.Down))
                        {
                            this.source.X = 0;
                        }
                        else
                        {
                            this.source.X = 128;
                        }
                    }
                    else
                    {
                        spriteStripCounter++;
                        if (this.lineOfSightDirection.Equals(Direction.Right) || this.lineOfSightDirection.Equals(Direction.Down))
                        {
                            this.source.X = 64;
                        }
                        else
                        {
                            this.source.X = 192;
                        }
                    }
                }

                spriteBatch.Draw(snakeTexture, this.destination, this.source, Color.White);
            }
        }
    }
}
