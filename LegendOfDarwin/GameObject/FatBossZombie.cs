﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class FatBossZombie : Zombie
    {
        // mode for zombie
        enum Stage { Nothing, Gape, Walk, Chuck }
        private Stage curMode;

        private Darwin darwin;

        private Random ran;

        private int count;

        //private int health;

        //private LinkedList<BabyZombie> babies;

        public FatBossZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) :
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            //health = 4;

            darwin = dar;

            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            source = new Rectangle(7, 0, 47, 64);

            setEventLag(75);

            count = 0;
            ran = new Random();
            ZOMBIE_MOVE_RATE = 50;
        }

        public void reset()
        {
            board.setGridPositionOpen(this.X, this.Y);
            board.setGridPositionOpen(this.X + 1, this.Y);
            board.setGridPositionOpen(this.X + 2, this.Y);
            board.setGridPositionOpen(this.X, this.Y + 1);
            board.setGridPositionOpen(this.X + 1, this.Y + 1);
            board.setGridPositionOpen(this.X + 2, this.Y + 1);
            board.setGridPositionOpen(this.X, this.Y + 2);
            board.setGridPositionOpen(this.X + 1, this.Y + 2);
            board.setGridPositionOpen(this.X + 2, this.Y + 2);
        }

        public new void LoadContent(Texture2D texIn)
        {
            zombieTexture = texIn;
        }

        // checks if darwin is immediately to the left of the zombie, that is in range and not up or down
        public bool isDarwinToTheLeft() 
        {

            bool result = false;

            if (darwin.Y == this.Y || darwin.Y == this.Y + 1 || darwin.Y == this.Y + 2)
            {

                for (int i = 1; i < 6; i++)
                {
                    if (this.isZombieInRange(this.X-i,this.Y) && darwin.X==this.X-i)
                        result = true;

                }

            }

            return result; 
        }

        public bool isInCollision(Darwin myDarwin) 
        {
            if ((this.X == darwin.X && this.Y == darwin.Y) || (this.X+1 == darwin.X && this.Y == darwin.Y) || (this.X+2 == darwin.X && this.Y == darwin.Y)
                || (this.X == darwin.X && this.Y+1 == darwin.Y) || (this.X+1 == darwin.X && this.Y+1 == darwin.Y) || (this.X+2 == darwin.X && this.Y+1 == darwin.Y)
                || (this.X == darwin.X && this.Y+2 == darwin.Y) || (this.X+1 == darwin.X && this.Y+2 == darwin.Y) || (this.X+2 == darwin.X && this.Y+2 == darwin.Y))
                return true;
            else
                return false;
        }

        public new void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                setEventFalse();
                randomWalk();
                movecounter = 0;
            }
            movecounter++;

        }

        public new void Draw(SpriteBatch sb)
        {
            sb.Draw(zombieTexture, destination, source, Color.White);
        }

        private void randomWalk()
        { 
            int i = ran.Next(1, 5);

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

            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            setEventFalse();
            if(count == 80)
            {
                count = 0;
            }
            
            count++;

            if (count > 40)
            {
                destination.Height = board.getSquareLength() * 3 + (int)(board.getSquareLength() * .1);
                destination.Width = board.getSquareWidth() * 3 + (int)(board.getSquareWidth() * .1);
            }
            else
            {
                destination.Height = board.getSquareLength() * 3;
                destination.Width = board.getSquareWidth() * 3;
            }
            
        }

        private new void MoveUp()
        {
            if (((board.isGridPositionOpen(this.X, this.Y - 1) &&
                board.isGridPositionOpen(this.X + 1, this.Y - 1) &&
                board.isGridPositionOpen(this.X + 2, this.Y - 1)) || 
                ((darwin.X == this.X && darwin.Y == this.Y -1) || (darwin.X == this.X+1 && darwin.Y == this.Y - 1) ||
                (darwin.X == this.X+2 && darwin.Y == this.Y - 1))) && this.isZombieInRange(this.X, this.Y - 1))
            {
                board.setGridPositionOccupied(this.X, this.Y - 1);
                board.setGridPositionOccupied(this.X + 1, this.Y - 1);
                board.setGridPositionOccupied(this.X + 2, this.Y - 1);
                board.setGridPositionOpen(this.X, this.Y + 2);
                board.setGridPositionOpen(this.X + 1, this.Y + 2);
                board.setGridPositionOpen(this.X + 2, this.Y + 2);
                this.setGridPosition(this.X, this.Y - 1);
            }
        }

        private new void MoveDown()
        {
            if (((board.isGridPositionOpen(this.X, this.Y + 3) &&
                board.isGridPositionOpen(this.X + 1, this.Y + 3) &&
                board.isGridPositionOpen(this.X + 2, this.Y + 3)) || 
                ((darwin.X == this.X && darwin.Y == this.Y + 3) || (darwin.X == this.X+1 && darwin.Y == this.Y + 3) ||
                (darwin.X == this.X+2 && darwin.Y == this.Y + 3))) && this.isZombieInRange(this.X, this.Y + 1))
            {
                board.setGridPositionOccupied(this.X, this.Y + 3);
                board.setGridPositionOccupied(this.X + 1, this.Y + 3);
                board.setGridPositionOccupied(this.X + 2, this.Y + 3);
                board.setGridPositionOpen(this.X, this.Y);
                board.setGridPositionOpen(this.X + 1, this.Y);
                board.setGridPositionOpen(this.X + 2, this.Y);
                this.setGridPosition(this.X, this.Y + 1);
            }
        }

        private new void MoveLeft()
        {
            if (((board.isGridPositionOpen(this.X - 1, this.Y) &&
                board.isGridPositionOpen(this.X - 1, this.Y + 1) &&
                board.isGridPositionOpen(this.X - 1, this.Y + 2)) ||
                ((darwin.X == this.X-1 && darwin.Y == this.Y) || (darwin.X == this.X - 1 && darwin.Y == this.Y + 1) ||
                (darwin.X == this.X -1 && darwin.Y == this.Y + 2))) && this.isZombieInRange(this.X-1, this.Y))
            {
                board.setGridPositionOccupied(this.X - 1, this.Y);
                board.setGridPositionOccupied(this.X - 1, this.Y + 1);
                board.setGridPositionOccupied(this.X - 1, this.Y + 2);
                board.setGridPositionOpen(this.X + 2, this.Y);
                board.setGridPositionOpen(this.X + 2, this.Y + 1);
                board.setGridPositionOpen(this.X + 2, this.Y + 2);
                this.setGridPosition(this.X - 1, this.Y);
            }
        }

        private new void MoveRight()
        {
            if (((board.isGridPositionOpen(this.X + 3, this.Y) &&
                board.isGridPositionOpen(this.X + 3, this.Y + 1) &&
                board.isGridPositionOpen(this.X + 3, this.Y + 2)) ||
                ((darwin.X == this.X+3 && darwin.Y == this.Y) || (darwin.X == this.X+3 && darwin.Y == this.Y + 1) ||
                (darwin.X == this.X+3 && darwin.Y == this.Y + 2))) && this.isZombieInRange(this.X+1, this.Y))
            {
                board.setGridPositionOccupied(this.X + 3, this.Y);
                board.setGridPositionOccupied(this.X + 3, this.Y + 1);
                board.setGridPositionOccupied(this.X + 3, this.Y + 2);
                board.setGridPositionOpen(this.X , this.Y);
                board.setGridPositionOpen(this.X , this.Y + 1);
                board.setGridPositionOpen(this.X , this.Y + 2);
                this.setGridPosition(this.X + 1, this.Y);
            }
        }

    }
}
