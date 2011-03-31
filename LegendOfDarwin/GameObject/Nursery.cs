using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class Nursery : BasicObject
    {
        private int maxBabies = 7;
        private int babyTimeSpawn = 300;
        private Texture2D nurseTex;
        private BabyZombie[] babies;

        private int spawnX, spawnY;

        //private Vector2[] spawnPoints;

        public Nursery(GameBoard gb, Darwin darwin)
            : base(gb)
        {
            babies = new BabyZombie[maxBabies];
            /*spawnPoints = new Vector2[10];
            for (int i = 0; i < 10; i++)
            {
                spawnPoints[i] = new Vector2();
            }
            */
            for (int i = 0; i < maxBabies; i++)
            {
                babies[i] = new BabyZombie(0, 0, 32, 0, 24, 0, darwin, gb);

                babies[i].setZombieAlive(false);
            }

            this.setEventLag(babyTimeSpawn);

            this.destination.Height = board.getSquareLength() * 3;
            this.destination.Width = board.getSquareWidth() * 2;

        }

        public new void setGridPosition(int x, int y)
        {
            //setSpawnPoints(x, y);

            this.destination.X = board.getPosition(x, y).X;
            this.destination.Y = board.getPosition(x, y).Y;
        }

        public void LoadContent(Texture2D nurseTexIn, Texture2D babyTexIn, Texture2D explodeTexIn)
        {
            nurseTex = nurseTexIn;

            foreach (BabyZombie b in babies)
            {
                b.LoadContent(babyTexIn, explodeTexIn);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(nurseTex, destination, Color.White);

            foreach (BabyZombie b in babies)
            {
                b.Draw(sb);
            }
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (BabyZombie b in babies)
            {
                b.Update(gameTime);
            }

            if (canEventHappen())
            {
                for( int i = 0; i < maxBabies; i ++)
                {
                    if (!babies[i].isZombieAlive())
                    {
                        babies[i].setZombieAlive(true);
                        babies[i].setGridPosition(spawnX, spawnY);
                        //Vector2 temp = findSpawnPoint();
                        //babies[i].setGridPosition((int)temp.X, (int)temp.Y);
                        break;
                    }
                }

                this.setEventFalse();
            }
        }

        public void setSpawnPoint(int x, int y)
        {
            spawnX = x;
            spawnY = y;
        }
        
        /*
        private Vector2 findSpawnPoint()
        {
            int mark = 100;

            for(int i = 0; i < 10; i++)
            {
                if (board.isGridPositionOpen((int)spawnPoints[i].X, (int)spawnPoints[i].Y))
                {
                    mark = i;
                    break;
                }
            }

            return new Vector2(spawnPoints[mark].X, spawnPoints[mark].Y);
        }

        private void setSpawnPoints(int x, int y)
        {
            spawnPoints[0].X = x;
            spawnPoints[0].Y = y - 1;

            spawnPoints[1].X = x - 1;
            spawnPoints[1].Y = y;

            spawnPoints[2].X = x - 1;
            spawnPoints[2].Y = y + 1;

            spawnPoints[3].X = x - 1;
            spawnPoints[3].Y = y + 2;

            spawnPoints[4].X = x;
            spawnPoints[4].Y = y + 3;

            spawnPoints[5].X = x + 1;
            spawnPoints[5].Y = y + 3;

            spawnPoints[6].X = x + 2;
            spawnPoints[6].Y = y + 2;

            spawnPoints[7].X = x + 2;
            spawnPoints[7].Y = y + 1;

            spawnPoints[8].X = x + 2;
            spawnPoints[8].Y = y;

            spawnPoints[9].X = x + 1;
            spawnPoints[9].Y = y - 1;
        }*/
    }
}
