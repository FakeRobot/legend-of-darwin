using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class PyroZombie : Zombie
    {

        // directional stuffs
        protected enum Dir { Up, Down, Left, Right };

        // texture to use when zombie is flaming darwin
        protected Texture2D flamingZombieTexture; // deprecated ???
        private SoundEffect flameSound;
        private bool playFlameSound = true;

        // boolean to know whether or not we're patrolling or shooting flames
        public Boolean patrolling;

        // patrol path nodes
        protected Vector2 currentPoint;
        protected Vector2 nextPoint;

        public Boolean killedDarwin;

        // refer to zombie constructor
        public PyroZombie(int startX, int startY, int maxX, int minX, int maxY, int minY, GameBoard myboard)
            : base(startX, startY, maxX, minX, maxY, minY, myboard)
        {
            allowVision = true;
            visionMaxX = 4;
            visionMaxY = 4;
            this.patrolling = true;
            ZOMBIE_MOVE_RATE = 35;
            this.killedDarwin = false;
        }

        public void LoadContent(Texture2D pyroZombieTexture, Texture2D zombieFlamingTex, SoundEffect fSound)
        {
            zombieTexture = pyroZombieTexture;
            this.flamingZombieTexture = zombieFlamingTex;
            flameSound = fSound;
        }

        /// <summary>
        /// Checks whether or not the PyroZombie is patrolling.
        /// </summary>
        /// <returns>True if it is, False otherwise.</returns>
        public Boolean isPatrolling()
        {
            return this.patrolling;
        }

        /// <summary>
        /// Sets the current patrol path point that we want this zombie to go to.
        /// </summary>
        /// <param name="curr">Vector containing the x,y co-ordinates for the point.</param>
        public void setCurrentPatrolPoint(Vector2 curr)
        {
            this.currentPoint = curr;
        }

        /// <summary>
        /// Sets the next patrol path point that we want this zombie to go to.
        /// </summary>
        /// <param name="next">Vector containing the x,y co-ordinates for the point.</param>
        public void setNextPatrolPoint(Vector2 next)
        {
            this.nextPoint = next;
        }

        /// <summary>
        /// Returns the current patrol path point that this zombie is going to.
        /// </summary>
        /// <returns>Vector containing the x,y co-ordinates for the point.</returns>
        public Vector2 getCurrentPatrolPoint()
        {
            return this.currentPoint;
        }

        /// <summary>
        /// Returns the next patrol path point that this zombie is going to.
        /// </summary>
        /// <returns>Vector containing the x,y co-ordinates for the point.</returns>
        public Vector2 getNextPatrolPoint()
        {
            return this.nextPoint;
        }

        /// <summary>
        /// Simple function to tell this zombie which direction we want him to move in, 
        /// base upon the point given to it.
        /// </summary>
        /// <param name="point">The Vector containing the x,y co-ordinates we want this zombie to walk to.</param>
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

        /// <summary>
        /// Plays the flamethrower sound.
        /// </summary>
        public void doFlameSound()
        {
            if (playFlameSound)
            {
                flameSound.Play();
                playFlameSound = false;
            }
        }

        public void Update(Darwin darwin)
        {
            // once the zombie can move
            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                // check if he see's darwin
                // might be better to have a ! instead of the pyrozombie just stopping his patrol
                if (this.isPointInVision(darwin.X, darwin.Y))
                {
                    // pyro zombies dislike darwin
                    if (!darwin.isZombie())
                    {
                        patrolling = false;
                    }
                }
                else
                {
                    patrolling = true;
                    playFlameSound = true;
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
                
                // reset move counter
                movecounter = 0;
            }

            movecounter++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (this.patrolling)
            {
                case (true):
                    // Don't use a source so that it shows the whole sprite
                    spriteBatch.Draw(zombieTexture, destination, Color.White);
                    break;
                case (false):
                    // flamin' tiem
                    spriteBatch.Draw(this.flamingZombieTexture, destination, Color.White);
                    break;
                default:
                    throw new Exception("failed to draw pyro zombie");
            }
        }

    }
}
