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
        private Darwin darwin;

        private Random ran;

        
        //private int health;

        //private LinkedList<BabyZombie> babies;

        public FatBossZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) :
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            //health = 4;

            darwin = dar;

            destination.Height = board.getSquareLength() * 2;
            destination.Width = board.getSquareWidth() * 2;

            source = new Rectangle(7, 0, 47, 64);

            setEventLag(55);
            ran = new Random();
        }

        public new void LoadContent(Texture2D texIn)
        {
            zombieTexture = texIn;
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (canEventHappen())
            {
                int i = ran.Next(1, 4);

                switch (i)
                {
                    case 1:
                        MoveUp();
                        break;
                    case 2:
                        MoveDown();
                        break;
                    case 3:
                        MoveLeft();
                        break;
                    case 4:
                        MoveRight();
                        break;
                }

                setEventFalse();
            }
        }

        public new void Draw(SpriteBatch sb)
        {
            destination.Height = board.getSquareLength() * 2;
            destination.Width = board.getSquareWidth() * 2;
            sb.Draw(zombieTexture, destination, source, Color.White);
        }

        private new void MoveUp()
        {
            if (board.isGridPositionOpen(this.X, this.Y - 1) &&
                board.isGridPositionOpen(this.X + 1, this.Y - 1))
            {
                board.setGridPositionOccupied(this.X, this.Y - 1);
                board.setGridPositionOccupied(this.X + 1, this.Y - 1);
                board.setGridPositionOpen(this.X, this.Y + 1);
                board.setGridPositionOpen(this.X + 1, this.Y + 1);
                this.setGridPosition(this.X, this.Y - 1);
            }
        }

        private new void MoveDown()
        {
            if (board.isGridPositionOpen(this.X, this.Y + 2) &&
                board.isGridPositionOpen(this.X + 1, this.Y + 2))
            {
                board.setGridPositionOccupied(this.X, this.Y + 2);
                board.setGridPositionOccupied(this.X + 1, this.Y + 2);
                board.setGridPositionOpen(this.X, this.Y);
                board.setGridPositionOpen(this.X + 1, this.Y);
                this.setGridPosition(this.X, this.Y + 1);
            }
        }

        private new void MoveLeft()
        {
            if (board.isGridPositionOpen(this.X - 1, this.Y) &&
                board.isGridPositionOpen(this.X - 1, this.Y + 1))
            {
                board.setGridPositionOccupied(this.X - 1, this.Y);
                board.setGridPositionOccupied(this.X - 1, this.Y + 1);
                board.setGridPositionOpen(this.X + 1, this.Y);
                board.setGridPositionOpen(this.X + 1, this.Y + 1);
                this.setGridPosition(this.X - 1, this.Y);
            }
        }

        private new void MoveRight()
        {
            if (board.isGridPositionOpen(this.X + 2, this.Y) &&
                board.isGridPositionOpen(this.X + 2, this.Y + 1))
            {
                board.setGridPositionOccupied(this.X + 2, this.Y);
                board.setGridPositionOccupied(this.X + 2, this.Y + 1);
                board.setGridPositionOpen(this.X , this.Y);
                board.setGridPositionOpen(this.X , this.Y + 1);
                this.setGridPosition(this.X + 1, this.Y);
            }
        }

    }
}
