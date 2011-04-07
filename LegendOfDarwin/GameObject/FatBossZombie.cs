using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class FatBossZombie : Zombie
    {
        // mode for zombie
        enum Stage { Nothing, Gape, Walk, Chuck }
        private Stage curMode;

        private Darwin darwin;
        private Texture2D explosionTexture;
        private SoundEffect explosionSound;

        // for random walk and randomly opening his mouth
        private Random ran;
        private Random ran1;

        // for breathing thingy
        private int count;
        private int explodeCount;
        private int secondExplodeCount = 0;
        private int thirdExplodeCount = 0;
        private int fourthExplodeCount = 0;
        private bool exploding = false;
        private bool secondExplosion = false;
        private bool thirdExplosion = false;
        private bool fourthExplosion = false;
        public bool explodeFirstWaveOfBabies = true;
        public bool explodeSecondWaveOfBabies = true;
        public bool explodeThirdWaveOfBabies = true;

        private bool allowedToWalk;
        private int spriteStripCounter = 0;
        private bool eatingBaby = false;
        private int eatingCounter = 0;
        private Rectangle[] explodeSource;

        // is this in mode where his mouth is open
        private bool gapeMode = false;
        private int gapeCount = 0;

        // number of babies boss must eat to die
        private int health;

        //private LinkedList<BabyZombie> babies;

        public FatBossZombie(int x, int y, int maxX, int minX, int maxY, int minY, Darwin dar, GameBoard gb) :
            base(x, y, maxX, minX, maxY, minY, gb)
        {
            health = 4;
            darwin = dar;

            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            source = new Rectangle(0, 0, 128, 128);

            setEventLag(75);

            count = 0;
            explodeCount = 0;

            explodeSource = new Rectangle[4];
            explodeSource[0] = new Rectangle(0, 0, 75, 90);
            explodeSource[1] = new Rectangle(0, 0, 75, 90);
            explodeSource[2] = new Rectangle(76, 0, 87, 90);
            explodeSource[3] = new Rectangle(169, 0, 101, 90);

            ran = new Random();
            ran1 = new Random();
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
            gapeMode = false;
            this.setZombieAlive(true);
            health = 4;
            explodeCount = 0;
        }

        public void LoadContent(Texture2D texIn, Texture2D explosion, SoundEffect eSound)
        {
            zombieTexture = texIn;
            explosionTexture = explosion;
            explosionSound = eSound;
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

        // checks if darwin is immediately to the right of the zombie, that is in range and not up or down
        public bool isDarwinToTheRight()
        {

            bool result = false;

            if (darwin.Y == this.Y || darwin.Y == this.Y + 1 || darwin.Y == this.Y + 2)
            {

                for (int i = 1; i < 6; i++)
                {
                    if (this.isZombieInRange(this.X + i, this.Y) && darwin.X == this.X + i)
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

        // checks if darwin is in front of boss and boss has his mouth open and darwin is a human
        public bool canDarwinBeEaten() 
        {
            if ((darwin.X == this.X || darwin.X == this.X + 1 || darwin.X == this.X + 2)) 
            {
                if (darwin.Y == this.Y + 3 && !darwin.isZombie() && gapeMode && this.isZombieAlive())
                    return true;
            }

            return false;
        }

        // checks if a baby is in front of boss and boss has mouth open
        public bool canBabyBeEaten(BabyZombie baby)
        {
            if ((baby.X == this.X || baby.X == this.X + 1 || baby.X == this.X + 2))
            {
                if (baby.Y == this.Y + 3 && gapeMode && !eatingBaby)
                    return true;
            }

            return false;
        }

        public void resetGapeMode() 
        { 
            gapeMode = false;
            gapeCount = 0;
        }

        /**
         * sets the zombie to alive (true) or dead (false)
         * if it is dead it won't be drawn
         * opens up all zombie boss tiles
         */
        public new void setZombieAlive(bool living)
        {
            isAlive = living;
            if (isAlive == false)
            {
                // if you are killing the zombie, free up his space
                board.setGridPositionOpen(this.X, this.Y);
                board.setGridPositionOpen(this.X + 1,this.Y);
                board.setGridPositionOpen(this.X + 2, this.Y);
                board.setGridPositionOpen(this.X , this.Y+1);
                board.setGridPositionOpen(this.X + 1, this.Y+1);
                board.setGridPositionOpen(this.X + 2, this.Y+1);
                board.setGridPositionOpen(this.X, this.Y+2);
                board.setGridPositionOpen(this.X + 1, this.Y+2);
                board.setGridPositionOpen(this.X + 2, this.Y+2);
            }
        }

        /**
         * checks if babies are in eating range, kills them, modifies health if necessary
         */
        public void checkForBabyDeaths(Nursery nurseryOne, Nursery nurseryTwo) 
        {
            foreach (BabyZombie baby in nurseryOne.babies) 
            {
                if (canBabyBeEaten(baby) && baby.isZombieAlive()) 
                {
                    baby.setZombieAlive(false);
                    health--;
                    eatingBaby = true;
                    exploding = true;
                }
            }

            foreach (BabyZombie baby in nurseryTwo.babies)
            {
                if (canBabyBeEaten(baby) && baby.isZombieAlive())
                {
                    baby.setZombieAlive(false);
                    health--;
                    eatingBaby = true;
                    exploding = true;
                }
            }
        }

        public new void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            if (health <= 0)
                this.setZombieAlive(false);

            if (isDarwinToTheLeft() || isDarwinToTheRight())
                ZOMBIE_MOVE_RATE = 5;

            checkForRandomBossGape();

            if (movecounter > ZOMBIE_MOVE_RATE)
            {

                if (eatingCounter > (ZOMBIE_MOVE_RATE * 7))
                {
                    eatingCounter = 0;
                    eatingBaby = false;
                    gapeMode = false;
                }


                allowedToWalk = true;
                setEventFalse();

                if (isDarwinToTheLeft())
                    MoveLeft();
                else if (isDarwinToTheRight())
                    MoveRight();
                else if (gapeMode) 
                {
                    gapeCount++;

                    if (gapeCount > 10) 
                    {
                        gapeCount = 0;
                        gapeMode = false;
                    }
                }
                else
                {
                    ZOMBIE_MOVE_RATE = 50;
                    randomWalk();
                }

                updateBossExplosions();

                movecounter = 0;
            }
            else
            {
                allowedToWalk = false;
            }
            movecounter++;
            eatingCounter++;

        }

        private void checkForRandomBossGape()
        {
            if (!exploding && !secondExplosion && !thirdExplosion && !fourthExplosion)
            {
                int checkForGape = 0;
                checkForGape = ran1.Next(200);
                if (checkForGape == 150)
                {
                    gapeMode = true;
                }
            }
        }

        private void updateBossExplosions()
        {
            if (exploding)
            {
                explodeCount++;
                if (explodeCount == 4)
                {
                    exploding = false;
                    explodeCount = 0;
                }
                if (explodeCount == 2)
                {
                    secondExplosion = true;  
                }
            }

            if (secondExplosion)
            {
                explodeFirstWaveOfBabies = true;
                secondExplodeCount++;
                if (secondExplodeCount == 4)
                {
                    secondExplosion = false;
                    secondExplodeCount = 0;
                    explodeFirstWaveOfBabies = false;
                }
                if(secondExplodeCount == 3)
                {
                    thirdExplosion = true;
                    explodeSecondWaveOfBabies = true;
                }
            }

            if (thirdExplosion)
            {       
                thirdExplodeCount++;
                if (thirdExplodeCount == 4)
                {
                    thirdExplosion = false;
                    thirdExplodeCount = 0;
                    explodeSecondWaveOfBabies = false;

                }
                if (thirdExplodeCount == 2)
                {
                    fourthExplosion = true;
                }
            }

            if (fourthExplosion)
            {
                fourthExplodeCount++;
                if (fourthExplodeCount == 4)
                {
                    fourthExplosion = false;
                    fourthExplodeCount = 0;
                }
            }
        }

        public new void Draw(SpriteBatch sb)
        {
            
            destination.Height = board.getSquareLength() * 3;
            destination.Width = board.getSquareWidth() * 3;

            if (allowedToWalk)
            {
                if (spriteStripCounter == 1)
                {
                    spriteStripCounter = 0;
                    this.source.X = 0;
                }
                else
                {
                    spriteStripCounter++;
                    this.source.X = 128;
                }
            }

            if (eatingBaby)
            {
                this.source.X = 384;
            }
            else if (gapeMode)
            {
                this.source.X = 256;
            }

            sb.Draw(zombieTexture, destination, source, Color.White);

            if (exploding)
            {
                sb.Draw(explosionTexture, board.getPosition(this), explodeSource[explodeCount], Color.White);
            }
            if (secondExplosion)
            {
                sb.Draw(explosionTexture, board.getPosition(this.X + 1, this.Y), explodeSource[secondExplodeCount], Color.White);
            }
            if (thirdExplosion)
            {
                sb.Draw(explosionTexture, board.getPosition(this.X, this.Y + 1), explodeSource[thirdExplodeCount], Color.White);
            }
            if (fourthExplosion)
            {
                sb.Draw(explosionTexture, board.getPosition(this.X + 1, this.Y + 1), explodeSource[fourthExplodeCount], Color.White);
            }
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
