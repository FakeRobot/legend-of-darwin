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
        private bool lineOfSight = false;
        private int snakeDelayCounter = 0;

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

            if (snakeDelayCounter > (ZOMBIE_MOVE_RATE * 2))
            {
                lineOfSight = true;
                snakeDelayCounter = 0;
            }

            //base.Update(gameTime);
            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                if(isDarwinAboveSnake(darwin) && lineOfSight){
                    this.MoveUp();

                    if (isDirectlyOnTopOfDarwin(darwin) && board.isGridPositionOpen(darwin.X, darwin.Y - 1))
                    {
                        darwin.MoveUp();

                    }
                    else if (isDirectlyOnTopOfDarwin(darwin) && !board.isGridPositionOpen(darwin.X, darwin.Y - 1))
                    {
                        this.MoveDown();
                        this.MoveDown();
                        lineOfSight = false;
                    }
                }
                else
                {
                    this.RandomWalk();
                }


                movecounter = 0;
            }

            movecounter++;
            snakeDelayCounter++;
        }

        private bool isDarwinAboveSnake(Darwin darwin)
        {
            if(darwin.X == this.X && darwin.Y < this.Y){
                return true;
            }
            return false;
        }

        public bool isDirectlyOnTopOfDarwin(Darwin darwin)
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
