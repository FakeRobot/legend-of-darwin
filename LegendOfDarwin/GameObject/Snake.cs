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


        public Snake(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard)
            : base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard)
        {
            ZOMBIE_MOVE_RATE = 20;
        }

        public new void LoadContent(Texture2D snakeTex)
        {
            snakeTexture = snakeTex;
        }

        public new void Update(GameTime gameTime, Darwin darwin)
        {
            //base.Update(gameTime);
            if (movecounter > ZOMBIE_MOVE_RATE)
            {

                allowedToWalk = true;

                if (snakeDelayCounter > (ZOMBIE_MOVE_RATE * 5))
                {
                    Console.Out.WriteLine("-----------" + snakeDelayCounter + " > " + (ZOMBIE_MOVE_RATE * 5));
                    snakeDelayCounter = 0;
                    delaySnakeCounter = false;
                }

                if (isDarwinAboveSnakeSomewhere(darwin))
                {
                    if (!delaySnakeCounter)
                    {
                        lineOfSight = true;
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

        public void moveSnakeUp()
        {

                this.MoveUp();


        }

        public void pushDarwinUp(Darwin darwin)
        {
                darwin.MoveUp();

                if (board.isGridPositionOpen(this.X, this.Y - 1))
                {
                    this.MoveUp();
                }
        }

        public void backOff()
        {
                this.lineOfSight = false;
                this.delaySnakeCounter = true;
                this.MoveDown();
                this.MoveDown();
                this.MoveDown();

        }

        public bool isDarwinAboveSnakeSomewhere(Darwin darwin)
        {
            if(darwin.X == this.X && darwin.Y < this.Y){
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

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(snakeTexture, this.destination, Color.White);
        }



    }
}
