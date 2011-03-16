using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LegendOfDarwin.MenuObject;

namespace LegendOfDarwin.GameObject
{
    class Potion : BasicObject
    {
        private Texture2D potionTex;
        private bool isConsumed;
        private const int healthReplenished = 5;

        public Potion(GameBoard board) : base(board)
        {
            isConsumed = false;
            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOccupied(this);
            }
        }

        public void LoadContent(Texture2D potTex)
        {
            potionTex = potTex;
        }

        public void Update(GameTime gameTime, KeyboardState ks, Darwin darwin, ZombieTime zTime)
        { 
            // grab us the current
            LegendOfDarwin.Darwin.Dir facing = darwin.facing;

            // check switch position in relation to darwin's position + facing direction 
            switch (facing)
            {
                case (LegendOfDarwin.Darwin.Dir.Left):
                    if (((this.X + 1) == darwin.X) && (this.Y == darwin.Y))
                    {
                        consumePotion(zTime);
                    }
                    break;
                case (LegendOfDarwin.Darwin.Dir.Right):
                    if (((this.X - 1) == darwin.X) && (this.Y == darwin.Y))
                    {
                        consumePotion(zTime);
                    }
                    break;
                case (LegendOfDarwin.Darwin.Dir.Up):
                    if ((this.X == darwin.X) && ((this.Y + 1) == darwin.Y))
                    {
                        consumePotion(zTime);
                    }
                    break;
                case (LegendOfDarwin.Darwin.Dir.Down):
                    if ((this.X == darwin.X) && ((this.Y - 1) == darwin.Y))
                    {
                        consumePotion(zTime);
                    }
                    break;
            }
        }

        private void consumePotion(ZombieTime zTime)
        {
            board.setGridPositionOpen(this);
            isConsumed = true;

            int updateTime = zTime.getTime() - healthReplenished;
            zTime.setTime(updateTime);

        }

        public void reset()
        {
            isConsumed = false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (isConsumed == false)
            {
                spriteBatch.Draw(potionTex, this.destination, Color.White);
            }
        }
    }
}
