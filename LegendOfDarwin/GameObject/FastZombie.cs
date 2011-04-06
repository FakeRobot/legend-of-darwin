using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfDarwin.GameObject
{
    /// <summary>
    /// step on a leaf -> this zombie goes to leaf -> looks around -> if sees darwin, chase; else sleep
    /// </summary>
    class FastZombie : Zombie
    {

        // leaves that this zombie is 'listening' to
        public LinkedList<Leaf> watchedLeaves;

        // the broken leaf that the zombie will beeline to
        public Leaf brokenLeaf;

        // the zombie's chase direction
        public enum Dir { Up, Down, Left, Right };
        public Dir chaseDir;

        private Boolean sleeping = true;
        public Boolean chasingDarwin = false;

        int sleepyTime = 0;

        public FastZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard)
        {
            // fast zombies should move faster, obviously
            ZOMBIE_MOVE_RATE = 4;
            this.watchedLeaves = new LinkedList<Leaf>();
            visionMaxX = 2;
            visionMaxY = 2;
        }

        public void LoadContent(Texture2D fastZombieTexture)
        {
            zombieTexture = fastZombieTexture;
        }

        /// <summary>
        /// Adds a leaf to the list
        /// </summary>
        /// <param name="leaf">The leaf to add</param>
        public void addLeaf(Leaf leaf)
        {
            this.watchedLeaves.AddFirst(leaf);
        }

        /// <summary>
        /// Removes a leaf from the list
        /// </summary>
        /// <param name="leaf">The leaf to remove</param>
        public void removeLeaf(Leaf leaf)
        {
            this.watchedLeaves.Remove(leaf);
        }

        /// <summary>
        /// Gets whether or not this zombie is sleeping
        /// </summary>
        /// <returns>True if sleeping, False if not.</returns>
        public Boolean isSleeping()
        {
            return this.sleeping;
        }

        /// <summary>
        /// Wakes up the zombie so he can start chasing Darwin
        /// </summary>
        public void wakeUp()
        {
            this.sleeping = false;
        }

        /// <summary>
        /// The zombie goes back to sleep because he couldn't see Darwin
        /// </summary>
        public void goBackToSleep()
        {
            this.sleeping = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaf"></param>
        public void assignLeaf(Leaf leaf)
        {
            this.brokenLeaf = leaf;
        }

        /// <summary>
        /// Moves the Zombie towards the newly broken leaf.
        /// </summary>
        /// <param name="leaf">The leaf that was broken by Darwin</param>
        public void goToLeaf(Leaf leaf)
        {
            // do this better later
            if (this.X < leaf.X)
                this.MoveRight();
            else if (this.X > leaf.X)
                this.MoveLeft();
            else if (this.Y < leaf.Y)
                this.MoveDown();
            else if (this.Y > leaf.Y)
                this.MoveUp();
        }

        /// <summary>
        /// At this point on the grid, look around for Darwin.
        /// </summary>
        public void lookForDarwin(Darwin darwin)
        {
            if (this.isPointInVision(darwin.X,darwin.Y))
                chaseDarwin(darwin);
            else
                this.goBackToSleep();
            // if see darwin
                // chaseDarwin()
            // else
                // this.goBackToSleep()
        }

        /// <summary>
        /// Once the zombie has seen Darwin, he wants to chase after him to eat his brains
        /// </summary>
        public void chaseDarwin(Darwin darwin)
        {
            // do this better later
            if (this.X < darwin.X)
                this.MoveRight();
            else if (this.X > darwin.X)
                this.MoveLeft();
            else if (this.Y < darwin.Y)
                this.MoveDown();
            else if (this.Y > darwin.Y)
                this.MoveUp();
        }

        public void Update(GameTime gametime, Darwin darwin, Brain brain)
        {
            //base.Update(gametime, darwin, brain);
            if (movecounter > ZOMBIE_MOVE_RATE)
            {
                // if the zombie is not sleeping
                if (!this.isSleeping())
                {
                    if (!chasingDarwin)
                        this.goToLeaf(brokenLeaf);

                    if (this.isOnTop(brokenLeaf))
                        lookForDarwin(darwin);
                }

                movecounter = 0;
            }

            movecounter++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // hard code the source rectangle size here incase we want to animate the sleeping
            Rectangle source;
            switch (this.isSleeping())
            {
                case (true):
                    source = new Rectangle(64, 0, 64, 64);
                    // cycle through the sleeping ones
                    sleepyTime++;
                    if (sleepyTime > 100)
                        sleepyTime = 0;
                    else if (sleepyTime < 50)
                        source.X = 64;
                    else
                        source.X = 128;

                    spriteBatch.Draw(zombieTexture, destination, source, Color.White);
                    break;
                case (false):
                    // the first sprite is the awake one
                    source = new Rectangle(0, 0, 64, 64);
                    spriteBatch.Draw(zombieTexture, destination, source, Color.White);
                    break;
                default:
                    throw new Exception("lolwut");
            }
        }

    }
}
