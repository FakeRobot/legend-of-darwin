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
        private struct vect { public int x; public int y;}

        private int maxBabies = 7;
        private int babyTimeSpawn = 300;
        private Texture2D nurseTex;
        private BabyZombie[] babies;

        private vect[] spawnPoints;

        public Nursery(GameBoard gb, Darwin darwin)
            : base(gb)
        {
            babies = new BabyZombie[maxBabies];
            spawnPoints = new vect[10];

            for (int i = 0; i < maxBabies; i++)
            {
                babies[i] = new BabyZombie(0, 0, 15, 5, 15, 5, darwin, gb);

                babies[i].setZombieAlive(false);
            }

            this.setEventLag(babyTimeSpawn);

            this.destination.Height = board.getSquareLength() * 3;
            this.destination.Width = board.getSquareWidth() * 2;

        }

        public new void setGridPosition(int x, int y)
        {
            this.destination.X = board.getPosition(x, y).X;
            this.destination.Y = board.getPosition(x, y).Y;

            setSpawnPoints(x, y);
        }

        public void LoadContent(Texture2D nurseTexIn, Texture2D babyTexIn)
        {
            nurseTex = nurseTexIn;

            foreach (BabyZombie b in babies)
            {
                b.LoadContent(babyTexIn);
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
                foreach (BabyZombie b in babies)
                {
                    if (!b.isZombieAlive())
                    {
                        b.setZombieAlive(true);
                        b.setDestination(findSpawnPoint());
                        break;
                    }
                }

                this.setEventFalse();
            }
        }

        private Rectangle findSpawnPoint()
        {
            foreach (vect v in spawnPoints)
            {
                if (board.isGridPositionOpen(v.x, v.y))
                {
                    return board.getPosition(v.x, v.y);
                }
            }

            return new Rectangle();
        }

        private void setSpawnPoints(int x, int y)
        {
            spawnPoints[0].x = x;
            spawnPoints[0].y = y - 1;

            spawnPoints[1].x = x - 1;
            spawnPoints[1].y = y;

            spawnPoints[2].x = x - 1;
            spawnPoints[2].y = y + 1;

            spawnPoints[3].x = x - 1;
            spawnPoints[3].y = y + 2;

            spawnPoints[4].x = x;
            spawnPoints[4].y = y + 3;

            spawnPoints[5].x = x + 1;
            spawnPoints[5].y = y + 3;

            spawnPoints[6].x = x + 2;
            spawnPoints[6].y = y + 2;

            spawnPoints[7].x = x + 2;
            spawnPoints[7].y = y + 1;

            spawnPoints[8].x = x + 2;
            spawnPoints[8].y = y;

            spawnPoints[9].x = x + 1;
            spawnPoints[9].y = y - 1;
        }
    }
}
