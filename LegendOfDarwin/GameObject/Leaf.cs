using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class Leaf : BasicObject
    {
        // leaf textures
        private Texture2D brokenLeaf;
        private Texture2D wholeLeaf;

        private FastZombie parentZombie; // deprecated ???

        // bool for whether or not the leaf is broken
        private Boolean leafBroken;

        private SoundEffect leafSound;

        public Leaf( GameBoard myBoard, FastZombie parentZombie)
            : base(myBoard)
        {
            // grid location
            //this.X = x;
            //this.Y = y;

            this.parentZombie = parentZombie;

            // all leaves are initially false
            this.leafBroken = false;
        }

        public void LoadContent(Texture2D brokenLeafTex, Texture2D wholeLeafTex, SoundEffect lSound)
        {
            // textures
            this.brokenLeaf = brokenLeafTex;
            this.wholeLeaf = wholeLeafTex;
            this.leafSound = lSound;
        }

        /// <summary>
        /// Get whether or not this leaf is whole, or broken
        /// </summary>
        /// <returns>True for when the leaf is broken, and false for when it is whole.</returns>
        public Boolean getLeafState()
        {
            return leafBroken;
        }

        /// <summary>
        /// Once Darwin steps on the leaf, it is now broken
        /// </summary>
        public void breakLeaf()
        {
            leafBroken = true;
            leafSound.Play();
        }

        /// <summary>
        /// Resets the leaf state to be unbroken.
        /// </summary>
        public void resetLeaf()
        {
            leafBroken = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (leafBroken)
            {
                case (true):
                    spriteBatch.Draw(brokenLeaf, destination, Color.White);
                    break;
                case (false):
                    spriteBatch.Draw(wholeLeaf, destination, Color.White);
                    break;
                default:
                    throw new Exception("Something very bad just happened with a leaf");
            }
        }

        public void Update(Darwin darwin)
        {
            // if darwin is on top of the leaf
            if (darwin.isOnTop(this))
            {
                // we want to break it, and wake up the zombie who is listening to it
                breakLeaf();
                parentZombie.wakeUp();
                parentZombie.assignLeaf(this);
            }
        }
    }
}
