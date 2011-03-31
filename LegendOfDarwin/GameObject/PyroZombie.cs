using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class PyroZombie : Zombie
    {

        // directional stuffs
        protected enum Dir { Up, Down, Left, Right };

        // need directions to shoot flames and directions to walk
        protected Dir flameDir;

        // boolean to know whether or not we're patrolling or shooting flames
        protected Boolean patrolling;

        // texture for the flame
        protected Texture2D flame;

        // patrol path nodes
        protected Vector2 currentPoint;
        protected Vector2 nextPoint;

        public PyroZombie(int startX, int startY, int maxX, int minX, int maxY, int minY, GameBoard myboard)
            : base(startX, startY, maxX, minX, maxY, minY, myboard)
        {
            allowVision = true;
            visionMaxX = 4;
            visionMaxY = 4;
            this.patrolling = true;
            ZOMBIE_MOVE_RATE = 35;
        }

        public void LoadContent(Texture2D pyroZombieTexture, Texture2D flameTexture)
        {
            zombieTexture = pyroZombieTexture;
            this.flame = flameTexture;
        }

        public Boolean isPatrolling()
        {
            return this.patrolling;
        }

        public void setCurrentPatrolPoint(Vector2 curr)
        {
            this.currentPoint = curr;
        }

        public void setNextPatrolPoint(Vector2 next)
        {
            this.nextPoint = next;
        }

        public Vector2 getCurrentPatrolPoint()
        {
            return this.currentPoint;
        }

        public Vector2 getNextPatrolPoint()
        {
            return this.nextPoint;
        }

        public void moveToPoint(Vector2 point)
        {
            if (this.X < point.X)
                this.MoveRight();
            else if (this.X > point.X)
                this.MoveLeft();
            else if (this.Y < point.Y)
                this.MoveDown();
            else if (this.Y > point.Y)
                this.MoveUp();
        }

        public void Update(Darwin darwin)
        {
            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                if (this.isPointInVision(darwin.X, darwin.Y))
                {
                    patrolling = false;
                    // flame darwin up the ass
                }
                else
                {
                    patrolling = true;
                }

                // if he is patrolling
                if (this.patrolling)
                {
                    // could probably do this better
                    if ((this.X == nextPoint.X) && (this.Y == nextPoint.Y))
                    {
                        // switch patrol points
                        Vector2 temp = currentPoint;
                        setCurrentPatrolPoint(this.getNextPatrolPoint());
                        setNextPatrolPoint(temp);
                    }
                    else
                    {
                        this.moveToPoint(nextPoint);
                    }
                }

                movecounter = 0;
            }

            movecounter++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.patrolling)
            {
                case (true):
                    spriteBatch.Draw(zombieTexture, destination, source, Color.White);
                    break;
                case (false):
                    // need both flame and 
                    //spriteBatch.Draw(zombieTexture, destination, source, Color.White);
                    spriteBatch.Draw(flame, destination, source, Color.White);
                    break;
                default:
                    throw new Exception("failed to draw pyro zombie");
            }
        }

    }
}
